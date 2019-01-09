using SevenWonders.BaseEntities;
using SevenWonders.Entities;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenWonders.CardGenerator
{
    [XmlType(AnonymousType = true, Namespace = "http://tempuri.org/CardCollection.xsd")]
    [XmlRoot(Namespace = "http://tempuri.org/CardCollection.xsd", IsNullable = false)]
    public class CardCollection
    {
        public List<CardMapping> CardMapping { get; set; } = new List<CardMapping>();
    }

    public class CardMapping
    {
        public List<ResourceType> ResourceCosts { get; set; } = new List<ResourceType>();

        public List<CardName> CardCosts { get; set; } = new List<CardName>();

        public List<Effect> Effects { get; set; } = new List<Effect>();

        [XmlAttribute]
        public StructureType Type { get; set; }

        [XmlAttribute]
        public CardName Name { get; set; }

        [XmlAttribute]
        public int MinimumPlayers { get; set; }

        [XmlIgnore]
        public bool MinimumPlayersFieldSpecified { get; set; }

        [XmlAttribute]
        public Age Age { get; set; }
    }
}
