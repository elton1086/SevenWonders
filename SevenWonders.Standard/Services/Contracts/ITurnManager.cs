using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using SevenWonders.Entities;
using System.Collections.Generic;

namespace SevenWonders.Services.Contracts
{
    public interface ITurnManager
    {
        void SetScope(IUnitOfWork unitOfWork);
        void Play(IList<TurnPlayer> players, IList<StructureCard> discardedCards, Age age);
        void GetMultipleTimesRewards(IList<TurnPlayer> players, IList<StructureCard> discardedCards, int turn, Age age);
        void GetRewards(IList<TurnPlayer> players, IList<StructureCard> discardedCards, Age age);
        void GetPostGameRewards(IList<GamePlayer> players);
    }
}