using AutoFixture;
using SevenWonders.Entities;
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
            var players = fixture.Create<List<GamePlayer>>();
            fixture.Register(() => players.Select(p =>  new TurnPlayer(p)).ToList());
        }
    }
}
