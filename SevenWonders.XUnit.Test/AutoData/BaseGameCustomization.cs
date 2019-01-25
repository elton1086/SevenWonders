using AutoFixture;
using SevenWonders.CardGenerator;
using SevenWonders.Entities;
using SevenWonders.Factories;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.XUnit.Test.AutoData
{
    public class BaseGameCustomization : GamePlayerCustomization
    {
        public BaseGameCustomization(int totalPlayers) : base(totalPlayers)
        { }

        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);
            var players = new List<TurnPlayer>();
            fixture.Create<List<GamePlayer>>()
                .ForEach(p => {
                    var player = new TurnPlayer(p);
                    player.SetSelectableCards(fixture.CreateMany<CardMapping>(7)
                        .Select(c => StructureCardFactory.CreateStructureCard(c))
                        .ToList());
                    players.Add(player);
                });

            fixture.Register(() => players);

            fixture.Register(() => CardMappingGenerator.GenerateBaseGameCards().CardMapping
                .Select(m => StructureCardFactory.CreateStructureCard(m))
                .ToList());
        }
    }
}
