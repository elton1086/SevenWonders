using SevenWonder.BaseEntities;

namespace SevenWonder.Contracts
{
    public interface IEffect
    {
        EffectType Type { get; set; }
        int Quantity { get; set; }
        PlayerDirection Direction { get; set; }
        /// <summary>
        /// Additional information used by some of the effects.
        /// </summary>
        object Info { get; set; }
    }
}
