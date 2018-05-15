using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using SevenWonder.Entities;
using SevenWonder.Helper;
using SevenWonder.Services;
using SevenWonder.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlay
{
    class Program
    {
        static GameFlowManager manager = new GameFlowManager(GameStyle.Base);

        static void Main(string[] args)
        {
            DefinePlayers();
            StartGame();
            while (true)
            {
                PlayTurn();
                if (manager.CurrentAge == Age.III && manager.CurrentTurn == 7)
                {
                    EndGame();
                    Console.WriteLine("Press any key to quit.");
                    Console.Read();
                    return;
                }
                Console.WriteLine("Press ESC to quit. Any other key to play next turn.");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    return;
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static void DefinePlayers()
        {
            while (true)
            {
                Console.WriteLine("Please type the name of the players separated by comma (,) or semi-colon (;)");
                var names = Console.ReadLine().Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                if (names.Length < 3)
                {
                    Console.WriteLine("At least three players to play the game");
                    continue;
                }
                if (names.Length > 7)
                {
                    Console.WriteLine("No more than seven players to play the game");
                    continue;
                }
                foreach (var s in names)
                    manager.CreateNewPlayer(s);
                break;
            }
            Console.WriteLine();
        }

        static void StartGame()
        {
            manager.SetupGame();
            manager.StartAge();
        }

        static void PlayTurn()
        {
            Console.WriteLine(string.Format("Play turn {0}", manager.CurrentTurn));
            Console.WriteLine();
            foreach (var p in manager.Players)
                ChoosePlay(p);
            manager.PlayTurn();
            Console.WriteLine("Played cards");
            foreach (var p in manager.Players)
                Console.WriteLine(string.Format("{0} => {1} => {2}", p.Name.ToUpper(), p.ExecutedAction, p.SelectedCard.Name));
            Console.WriteLine();
            //Seventh card should be played before the rest of extra choice play
            SeventhCardPlay();
            ExtraChoicePlay();
            manager.CollectTurnRewards();
            manager.EndTurn();
            if (manager.CurrentTurn == 1)
            {
                ConflictResults();
                NextAge();
            }
            else if (manager.CurrentTurn > 6 && manager.CurrentAge == Age.III)
                ConflictResults();
            Console.WriteLine();
        }

        static void ChoosePlay(ITurnPlayer player)
        {
            Console.WriteLine(string.Format("Player {0} ", player.Name.ToUpper()));
            //Console.WriteLine(string.Format("Player {0} ({1} {2} {3} stage(s))", player.Name.ToUpper(), player.Wonder.Name, player.Wonder.SelectedSide, player.Wonder.StagesBuilt));
            CurrentData((IGamePlayer)player);
            CardOptions(player);
            SelectCard(player);
            SelectAction(player);
            BorrowResources(player);            
        }

        static void CardOptions(ITurnPlayer player)
        {
            Console.WriteLine("These are your card options:");
            for (int i = 0; i < player.SelectableCards.Count; i++)
            {
                var card = player.SelectableCards[i];
                ShowCardOption(card, i + 1);
            }
            Console.WriteLine();
        }

        static void ShowCardOption(IStructureCard card, int choice)
        {
            var sb = string.Format("{0} - {1} ({2})", choice, card.Name.ToString().ToUpper(), card.Type);
            var resSb = new StringBuilder();
            var separator = "";
            if (!card.CardCosts.Any() && !card.ResourceCosts.Any())
                resSb.Append(" -> Free");
            else
            {
                resSb.Append(" -> ");                
                if (card.CardCosts.Any())
                {
                    var cc = string.Empty;
                    foreach (var c in card.CardCosts)
                    {
                        cc += separator + c.ToString();
                        separator = ";";
                    }
                    resSb.AppendFormat("Card costs = {0}", cc);
                }

                if (card.CardCosts.Any() && card.ResourceCosts.Any())
                    resSb.Append(" | ");

                if (card.ResourceCosts.Any())
                {
                    var rc = string.Empty;
                    separator = "";
                    foreach (var r in card.ResourceCosts)
                    {
                        rc += separator + r.ToString();
                        separator = ";";
                    }
                    resSb.AppendFormat("Resource costs = {0}", rc);
                }               
            }
            var efSb = new StringBuilder(" => ");
            separator = "";
            foreach (var e in card.StandaloneEffect)
            {
                var addInfo = "";
                var dir = GetPlayerDirections(e.Direction);
                if (!string.IsNullOrWhiteSpace(dir))
                    addInfo = "(" + dir + ")";
                efSb.Append(separator + e.Type.ToString() + dir);
                separator = ";";
            }
            separator = "";
            foreach (var e in card.ChoosableEffect)
            {
                var addInfo = "";
                var dir = GetPlayerDirections(e.Direction);
                if (!string.IsNullOrWhiteSpace(dir))
                    addInfo = "(" + dir + ")";
                efSb.Append(separator + e.Type.ToString() + dir);
                separator = "|";
            }
            Console.WriteLine(sb.ToString().PadRight(35) + resSb.ToString().PadRight(65) + efSb.ToString());
        }

        static string GetPlayerDirections(PlayerDirection direction)
        {
            var result = "";
            if (direction.HasFlag(PlayerDirection.ToTheLeft))
                result += "<-";
            if (direction.HasFlag(PlayerDirection.Myself))
                result += "^";
            if (direction.HasFlag(PlayerDirection.ToTheRight))
                result += "->";
            return result;
        }

        static void SelectCard(ITurnPlayer player)
        {
            Console.WriteLine("Please select your card number");
            int index;
            while (true)
            {
                var selection = Console.ReadLine();
                if (!int.TryParse(selection, out index) || index > player.SelectableCards.Count || index < 1)
                    continue;
                player.SelectedCard = player.SelectableCards[index - 1];
                Console.WriteLine(player.SelectedCard.Name + " chosen.");
                Console.WriteLine();
                break;
            }
        }

        static void SelectAction(ITurnPlayer player)
        {
            Console.WriteLine("Please select option 1 (BUY card), 2 (SELL card), 3 (BUILD wonder stage)");
            int index;
            while (true)
            {
                var action = Console.ReadLine();
                if (!int.TryParse(action, out index) || index > 3 || index < 1)
                    continue;
                player.ChosenAction = (TurnAction)index - 1;
                Console.WriteLine(player.ChosenAction + " chosen.");
                Console.WriteLine();
                break;
            }
        }

        static void BorrowResources(ITurnPlayer player)
        {
            var resources = new List<ResourceType>();
            if (player.ChosenAction == TurnAction.BuyCard)
                resources.AddRange(player.SelectedCard.ResourceCosts.Where(r => r != ResourceType.Coin));
            if(player.ChosenAction == TurnAction.BuildWonderStage)
                resources.AddRange(player.Wonder.NextStage.Costs.Where(r => r != ResourceType.Coin));

            if (resources.Any() && player.CheckResourceAvailability(resources, false).Any())
            {
                int index;
                while (true)
                {
                    var oneForFreeCard = player.GetNonResourceEffects().FirstOrDefault(e => e.Type == EffectType.PlayCardForFreeOncePerAge);
                    Console.WriteLine("You need to borrow resources, select which neighbor: 1 (from LEFT), 2 (from RIGHT), 3 (SKIP)" 
                        + (oneForFreeCard != null && player.ChosenAction == TurnAction.BuyCard && (oneForFreeCard.Info == null || (Age)oneForFreeCard.Info != manager.CurrentAge) ? ", 4 (PLAY FOR FREE)" : ""));
                    var neighborSide = Console.ReadLine();
                    if (!int.TryParse(neighborSide, out index) || index == 3 || index > 4 || index < 1)
                        break;

                    if (index == 4)
                    {
                        player.SpecialCaseToUse = SpecialCaseType.PlayCardForFreeOncePerAge;
                        break;
                    }

                    var neighbor = index == 1 ? PlayerDirection.ToTheLeft : PlayerDirection.ToTheRight;
                    Console.WriteLine("Type the resource numbers you want to borrow from " + (index == 1 ? "LEFT" : "RIGHT") + " separated by comma (,)");
                    Console.WriteLine("1 (CLAY), 2 (ORE), 3 (STONE), 4 (WOOD), 5 (GLASS), 6 (LOOM), 7 (PAPYRUS)");
                    var borrow = Console.ReadLine();
                    foreach (var s in borrow.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!int.TryParse(s, out index) || index > 7 || index < 1)
                            continue;
                        #region Translate Resource
                        var res = ResourceType.Coin;
                        switch (index)
                        {
                            case 1:
                                res = ResourceType.Clay;
                                break;
                            case 2:
                                res = ResourceType.Ore;
                                break;
                            case 3:
                                res = ResourceType.Stone;
                                break;
                            case 4:
                                res = ResourceType.Wood;
                                break;
                            case 5:
                                res = ResourceType.Glass;
                                break;
                            case 6:
                                res = ResourceType.Loom;
                                break;
                            case 7:
                                res = ResourceType.Papyrus;
                                break;
                            default:
                                break;
                        }
                        #endregion
                        player.ResourcesToBorrow.Add(new BorrowResourceData { ChosenNeighbor = neighbor, ResourceType = res });
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
        
        private static void SeventhCardPlay()
        {
            foreach (ITurnPlayer player in manager.Players)
            {
                if (player.Wonder.EffectsAvailable.Any(e => e.Type == EffectType.PlaySeventhCard) && manager.CurrentTurn == 6)
                {
                    Console.WriteLine("What do you want to do with the seventh card?");
                    var card = player.SelectableCards.First(c => c != player.SelectedCard);
                    ShowCardOption(card, 1);
                    SelectAction(player);
                    BorrowResources(player);
                }
            }
        }

        private static void ExtraChoicePlay()
        {
            foreach (ITurnPlayer player in manager.Players)
            {
                if (player.ExecutedAction == TurnAction.BuildWonderStage && player.Wonder.CurrentStage != null && player.Wonder.CurrentStage.Effects.Any(e => e.Type == EffectType.PlayOneDiscardedCard))
                {
                    Console.WriteLine("Please select one discarded card to play for free:");
                    for (int i = 0; i < manager.DiscardPile.Count; i++)
                    {
                        var card = manager.DiscardPile[i];
                        ShowCardOption(card, i + 1);
                    }
                    SelectAdditionalCard(player);
                }
                if (player.Wonder.EffectsAvailable.Any(e => e.Type == EffectType.CopyGuildFromNeighbor) && manager.CurrentTurn == 6 && manager.CurrentAge == Age.III)
                {
                    Console.WriteLine("Select guild to copy from your neighbor.");

                    var neighbors = NeighborsHelper.GetNeighbors(manager.Players.Select(p => (IPlayer)p).ToList(), player);
                    var options = neighbors[NeighborsHelper.LEFTDIRECTION].Cards
                        .Where(c => c.Type == StructureType.Guilds).ToList();
                    options.AddRange(neighbors[NeighborsHelper.RIGHTDIRECTION].Cards
                        .Where(c => c.Type == StructureType.Guilds));

                    if (options.Any())
                    {
                        int i = 0;
                        var cardNames = new List<CardName>();
                        foreach (var c in options)
                        {
                            ShowCardOption(c, ++i);
                            cardNames.Add(c.Name);
                        }
                        CopyGuildCard(player, cardNames);
                    }
                }
            }
        }

        static void SelectAdditionalCard(ITurnPlayer player)
        {
            Console.WriteLine("Please select your card number");
            int index;
            while (true)
            {
                var selection = Console.ReadLine();
                if (!int.TryParse(selection, out index) || index > manager.DiscardPile.Count || index < 1)
                    continue;
                player.AdditionalInfo = manager.DiscardPile[index - 1];
                Console.WriteLine();
                break;
            }
        }

        static void CopyGuildCard(ITurnPlayer player, IEnumerable<CardName> selectableCards)
        {
            Console.WriteLine("Please select your card number");
            int index;
            while (true)
            {
                var selection = Console.ReadLine();
                if (!int.TryParse(selection, out index) || index > selectableCards.Count() || index < 1)
                    continue;
                player.Wonder.EffectsAvailable
                    .First(e => e.Type == EffectType.CopyGuildFromNeighbor).Info = selectableCards.ElementAt(index - 1);                
                Console.WriteLine();
                break;
            }
        }

        static void CurrentData(IGamePlayer player)
        {
            WonderStage(player);
            Console.WriteLine("Coins: " + player.Coins);
            CurrentResources(player);
            CurrentCards(player);
            Console.WriteLine();
            var neighbors = NeighborsHelper.GetNeighbors(manager.Players.Select(p => (IPlayer)p).ToList(), manager.Players.First(p => p.Name == player.Name));
            Console.Write("Right ");
            CurrentResources((IGamePlayer)neighbors[NeighborsHelper.RIGHTDIRECTION], true);
            Console.Write("Left ");
            CurrentResources((IGamePlayer)neighbors[NeighborsHelper.LEFTDIRECTION], true);
            Console.WriteLine();
        }

        private static void WonderStage(IGamePlayer player)
        {
            Console.WriteLine(string.Format("({0} {1} {2} stage(s))", player.Wonder.Name, player.Wonder.SelectedSide, player.Wonder.StagesBuilt));            
            if (player.Wonder.NextStage == null)
                return;
            var sb = new StringBuilder();
            sb.Append(" -> ");
            var separator = "";
            if (player.Wonder.NextStage.Costs.Any())
            {
                var rc = string.Empty;
                foreach (var r in player.Wonder.NextStage.Costs)
                {
                    rc += separator + r.ToString();
                    separator = ";";
                }
                sb.AppendFormat("Costs = {0}", rc);
            }
            sb.Append(" => ");
            separator = "";
            foreach (var e in player.Wonder.NextStage.Effects)
            {
                sb.Append(separator + e.Type.ToString());
                separator = ";";
            }
            Console.WriteLine(sb.ToString());
        }

        static void CurrentResources(IGamePlayer player, bool shareable = false)
        {
            var sb = new StringBuilder();
            var separator = "";

            foreach (var r in player.GetResourcesAvailable(shareable))
            {
                sb.AppendFormat("{0}{1}", separator, r.ToString());
                separator = ";";
            }

            foreach (var c in player.GetChoosableResources(shareable))
            {
                if (!Enumerator.ContainsEnumeratorValue<ResourceType>((int)c.First()))
                    continue;

                sb.Append(separator);
                var chooseSeparator = "";
                foreach (var r in c)
                {
                    sb.AppendFormat("{0}{1}", chooseSeparator, r.ToString());
                    chooseSeparator = "/";
                }
                separator = ";";
            }
            Console.WriteLine(string.Format("Resources: {0}", sb.ToString()));
        }

        static void CurrentCards(IGamePlayer player)
        {
            var sb = new StringBuilder();
            var separator = "";

            foreach (var c in player.Cards)
            {
                var addInfo = "";
                if (c is ScientificCard)
                    addInfo = " " + c.StandaloneEffect.First().Type.ToString().Substring(0, 1).ToUpper();
                sb.AppendFormat("{0}{1}({2}{3})", separator, c.Name, c.Type.ToString().Substring(0,2).ToUpper(), addInfo);
                separator = ";";
            }
            Console.WriteLine(string.Format("Cards: {0}", sb.ToString()));            
        }

        static void ConflictResults()
        {
            Console.WriteLine("Military conflict results");
            foreach (var p in manager.Players)
            {
                var sb = new StringBuilder(p.Name.ToUpper() + " => ");
                var separator = "";
                foreach (var t in p.ConflictTokens)
                {
                    sb.AppendFormat("{0}{1}", separator, t);
                    separator = ";";
                }
                Console.WriteLine(sb.ToString());
            }
            Console.WriteLine();
        }

        static void NextAge()
        {            
            Console.WriteLine("Starting Age " + manager.CurrentAge.ToString());
            Console.WriteLine();
        }

        private static void EndGame()
        {
            Console.WriteLine("Computing post game rewards.");
            manager.CollectPostGameRewards();
            Console.WriteLine("Game is over, computing points");
            manager.ComputePoints();
            Console.WriteLine("These are the total victory points (VP) for each player:");
            foreach(var p in manager.Players)
                Console.WriteLine(string.Format("{0} => {1} VP", p.Name.ToUpper(), p.VictoryPoints));
            Console.WriteLine(string.Format("And the winner is... {0}. Congrats!", manager.Players.OrderByDescending(p => p.VictoryPoints).First().Name.ToUpper()));
        }
    }
}
