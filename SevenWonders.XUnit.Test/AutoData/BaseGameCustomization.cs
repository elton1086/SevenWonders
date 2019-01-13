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
                .CreateMany<GamePlayer>(totalPlayers)
                .ToList();

            fixture.Register(() => players);
        }
    }
}
