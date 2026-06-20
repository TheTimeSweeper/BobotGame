using SpellCasting;
using SpellCasting.AI;
using UnityEngine;

namespace ActiveStates.AI
{
    public class ChaseToCombat : AITargetState
    {
        public float ChaseTime = 1;

        public override void OnEnter()
        {
            base.OnEnter();
            CurrentGesture = Brain.RollGesture();

            Vector3 difference = Brain.CurrentTargetBody.transform.position - transform.position;

            Brain.AIInputController.MoveDirection = difference;
        }    

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (Brain.CurrentTargetBody == null)
            {
                Machine.SetStateToDefault();
                return;
            }

            Vector3 difference = Brain.CurrentTargetBody.transform.position - transform.position;

            if (difference.magnitude >= CurrentGesture.CloseDistasnce)
            {
                Brain.AIInputController.MoveDirection = difference;
            } 
            else
            {
                Brain.AIInputController.MoveDirection = Vector3.zero;

                if (fixedAge > ChaseTime)
                {
                    Machine.SetState(new Combat { Brain = Brain, CurrentGesture = CurrentGesture });
                }
            }
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            Brain.AIInputController.MoveDirection = Vector3.zero;
        }
    }
}
