#define FULL
using SevenWonders.BaseEntities;
using System;

namespace SevenWonders.Helper
{
    public static class Enumerator
    {
        public static bool ContainsEnumeratorValue<T>(int value)
        {
            Type enumType = typeof(T);
#if FULL
            if (!enumType.IsEnum)
                return false;
#endif
            //Convert int value to enum of type T
            var foundValue = (T)Enum.ToObject(enumType, value);
            // Check if it is a valid enum value
            return Enum.GetName(enumType, foundValue) != null;
        }

        public static TradeDiscountType GetTradeDiscountType(ResourceType resourceType)
        {
            var discountType = TradeDiscountType.None;
            if (Enumerator.ContainsEnumeratorValue<RawMaterialType>((int)resourceType))
                discountType = TradeDiscountType.RawMaterial;
            else if (Enumerator.ContainsEnumeratorValue<ManufacturedGoodType>((int)resourceType))
                discountType = TradeDiscountType.ManufacturedGood;
            return discountType;
        }
    }
}
