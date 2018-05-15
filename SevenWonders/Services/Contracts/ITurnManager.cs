using System.Collections.Generic;
using SevenWonder.BaseEntities;
using SevenWonder.Contracts;

namespace SevenWonder.Services.Contracts
{
    public interface ITurnManager
    {
        void SetCurrentInfo(Age age, int turn);
        void MoveSelectableCards(IList<ITurnPlayer> players);
        void Play(IList<ITurnPlayer> players, IList<IStructureCard> discardedCards);
        void GetMultipleTimesRewards(IList<ITurnPlayer> players, IList<IStructureCard> discardedCards);
        void GetRewards(IList<ITurnPlayer> players, IList<IStructureCard> discardedCards);
        void GetPostGameRewards(IList<IGamePlayer> players);
        void InitializeTurnPlayers(IList<ITurnPlayer> players);        
    }
}