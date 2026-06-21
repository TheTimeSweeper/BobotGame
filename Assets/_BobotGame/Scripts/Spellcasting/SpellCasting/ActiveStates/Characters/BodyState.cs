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

        protected void ResetAnimationMovementFloats()
        {
            if (animator)
            {
                animator.SetFloat("rightSpeed", 0);
                animator.SetFloat("forwardSpeed", 0);
                animator.SetFloat("walkSpeed", 0);
            }
        }

        protected void SetAnimationMovementFloats()
        {
            if (animator)
            {
                animator.SetFloat("rightSpeed", inputBank.LocalMoveDirection.x);
                animator.SetFloat("forwardSpeed", inputBank.LocalMoveDirection.z);
                animator.SetFloat("walkSpeed", fixedMotorDriver.FinalVelocity.magnitude);
            }
        }

        protected void SetAimForward()
        {
            if (inputBank.AimOut != default)
            {
                characterModel.CharacterDirection.DesiredDirection = inputBank.AimOut;
            }
            else
            {
                characterModel.CharacterDirection.DesiredDirection = fixedMotorDriver.DesiredDirection;
            }
        }
    }
}
