using SevenWonders.BaseEntities;
using System.Xml.Serialization;

namespace SevenWonders.Entities
{
    public class Effect
    {
        public EffectType Type { get; set; }

        public int Quantity { get; set; }

        [XmlIgnore]
        public PlayerDirection Direction { get; set; }

        public int AllDirections
        {
            get { return (int)Direction; }
            set { Direction = (PlayerDirection)value; }
        }

        public Effect(EffectType type, int quantity = 1, PlayerDirection direction = PlayerDirection.None)
        {
            this.Type = type;
            this.Quantity = quantity;
            this.Direction = direction;
        }

        /// <summary>
        /// Additional information used by some of the effects.
        /// </summary>
        [XmlIgnore]
        public object Info { get; set; }
    }
}
