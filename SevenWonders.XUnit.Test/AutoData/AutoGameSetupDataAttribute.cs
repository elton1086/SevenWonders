using AutoFixture.Xunit2;

namespace SevenWonders.XUnit.Test.AutoData
{
    public class AutoGameSetupDataAttribute : InlineAutoDataAttribute
    {
        public AutoGameSetupDataAttribute(int totalPlayers = 3, params object[] values) : 
            base(new AutoMoqDataAttribute(new GamePlayerCustomization(totalPlayers)), values)
        {
        }
    }
}
