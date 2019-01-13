using AutoFixture.Xunit2;

namespace SevenWonders.XUnit.Test.AutoData
{
    public class AutoBaseGameSetupDataAttribute : InlineAutoDataAttribute
    {
        public AutoBaseGameSetupDataAttribute(int totalPlayers = 3, params object[] values) : 
            base(new AutoMoqDataAttribute(new BaseGameCustomization(totalPlayers)), values)
        {
        }
    }
}
