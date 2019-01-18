using AutoFixture;
using SevenWonders.BaseEntities;
using SevenWonders.Entities;
using SevenWonders.Factories;
using System.Linq;

namespace SevenWonders.XUnit.Test.AutoData
{
    public class GamePlayerCustomization : ICustomization
    {
        protected readonly int totalPlayers;

        public GamePlayerCustomization(int totalPlayers)
        {
            this.totalPlayers = totalPlayers;
        }

        public virtual void Customize(IFixture fixture)
        {
            var players = fixture
                .Build<GamePlayer>()
                .Without(p => p.VictoryPoints)
                .CreateMany(totalPlayers)
                .ToList();

            players.ForEach(p =>
            {
                p.SetWonder(WonderFactory.CreateWonder(fixture.Create<WonderName>(), fixture.Create<WonderBoardSide>()));
            });

            fixture.Register(() => players);

            fixture.Register(() => players.Select(p => new TurnPlayer(p)).ToList());
        }
    }
}
