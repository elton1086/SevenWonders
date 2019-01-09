using SevenWonders.Contracts;

namespace SevenWonders.Entities.Events
{
    public class EffectInfoEvent : IEvent
    {
        Effect effect;
        object info;
        object oldInfo;

        public EffectInfoEvent(Effect effect, object info)
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
