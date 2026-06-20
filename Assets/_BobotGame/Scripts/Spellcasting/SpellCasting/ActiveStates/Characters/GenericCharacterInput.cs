using SpellCasting;

namespace ActiveStates.Characters
{
    public class GenericCharacterInput : ActiveState
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (skillController && inputBank)
            {
                HandleSkill(skillController.PrimarySkill, inputBank.Primary);
                HandleSkill(skillController.BlockSkill, inputBank.Block);
                HandleSkill(skillController.DashSkill, inputBank.Dash);
                HandleSkill(skillController.CrouchSkill, inputBank.Crouch);
                HandleSkill(skillController.AbilitySkill, inputBank.Ability);
            }
        }

        private void HandleSkill(SkillSlot primarySkill, InputState inputState)
        {
            primarySkill.InputSkill(inputState);
        }
    }
}
