using SevenWonder.Contracts;

namespace SevenWonder.Entities.Events
{
    public class EffectInfoEvent : IEvent
    {
        IEffect effect;
        object info;
        object oldInfo;

        public EffectInfoEvent(IEffect effect, object info)
        {
            this.effect = effect;
            this.info = info;
        }

        public void Commit()
        {
            oldInfo = effect.Info;
            effect.Info = info;
        }
    }
}
