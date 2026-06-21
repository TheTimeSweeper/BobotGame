using System;

namespace ActiveStates.Characters
{
    public class GenericCharacterMove : BodyState
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            fixedMotorDriver.DesiredDirection = inputBank.GlobalMoveDirection;
            fixedMotorDriver.DesiredSpeed = characterBody.stats.MoveSpeed;

            SetAimForward();

            SetAnimationMovementFloats();
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            ResetAnimationMovementFloats();
            fixedMotorDriver.DesiredDirection = UnityEngine.Vector3.zero;
        }
    }
}
