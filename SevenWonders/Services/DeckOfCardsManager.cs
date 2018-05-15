using SevenWonder.BaseEntities;
using SevenWonder.CardGenerator;
using SevenWonder.Contracts;
using SevenWonder.Factories;
using SevenWonder.Helper;
using SevenWonder.Services.Contracts;
using SevenWonder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonder.Services
{
    public class DeckOfCardsManager : IDeckOfCardsManager
    {
        private IUnitOfWork unitOfWork;

        public DeckOfCardsManager()
        {
            this.unitOfWork = new NullUnitOfWork();
        }

        public DeckOfCardsManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IList<IStructureCard> GetShuffledDeck(int numberOfPlayers)
        {
            var cardMapping = CardMappingHelper.ReadMainXmlFile();
            LoggerHelper.DebugFormat("The deck has {0} cards", cardMapping.CardMapping.Count);
            var deck = FilterAndMapDeck(cardMapping, numberOfPlayers);            
            Randomizer.Shuffle<IStructureCard>(deck, deck.Count * 4);
            LoggerHelper.DebugFormat("Shuffled {0} cards for {1} players", deck.Count, numberOfPlayers);
            return deck;
        }

        public IList<WonderCard> GetShuffledWonderCards()
        {
            var wonderCards = new List<WonderCard>();
            foreach (var name in Enum.GetNames(typeof(WonderName)).ToList())
            {
                foreach (var side in Enum.GetNames(typeof(WonderBoardSide)).ToList())
                {
                    wonderCards.Add(new WonderCard((WonderName)Enum.Parse(typeof(WonderName), name), (WonderBoardSide)Enum.Parse(typeof(WonderBoardSide), side)));
                }
            }
            Randomizer.Shuffle<WonderCard>(wonderCards, 40);
            return wonderCards.Distinct(new WonderCardEqualityComparer()).ToList();
        }

        private IList<IStructureCard> FilterAndMapDeck(CardCollection collection, int numberOfPlayers)
        {
            var finalDeck = new List<IStructureCard>();
            IList<IStructureCard> tempGuilds;

            foreach (var c in collection.CardMapping)
            {
                if (c.MinimumPlayers > numberOfPlayers && c.Type != StructureType.Guilds)
                    continue;
                finalDeck.Add(StructureCardFactory.CreateStructureCard(c));
            }
            //Should always have total guild cards = number of players + 2
            while ((tempGuilds = finalDeck.Where(c => c.Type == StructureType.Guilds).ToList()).Count > numberOfPlayers + 2)
            {
                finalDeck.Remove(Randomizer.SelectOne<IStructureCard>(tempGuilds));
            }
            return finalDeck;
        }
    }

    public class WonderCardEqualityComparer : IEqualityComparer<WonderCard>
    {
        public bool Equals(WonderCard x, WonderCard y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(WonderCard obj)
        {
            return base.GetHashCode();
        }
    }
}
