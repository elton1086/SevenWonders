using AutoFixture;
using SevenWonders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenWonders.XUnit.Test.AutoData
{
    public class BaseGameCustomization : ICustomization
    {
        private readonly int totalPlayers;

        public BaseGameCustomization(int totalPlayers)
        {
            this.totalPlayers = totalPlayers;
        }

        public void Customize(IFixture fixture)
        {
            var players = fixture
                .Build<GamePlayer>()
                .Without(p => p.VictoryPoints)
                .CreateMany(totalPlayers)
                .ToList();

            fixture.Register(() => players);

            fixture.Register(() => players.Select(p => new TurnPlayer(p)).ToList());
        }
    }
}
