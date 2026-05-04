using UnityEngine;

namespace ActiveStates.Characters
{
    public abstract class BodyState : ActiveState
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            CheckMovementInterruption();
        }

        private void CheckMovementInterruption()
        {
            if (!inputBank)
                return;

            if (GetMinimumInterruptPriority() == InterruptPriority.MOVEMENT && inputBank.GlobalMoveDirection != Vector3.zero)
            {
                OnMovementInterrupt();
            }
        }

        protected virtual void OnMovementInterrupt()
        {
            Machine.SetStateToDefault();
        }
    }
}
