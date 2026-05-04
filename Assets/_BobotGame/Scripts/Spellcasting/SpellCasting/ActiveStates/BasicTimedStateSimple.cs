using ActiveStates.Characters;

namespace ActiveStates
{
    public abstract class BasicTimedStateSimple : BodyState
    {
        protected abstract float baseDuration { get; }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (fixedAge >= baseDuration)
            {
                EndState();
            }
        }
    }
}
