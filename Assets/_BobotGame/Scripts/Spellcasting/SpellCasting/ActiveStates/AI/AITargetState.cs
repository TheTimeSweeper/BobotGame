using SpellCasting.AI;

namespace ActiveStates.AI
{
    public class AITargetState : AIState
    {
        public AIGestureBehavior CurrentGesture;

        public override void OnEnter()
        {
            base.OnEnter();
            if (CurrentGesture!= null)
            {
                CurrentGesture.OnEnter(Brain);
            }
        }
    }
}
