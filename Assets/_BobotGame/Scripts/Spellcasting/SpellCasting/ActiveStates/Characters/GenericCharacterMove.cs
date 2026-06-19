using System;

namespace ActiveStates.Characters
{
    public class GenericCharacterMove : ActiveState
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            fixedMotorDriver.DesiredDirection = inputBank.GlobalMoveDirection;
            fixedMotorDriver.DesiredSpeed = characterBody.stats.MoveSpeed;

            if (inputBank.AimOut != default)
            {
                characterModel.CharacterDirection.DesiredDirection = inputBank.AimOut;
            }
            else
            {
                characterModel.CharacterDirection.DesiredDirection = fixedMotorDriver.DesiredDirection;
            }

            if (animator)
            {
                animator.SetFloat("rightSpeed", inputBank.LocalMoveDirection.x);
                animator.SetFloat("forwardSpeed", inputBank.LocalMoveDirection.z);
                animator.SetFloat("walkSpeed", fixedMotorDriver.FinalVelocity.magnitude);
            }
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            fixedMotorDriver.DesiredDirection = UnityEngine.Vector3.zero;
        }
    }
}
