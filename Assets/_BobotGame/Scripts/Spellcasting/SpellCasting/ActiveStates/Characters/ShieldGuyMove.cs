using ActiveStates.Bobots;
using SpellCasting;
using UnityEngine;

namespace ActiveStates.Characters
{
    public class ShieldGuyMove : BodyState
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            
            Vector3 localMoveDirection = inputBank.LocalMoveDirection;

            gameObject.GetComponent<TEMPBobotCrouchController>().blockingbothupperandlower = true;

            if (inputBank.Block.Down)
            {
                PlayerInputControllerFirstPerson.TEMP_xLock = false;
                PlayAnimation("ShieldGuy", "Closed");

                localMoveDirection = Vector3.zero;

                //todo bobot: BLASPHEMY
                gameObject.GetComponent<TEMPBobotCrouchController>().isBlocking = true;
            }
            else
            {
                PlayerInputControllerFirstPerson.TEMP_xLock = true;
                PlayAnimation("ShieldGuy", "Open");

                fixedMotorDriver.DesiredDirection = inputBank.GlobalMoveDirection;

                localMoveDirection.x = 0;

                //todo bobot: BLASPHEMY
                gameObject.GetComponent<TEMPBobotCrouchController>().isBlocking = false;
            }
            fixedMotorDriver.DesiredDirection = inputBank.AimOrigin.TransformDirection(localMoveDirection);
            fixedMotorDriver.DesiredSpeed = characterBody.stats.MoveSpeed;


            characterModel.CharacterDirection.DesiredDirection = inputBank.AimOut;

            if (animator)
            {
                animator.SetFloat("rightSpeed", localMoveDirection.x);
                animator.SetFloat("forwardSpeed", localMoveDirection.z);
                animator.SetFloat("walkSpeed", fixedMotorDriver.FinalVelocity.magnitude);
            }
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            ResetAnimationMovementFloats();
            fixedMotorDriver.DesiredDirection = UnityEngine.Vector3.zero;
        }
    }
}
