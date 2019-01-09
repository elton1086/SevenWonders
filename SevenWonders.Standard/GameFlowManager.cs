using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Extensions;
using SevenWonders.Factories;
using SevenWonders.Services.Contracts;
using SevenWonders.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Services
{
    public class GameFlowManager
    {
        private readonly IGamePointsManager gamePointsManager;
        private readonly ITurnManager turnManager;
        private readonly ITradeManager tradeManager;
        private readonly IDeckOfCardsManager deckManager;
        private readonly ICoreLogger logger;        

        public IList<TurnPlayer> Players { get; } = new List<TurnPlayer>();
        public IList<StructureCard> FullDeckOfCards { get; private set; }
        public IList<WonderCard> WonderCards { get; private set; }
        public IList<StructureCard> DiscardPile { get; } = new List<StructureCard>();
        public int CurrentTurn { get; private set; } = 1;
        public Age CurrentAge { get; private set; } = Age.I;

        protected IList<GamePlayer> BasePlayers
        {
            get { return Players.Select(p => (GamePlayer)p).ToList(); }
        }

        public GameFlowManager(IDeckOfCardsManager deckManager, IGamePointsManager gamePointsManager, ITradeManager tradeManager,
            ITurnManager turnManager, ICoreLogger logger)
        {
            this.deckManager = deckManager;
            this.gamePointsManager = gamePointsManager;
            this.tradeManager = tradeManager;
            this.turnManager = turnManager;
            this.logger = logger;
        }

        public void CreateNewPlayer(string name)
        {
            Players.Add(new Entities.TurnPlayer(name));
        }

        public void SetupGame()
        {
            try
            {
                logger.Info("Starting to setup the game.");
                WonderCards = deckManager.GetShuffledWonderCards();
                FullDeckOfCards = deckManager.GetShuffledDeck(Players.Count);
                tradeManager.SetupCoinsFromBank(BasePlayers);
                DrawWonderCards();
            }
            catch (Exception e)
            {
                logger.Error(e, "Could not setup game properly.");
            }
        }

        public void StartAge()
        {
            try
            {
                logger.Info("Starting age {0}.", CurrentAge.ToString());
                DealCards(CurrentAge);
                logger.Info("Clear player data.");
                Players.InitializeTurnData();
            }
            catch (Exception e)
            {
                logger.Error(e, "Could not start age " + CurrentAge.ToString());
            }
        }

        void DrawWonderCards()
        {
            logger.Info("Randomly drawing wonder cards to players.");
            foreach (var p in Players)
            {
                var wonderCard = WonderCards.First();
                WonderCards.Remove(wonderCard);
                var wonder = WonderFactory.CreateWonder(wonderCard.Name, wonderCard.Side);
                logger.Debug("Defining side {0} of wonder {1} to player: {2}", wonderCard.Side, wonderCard.Name, p.Name);
                p.SetWonder(wonder);
            }
        }

        void DealCards(Age age)
        {
            var ageCards = FullDeckOfCards.Where(c => c.Age == age).ToList();
            foreach (var p in Players)
            {
                var initialCards = ageCards.Take(ConstantValues.PLAYER_CARDS_BY_AGE).ToList();
                ageCards.RemoveRange(0, ConstantValues.PLAYER_CARDS_BY_AGE);
                p.SetSelectableCards(initialCards);
            }
        }

        public void EndAge()
        {
            tradeManager.ResolveMilitaryConflicts(Players.Select(p => (GamePlayer)p).ToList(), CurrentAge);
            if(this.CurrentAge != Age.III)
                MoveToNextAge();
        }

        public void MoveToNextAge()
        {
            ++CurrentAge;
            CurrentTurn = 1;
            StartAge();
        }

        private void DiscardLeftCards()
        {
            foreach (var p in Players)
                foreach (var c in p.SelectableCards)
                {
                    DiscardPile.Add(c);
                    logger.Debug("Discarded {0}", c.Name);
                }
        }

        public void PlayTurn()
        {
            logger.Info("Play turn {0}.", CurrentTurn);
            var uow = new UnitOfWork();
            turnManager.SetScope(uow);
            turnManager.Play(Players, DiscardPile, CurrentAge);
            uow.Commit();
            logger.Info("All plays commited.", CurrentTurn);
        }

        public void CollectTurnRewards()
        {
            var uow = new UnitOfWork();
            turnManager.SetScope(uow);
            //Right now play seventh card is the only available reward that happens multiple times,
            //it needs to be played and commited before the rest of the plays as a player could discard the card and another player use it to buy from discard pile.
            turnManager.GetMultipleTimesRewards(Players, DiscardPile, CurrentTurn, CurrentAge);            
            uow.Commit();
            //If last turn of an age, needs to discard all left cards right away
            if (CurrentTurn == ConstantValues.TURNS_BY_AGE)
            {
                logger.Debug("Moving all selectable cards left to discard pile");
                DiscardLeftCards();
            }
            turnManager.GetRewards(Players, DiscardPile, CurrentAge);
            uow.Commit();
        }

        public void EndTurn()
        {
            var direction = CurrentAge == Age.II ? "to the right" : "to the left";
            logger.Debug("Moving all selectable cards {0}", direction);
            Players.MoveSelectableCards(CurrentAge);
            logger.Info("Clear player data for new turn.");
            Players.InitializeTurnData();
            if (++CurrentTurn > ConstantValues.TURNS_BY_AGE)
                EndAge();
        }

        public void CollectPostGameRewards()
        {
            var uow = new UnitOfWork();
            turnManager.SetScope(uow);
            turnManager.GetPostGameRewards(Players.Select(p => (GamePlayer)p).ToList());
            uow.Commit();
        }

        public void ComputePoints()
        {
            logger.Info("Computing points to end the game and declare the winner.");
            gamePointsManager.ComputeVictoryPoints(this.Players.Select(p => (Player)p).ToList());
        }
    }
}
