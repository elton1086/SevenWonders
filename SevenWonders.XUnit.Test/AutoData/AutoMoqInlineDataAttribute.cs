using AutoFixture.Xunit2;

namespace SevenWonders.XUnit.Test.AutoData
{
    public class AutoMoqInlineDataAttribute : InlineAutoDataAttribute
    {
        public AutoMoqInlineDataAttribute(params object[] values)
           : base(new AutoMoqDataAttribute(), values)
        {

        }
    }
}
