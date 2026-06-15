using SpellCasting.AI;

namespace ActiveStates.AI
{
    public class AITargetState : AIState
    {
        public AIGestureBehavior Gesture;

        public override void OnEnter()
        {
            base.OnEnter();
            if (Gesture!= null)
            {
                Gesture.OnEnter(Brain);
            }
        }
    }
}
