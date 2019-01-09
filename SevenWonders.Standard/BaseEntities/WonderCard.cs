using SevenWonders.BaseEntities;

namespace SevenWonders.Contracts
{
    public class WonderCard
    {
        private WonderName name;
        private WonderBoardSide side;

        public WonderCard(WonderName name, WonderBoardSide side)
        {
            this.name = name;
            this.side = side;
        }

        public WonderName Name { get { return name; } }
        public WonderBoardSide Side { get { return side; } }
    }
}
