using SevenWonder.BaseEntities;
using SevenWonder.Entities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenWonder.CardGenerator
{
    [XmlType(AnonymousType = true, Namespace = "http://tempuri.org/CardCollection.xsd")]
    [XmlRoot(Namespace = "http://tempuri.org/CardCollection.xsd", IsNullable = false)]
    public class CardCollection
    {
        private List<CardMapping> cardMappingField;

        public CardCollection()
        {
            this.cardMappingField = new List<CardMapping>();
        }

        public List<CardMapping> CardMapping
        {
            get { return cardMappingField; }
            set { cardMappingField = value; }
        }
    }

    public class CardMapping
    {
        private List<ResourceType> resourceCostsField;        
        private List<CardName> cardCostsField;
        private List<Effect> effectsField;
        private StructureType typeField;
        private CardName nameField;
        private int minimumPlayersField;
        private bool minimumPlayersFieldSpecified;
        private Age ageField;

        public CardMapping()
        {
            resourceCostsField = new List<ResourceType>();
            cardCostsField = new List<CardName>();
            effectsField = new List<Effect>();
        }

        public List<ResourceType> ResourceCosts
        {
            get { return resourceCostsField; }
            set { resourceCostsField = value; }
        }

        public List<CardName> CardCosts
        {
            get { return cardCostsField; }
            set { cardCostsField = value; }
        }

        public List<Effect> Effects
        {
            get { return effectsField; }
            set { effectsField = value; }
        }

        [XmlAttribute]
        public StructureType Type
        {
            get { return typeField; }
            set { typeField = value; }
        }

        [XmlAttribute]
        public CardName Name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        [XmlAttribute]
        public int MinimumPlayers
        {
            get { return minimumPlayersField; }
            set { minimumPlayersField = value; }
        }

        [XmlIgnore]
        public bool MinimumPlayersFieldSpecified
        {
            get { return minimumPlayersFieldSpecified; }
            set { minimumPlayersFieldSpecified = value; }
        }

        [XmlAttribute]
        public Age Age
        {
            get { return ageField; }
            set { ageField = value; }
        }
    }
}
