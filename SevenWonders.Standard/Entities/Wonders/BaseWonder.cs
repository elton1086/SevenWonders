using SevenWonders.BaseEntities;
using SevenWonders.Contracts;
using System.Collections.Generic;

namespace SevenWonders.Entities
{
    public abstract class BaseWonder
    {
        protected int stagesBuilt;
        protected List<Effect> effectsInUse = new List<Effect>();
        protected WonderBoardSide selectedSide;
        private IList<IWonderStage> stages;

        protected BaseWonder(WonderBoardSide boardSide)
        {
            this.selectedSide = boardSide;
            switch (selectedSide)
            {
                case WonderBoardSide.A:
                    CreateASideStages();
                    break;
                case WonderBoardSide.B:
                    CreateBSideStages();
                    break;
                default:
                    break;
            }
        }

        public abstract WonderName Name { get; }
        public abstract ResourceType OriginalResource { get; }

        public int NumberOfStages
        {
            get { return Stages.Count; }
        }

        public int StagesBuilt
        {
            get { return stagesBuilt; }
        }

        public IWonderStage NextStage
        {
            get
            {
                if (stagesBuilt == Stages.Count)
                    return null;
                return Stages[stagesBuilt];
            }
        }

        public IWonderStage CurrentStage
        {
            get
            {
                if (stagesBuilt == 0)
                    return null;
                return Stages[stagesBuilt - 1];
            }
        }

        public void BuildStage()
        {
            effectsInUse.AddRange(Stages[stagesBuilt].Effects);
            if(stagesBuilt < Stages.Count)
                stagesBuilt++;
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

        public WonderBoardSide SelectedSide
        {
            get { return selectedSide; }
        }

        protected IList<IWonderStage> Stages
        {
            get
            {
                if (stages == null)
                    stages = new List<IWonderStage>();
                return stages;
            }
        }

        protected void InitializeStages(int numberOfStages)
        {
            for (int i = 0; i < numberOfStages; i++)
                Stages.Add(new WonderStage());
        }

        protected abstract void CreateASideStages();
        protected abstract void CreateBSideStages();
    }
}
