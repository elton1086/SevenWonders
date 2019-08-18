using SevenWonders.BaseEntities;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public abstract class BaseWonder
    {
        protected IList<WonderStage> stages = new List<WonderStage>();

        protected List<Effect> effectsInUse = new List<Effect>();

        protected BaseWonder(WonderBoardSide boardSide)
        {
            SelectedSide = boardSide;
            switch (SelectedSide)
            {
                case WonderBoardSide.A:
                    stages = CreateASideStages() ?? new List<WonderStage>();
                    break;
                case WonderBoardSide.B:
                    stages = CreateBSideStages() ?? new List<WonderStage>();
                    break;
                default:
                    break;
            }
        }

        public abstract WonderName Name { get; }
        public abstract ResourceType OriginalResource { get; }

        public int NumberOfStages
        {
            get { return stages.Count; }
        }

        public int StagesBuilt { get; private set; }

        public WonderStage NextStage
        {
            get
            {
                if (StagesBuilt == stages.Count)
                    return null;
                return stages[StagesBuilt];
            }
        }

        public WonderStage CurrentStage
        {
            get
            {
                if (StagesBuilt == 0)
                    return null;
                return stages[StagesBuilt - 1];
            }
        }

        public void BuildStage()
        {
            effectsInUse.AddRange(stages[StagesBuilt].Effects);
            if(StagesBuilt < stages.Count)
                StagesBuilt++;
        }

        public virtual IList<Effect> EffectsAvailable
        {
            get { return effectsInUse; }
        }

        /// <summary>
        /// Effects available to choose from. This works as long as only one of the stages allows to choose effect.
        /// </summary>
        public virtual IList<IList<Effect>> ChoosableEffectsAvailable
        {
            get { return new List<IList<Effect>>(); }
        }

        public WonderBoardSide SelectedSide { get; }

        protected abstract IList<WonderStage> CreateASideStages();
        protected abstract IList<WonderStage> CreateBSideStages();
    }
}
