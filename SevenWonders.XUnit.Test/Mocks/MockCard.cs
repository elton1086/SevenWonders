using System.Collections.Generic;
using SevenWonders.BaseEntities;
using SevenWonders.Entities;

namespace SevenWonders.XUnit.Test.Mocks
{
    public class MockCard : StructureCard
    {
        public IList<Effect> SetEffects { get { return Production; } set { Production = value; } }

        public MockCard() : base(StructureType.Civilian, CardName.Academy, 0, Age.I, null, null, null)
        {
        }
    }
}
