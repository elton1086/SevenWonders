using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using SevenWonder.Factories;
using SevenWonder.Helper;
using SevenWonder.Services.Contracts;
using SevenWonder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonder.Services
{
    public class GameFlowManager
    {
        IList<ITurnPlayer> players = new List<ITurnPlayer>();
        IList<IStructureCard> fullDeckOfCards;
        IList<IStructureCard> discardPile = new List<IStructureCard>();
        IList<WonderCard> wonderCards;
        Age currentAge;
        GameStyle style;

        public IList<ITurnPlayer> Players
        {
            get { return this.players; }
        }

        public IList<IStructureCard> FullDeckOfCards
        {
            get { return this.fullDeckOfCards; }
        }

        public IList<WonderCard> WonderCards
        {
            get { return this.wonderCards; }
        }

        public IList<IStructureCard> DiscardPile
        {
            get { return this.discardPile; }
        }

        public int CurrentTurn { get; private set; } = 1;

        public Age CurrentAge
        {
            get { return this.currentAge; }
        }

        protected IList<IGamePlayer> BasePlayers
        {
            get { return this.players.Select(p => (IGamePlayer)p).ToList(); }
        }

        public GameFlowManager(GameStyle style = GameStyle.Base)
        {
            this.style = style;
            currentAge = Age.I;
        }

        public void CreateNewPlayer(string name)
        {
            players.Add(new TurnPlayer(name));
        }

        public void SetupGame()
        {
            try
            {
                LoggerHelper.Info("Starting to setup the game.");
                IDeckOfCardsManager deckManager = new DeckOfCardsManager();
                ITradeManager tradeManager = new TradeManager();
                wonderCards = deckManager.GetShuffledWonderCards();
                fullDeckOfCards = deckManager.GetShuffledDeck(players.Count);
                tradeManager.SetupCoinsFromBank(this.BasePlayers);
                DrawWonderCards();
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Could not setup game properly.", e);
            }
        }

        public void StartAge()
        {
            try
            {
                LoggerHelper.InfoFormat("Starting age {0}.", currentAge.ToString());
                DealCards(currentAge);
                ITurnManager turnManager = new TurnManager();
                turnManager.InitializeTurnPlayers(this.players);
            }
            catch (Exception e)
            {
                LoggerHelper.Error("Could not start age " + currentAge.ToString(), e);
            }
        }

        void DrawWonderCards()
        {
            LoggerHelper.Info("Randomly drawing wonder cards to players.");
            foreach (var p in this.Players)
            {
                var wonderCard = wonderCards.First();
                wonderCards.Remove(wonderCard);
                var wonder = WonderFactory.CreateWonder(wonderCard.Name, wonderCard.Side);
                LoggerHelper.DebugFormat("Defining side {0} of wonder {1} to player: {2}", wonderCard.Side, wonderCard.Name, p.Name);
                p.SetWonder(wonder);
            }
        }

        void DealCards(Age age)
        {
            var ageCards = fullDeckOfCards.Where(c => c.Age == age).ToList();
            foreach (var p in players)
            {
                var initialCards = ageCards.Take(ConstantValues.PLAYER_CARDS_BY_AGE).ToList();
                ageCards.RemoveRange(0, ConstantValues.PLAYER_CARDS_BY_AGE);
                p.SetSelectableCards(initialCards);
            }
        }

        public void EndAge()
        {
            ITradeManager manager = new TradeManager();
            manager.ResolveMilitaryConflicts(players.Select(p => (IGamePlayer)p).ToList(), currentAge);
            if(this.currentAge != Age.III)
                MoveToNextAge();
        }

        public void MoveToNextAge()
        {
            ++currentAge;
            CurrentTurn = 1;
            StartAge();
        }

        private void DiscardLeftCards()
        {
            foreach (var p in players)
                foreach (var c in p.SelectableCards)
                {
                    discardPile.Add(c);
                    LoggerHelper.DebugFormat("Discarded {0}", c.Name);
                }
        }

        public void PlayTurn()
        {
            LoggerHelper.InfoFormat("Play turn {0}.", CurrentTurn);
            var uow = new UnitOfWork();
            ITurnManager turnManager = new TurnManager(uow);
            turnManager.SetCurrentInfo(currentAge, CurrentTurn);
            turnManager.Play(players, discardPile);
            uow.Commit();
            LoggerHelper.InfoFormat("All plays commited.", CurrentTurn);
        }

        public void CollectTurnRewards()
        {
            var uow = new UnitOfWork();
            ITurnManager turnManager = new TurnManager(uow);
            turnManager.SetCurrentInfo(currentAge, CurrentTurn);
            //Right now play seventh card is the only available reward that happens multiple times,
            //it needs to be played and commited before the rest of the plays as a player could discard the card and another player use it to buy from discard pile.
            turnManager.GetMultipleTimesRewards(players, discardPile);            
            uow.Commit();
            //If last turn of an age, needs to discard all left cards right away
            if (CurrentTurn == ConstantValues.TURNS_BY_AGE)
            {
                LoggerHelper.Debug("Moving all selectable cards left to discard pile");
                DiscardLeftCards();
            }
            turnManager.GetRewards(players, discardPile);
            uow.Commit();
        }

        public void EndTurn()
        {
            ITurnManager turnManager = new TurnManager();
            turnManager.SetCurrentInfo(currentAge, CurrentTurn);
            turnManager.MoveSelectableCards(this.players);
            turnManager.InitializeTurnPlayers(this.players);
            if (++CurrentTurn > ConstantValues.TURNS_BY_AGE)
                EndAge();
        }

        public void CollectPostGameRewards()
        {
            var uow = new UnitOfWork();
            ITurnManager turnManager = new TurnManager(uow);
            turnManager.GetPostGameRewards(players.Select(p => (IGamePlayer)p).ToList());
            uow.Commit();
        }

        public void ComputePoints()
        {
            LoggerHelper.Info("Computing points to end the game and declare the winner.");
            IGamePointsManager manager = new GamePointsManager();
            manager.ComputeVictoryPoints(this.players.Select(p => (IPlayer)p).ToList());
        }
    }
}
