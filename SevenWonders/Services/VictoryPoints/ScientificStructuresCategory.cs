using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Exceptions;
using SevenWonder.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonder.Services.VictoryPoints
{
    public class ScientificStructuresCategory : PointsCategory
    {
        //need to check wonder to see if can choose a scientific card

        private class NewSymbols
        {
            public IList<ScientificSymbol> Symbols { get; set; }
            public int Total { get; set; }
        }

        protected override void Compute(IList<IPlayer> players)
        {
            LoggerHelper.DebugFormat("Starting to compute scientific symbol victory points");
            foreach (var player in players)
            {
                var symbols = new List<ScientificSymbol>();
                var cards = player.Cards.Where(c => c.Type == StructureType.Scientific || c.Type == StructureType.Guilds).ToList();
                var choosables = 0;

                foreach (var c in cards)
                {
                    symbols.AddRange(c.StandaloneEffect
                        .Where(e => Enumerator.ContainsEnumeratorValue<ScientificSymbol>((int)e.Type))
                        .Select(e => (ScientificSymbol)((int)e.Type)));

                    choosables += c.ChoosableEffect
                        .Any(e => Enumerator.ContainsEnumeratorValue<ScientificSymbol>((int)e.Type)) ?
                        1 : 0;
                }

                foreach (var p in player.Wonder.ChoosableEffectsAvailable)
                {
                    choosables += p.Any(e => Enumerator.ContainsEnumeratorValue<ScientificSymbol>((int)e.Type)) ?
                    1 : 0;
                }

                var symbolsToCompute = GetBestCombination(symbols, choosables);
                player.VictoryPoints += GetVictoryPoints(symbolsToCompute);

                LoggerHelper.DebugFormat("{0} has now {1} victory points.", player.Name, player.VictoryPoints);
            }
        }

        public static IList<ScientificSymbol> GetBestCombination(IList<ScientificSymbol> currentSymbols, int numberOfChoosables)
        {
            if (numberOfChoosables == 0)
                return currentSymbols;
            if (numberOfChoosables > 3)
                throw new TooManyException(string.Format("Number of choosable scientific symbols ({0}) cannot be handled.", numberOfChoosables));
            var newSymbols = new NewSymbols();
            if (numberOfChoosables == 1)
            {
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Compass }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Gear }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Tablet }, newSymbols);
            }

            if (numberOfChoosables == 2)
            {
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Compass, ScientificSymbol.Compass }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Gear }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Tablet, ScientificSymbol.Tablet }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Compass, ScientificSymbol.Gear }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Compass, ScientificSymbol.Tablet }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Tablet }, newSymbols);
            }

            if (numberOfChoosables == 3)
            {
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Compass, ScientificSymbol.Compass, ScientificSymbol.Compass }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Gear, ScientificSymbol.Gear }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Tablet, ScientificSymbol.Tablet, ScientificSymbol.Tablet }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Compass, ScientificSymbol.Gear, ScientificSymbol.Tablet }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Compass, ScientificSymbol.Compass, ScientificSymbol.Gear }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Compass, ScientificSymbol.Compass, ScientificSymbol.Tablet }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Gear, ScientificSymbol.Compass }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Gear, ScientificSymbol.Gear, ScientificSymbol.Tablet }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Tablet, ScientificSymbol.Tablet, ScientificSymbol.Compass }, newSymbols);
                TestVictoryPoints(currentSymbols, new List<ScientificSymbol> { ScientificSymbol.Tablet, ScientificSymbol.Tablet, ScientificSymbol.Gear }, newSymbols);
            }

            var result = currentSymbols.ToList();
            result.AddRange(newSymbols.Symbols);
            return result;
        }

        private static void TestVictoryPoints(IList<ScientificSymbol> currentSymbols, IList<ScientificSymbol> symbolsToAdd, NewSymbols bestSoFar)
        {
            var symbols = currentSymbols.ToList();
            symbols.AddRange(symbolsToAdd);
            var total = GetVictoryPoints(symbols);
            if (total > bestSoFar.Total)
            {
                bestSoFar.Total = total;
                bestSoFar.Symbols = symbolsToAdd;
            }
        }

        public static int GetVictoryPoints(IList<ScientificSymbol> symbols)
        {
            var compasses = symbols.Count(s => s == ScientificSymbol.Compass);
            var gears = symbols.Count(s => s == ScientificSymbol.Gear);
            var tablets = symbols.Count(s => s == ScientificSymbol.Tablet);

            var verticalSum = Math.Pow(gears, ConstantValues.SCIENTIFIC_SYMBOL_POWER) + Math.Pow(compasses, ConstantValues.SCIENTIFIC_SYMBOL_POWER) + Math.Pow(tablets, ConstantValues.SCIENTIFIC_SYMBOL_POWER);
            var horizontalSum = Math.Min(Math.Min(gears, compasses), tablets) * ConstantValues.SCIENTIFIC_SYMBOL_SET_POINTS;
            return (int)verticalSum + horizontalSum;
        }
    }
}
