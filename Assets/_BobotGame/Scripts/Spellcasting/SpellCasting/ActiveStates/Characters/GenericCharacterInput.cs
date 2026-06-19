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
    }
}
