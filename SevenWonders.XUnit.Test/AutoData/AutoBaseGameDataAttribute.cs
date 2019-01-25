using AutoFixture.Xunit2;

namespace SevenWonders.XUnit.Test.AutoData
{
    public class AutoBaseGameDataAttribute : InlineAutoDataAttribute
    {
        public AutoBaseGameDataAttribute(int totalPlayers = 3, params object[] values) :
            base(new AutoMoqDataAttribute(new BaseGameCustomization(totalPlayers)), values)
        {
        }
    }
}
