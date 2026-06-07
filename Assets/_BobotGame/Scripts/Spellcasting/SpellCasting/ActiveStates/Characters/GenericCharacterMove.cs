using System;
using SpellCasting;

namespace ActiveStates.Characters
{
    public class GenericCharacterMove : ActiveState
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            fixedMotorDriver.Direction = inputBank.GlobalMoveDirection;
            fixedMotorDriver.DesiredSpeed = characterBody.stats.MoveSpeed;

            if (inputBank.AimOut != default)
            {
                characterModel.CharacterDirection.DesiredDirection = inputBank.AimOut;
            }
            else
            {
                characterModel.CharacterDirection.DesiredDirection = fixedMotorDriver.Direction;
            }

            if (animator)
            {
                animator.SetFloat("rightSpeed", inputBank.LocalMoveDirection.x);
                animator.SetFloat("forwardSpeed", inputBank.LocalMoveDirection.z);
                animator.SetFloat("walkSpeed", fixedMotorDriver.FinalVelocity.magnitude);
            }

            if (skillController && inputBank)
            {

                //moofa proto WOOPS IMMEDIATELY BREAK SOC
                HandleSkill(skillController.PrimarySkill, inputBank.M1);
                HandleSkill(skillController.BlockSkill, inputBank.M2);
                HandleSkill(skillController.DashSkill, inputBank.Space);
                HandleSkill(skillController.CrouchSkill, inputBank.Shift);
                HandleSkill(skillController.SpecialSkill, inputBank.F);
            }
        }

        private void HandleSkill(SkillSlot primarySkill, InputState inputState)
        {
            if (inputState.Down)
            {
                primarySkill.TryCastSkill(inputState);
            }
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            fixedMotorDriver.Direction = UnityEngine.Vector3.zero;
        }
    }
}
