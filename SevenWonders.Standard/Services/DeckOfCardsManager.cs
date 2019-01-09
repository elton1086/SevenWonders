using SevenWonders.BaseEntities;
using SevenWonders.CardGenerator;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using SevenWonders.Factories;
using SevenWonders.Helper;
using SevenWonders.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.Services
{
    public class DeckOfCardsManager : IDeckOfCardsManager
    {
        private readonly ICoreLogger logger;

        public DeckOfCardsManager(ICoreLogger logger)
        {
            this.logger = logger;
        }

        public IList<StructureCard> GetShuffledDeck(int numberOfPlayers)
        {
            var cardMapping = CardMappingGenerator.GenerateBaseGameCards();
            logger.Debug("The deck has {0} cards", cardMapping.CardMapping.Count);
            var deck = FilterAndMapDeck(cardMapping, numberOfPlayers);            
            Randomizer.Shuffle(deck, deck.Count * 4);
            logger.Debug("Shuffled {0} cards for {1} players", deck.Count, numberOfPlayers);
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

        private IList<StructureCard> FilterAndMapDeck(CardCollection collection, int numberOfPlayers)
        {
            var finalDeck = collection.CardMapping
                .Where(c => c.MinimumPlayers <= numberOfPlayers || c.Type == StructureType.Guilds)
                .Select(c => StructureCardFactory.CreateStructureCard(c))
                .ToList();

            IList<StructureCard> tempGuilds;
            //Should always have total guild cards = number of players + 2
            while ((tempGuilds = finalDeck.Where(c => c.Type == StructureType.Guilds).ToList()).Count > numberOfPlayers + 2)
            {
                finalDeck.Remove(Randomizer.SelectOne(tempGuilds));
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
