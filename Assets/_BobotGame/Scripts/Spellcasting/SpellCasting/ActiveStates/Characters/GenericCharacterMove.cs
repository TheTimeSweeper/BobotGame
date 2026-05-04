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
                animator.SetFloat("rightSpeed", inputBank.AimMoveDirection.x);
                animator.SetFloat("forwardSpeed", inputBank.AimMoveDirection.z);
                //animator.SetFloat("walkSpeed", fixedMotorDriver.FinalVeolcity.magnitude);
            }

            if (skillController && inputBank)
            {

                //proto WOOPS IMMEDIATELY BREAK SOC
                HandleSkill(skillController.primarySkill, inputBank.M1);
                HandleSkill(skillController.secondarySkill, inputBank.M2);
                HandleSkill(skillController.utilitySkill, inputBank.Space);
            }
        }

        private void HandleSkill(SkillSlot primarySkill, InputState m1)
        {
            if (m1.Down)
            {
                primarySkill.TryCastSkill();
            }
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            fixedMotorDriver.Direction = UnityEngine.Vector3.zero;
        }
    }
}
