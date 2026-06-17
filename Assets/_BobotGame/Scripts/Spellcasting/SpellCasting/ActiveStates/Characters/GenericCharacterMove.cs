using System;
using SpellCasting;

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

            if (skillController && inputBank)
            {

                //moofa proto WOOPS IMMEDIATELY BREAK SOC
                HandleSkill(skillController.PrimarySkill, inputBank.Primary);
                HandleSkill(skillController.BlockSkill, inputBank.Block);
                HandleSkill(skillController.DashSkill, inputBank.Space);
                HandleSkill(skillController.CrouchSkill, inputBank.Shift);
                HandleSkill(skillController.HeavySkill, inputBank.Heavy);
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
            fixedMotorDriver.DesiredDirection = UnityEngine.Vector3.zero;
        }
    }
}
