using SpellCasting;
using SpellCasting.AI;

namespace ActiveStates.AI
{
    public class Combat : AITargetState
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (Brain.CurrentTargetBody == null)
            {
                Machine.SetStateToDefault();
                return;
            }

            bool ended = Gesture.OnFixedUpdate(Brain);

            if (ended)
            {
                if (Brain.CurrentTargetBody != null)
                {
                    Machine.SetState( new ChaseToCombat { Brain = Brain, ChaseTime = Brain.chaseTimeMinimunm } );
                }
                else
                {
                    Machine.SetStateToDefault();
                }
            }
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            Gesture.End(Brain);
        }
    }
}
