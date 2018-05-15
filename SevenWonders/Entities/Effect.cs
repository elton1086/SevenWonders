using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenWonder.Entities
{
    public class Effect : IEffect
    {
        private EffectType type;
        private int quantity;
        private PlayerDirection direction;
        private object info;

        public EffectType Type
        {
            get { return type; }
            set { type = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        [XmlIgnore]
        public PlayerDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public int AllDirections
        {
            get { return (int)direction; }
            set { direction = (PlayerDirection)value; }
        }

        public Effect()
        {
        }

        public Effect(EffectType type, int quantity = 1, PlayerDirection direction = PlayerDirection.None)
        {
            this.type = type;
            this.quantity = quantity;
            this.direction = direction;
        }

        [XmlIgnore]
        public object Info
        {
            get { return info; }
            set { info = value; }
        }
    }
}
