using SevenWonder.BaseEntities;
using System.Collections.Generic;

namespace SevenWonder.Contracts
{
    public interface IWonder
    {
        WonderName Name { get; }
        ResourceType OriginalResource { get; }
        WonderBoardSide SelectedSide { get; }
        int NumberOfStages { get; }
        int StagesBuilt { get; }
        IWonderStage NextStage { get; }
        IWonderStage CurrentStage { get; }
        void BuildStage();
        IList<IEffect> EffectsAvailable { get; }
        IList<IList<IEffect>> ChoosableEffectsAvailable { get; }
    }
}
