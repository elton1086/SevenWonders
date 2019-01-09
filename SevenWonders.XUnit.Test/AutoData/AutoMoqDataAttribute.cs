using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace SevenWonders.XUnit.Test.AutoData
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
           : base(() =>
           {
               var fixture = new Fixture().Customize(new AutoMoqCustomization());
               fixture.Behaviors.Clear();
               fixture.Behaviors.Add(new OmitOnRecursionBehavior());
               return fixture;
           })
        {
        }

        public AutoMoqDataAttribute(ICustomization customization)
           : base(() =>
           {
               var fixture = new Fixture().Customize(new CompositeCustomization(customization, new AutoMoqCustomization()));
               fixture.Behaviors.Clear();
               fixture.Behaviors.Add(new OmitOnRecursionBehavior());
               return fixture;
           })
        {
        }
    }
}
