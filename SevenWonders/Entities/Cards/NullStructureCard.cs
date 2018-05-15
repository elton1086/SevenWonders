using SevenWonder.BaseEntities;
using SevenWonder.Contracts;
using System;
using System.Collections.Generic;

namespace SevenWonder.Entities
{
    public class NullStructureCard : IStructureCard
    {
        public CardName Name
        {
            get { throw new NotImplementedException(); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int PlayersCount
        {
            get { throw new NotImplementedException(); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CoinCost
        {
            get { throw new NotImplementedException(); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Age Age
        {
            get { throw new NotImplementedException(); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<ResourceType> ResourceCosts
        {
            get { throw new NotImplementedException(); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<CardName> CardCosts
        {
            get { throw new NotImplementedException(); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<IEffect> Production
        {
            get { throw new NotImplementedException(); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public StructureType Type
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public IList<IEffect> StandaloneEffect
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<IEffect> ChoosableEffect
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
