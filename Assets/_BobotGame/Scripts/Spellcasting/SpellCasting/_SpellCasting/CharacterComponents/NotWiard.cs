using UnityEngine;

namespace SpellCasting
{
    public class NotWiard : MonoBehaviour
    {
        public SimpleSkill M1Skill;       
        public SimpleSkill M2Skill;       
        public SimpleSkill ShiftSkill;       
        public SimpleSkill SpaceSkill;

        [SerializeField]
        private StateMachineLocator stateMachineLocator;

        [SerializeField]
        private InputBank inputbank;

        public void FixedUpdate()
        {
            if (inputbank.M1.JustPressed(this))
            {
                stateMachineLocator.LocateByName(M1Skill.stateMachine).TryInterruptState(ActiveStateCatalog.InstantiateState(M1Skill.state), M1Skill.priority);
            }

            if (inputbank.M2.JustPressed(this))
            {
                stateMachineLocator.LocateByName(M2Skill.stateMachine).TryInterruptState(ActiveStateCatalog.InstantiateState(M2Skill.state), M2Skill.priority);
            }
            if (inputbank.Shift.JustPressed(this))
            {
                stateMachineLocator.LocateByName(ShiftSkill.stateMachine).TryInterruptState(ActiveStateCatalog.InstantiateState(ShiftSkill.state), ShiftSkill.priority);
            }
            if (inputbank.Space.JustPressed(this))
            {
                stateMachineLocator.LocateByName(SpaceSkill.stateMachine).TryInterruptState(ActiveStateCatalog.InstantiateState(SpaceSkill.state), SpaceSkill.priority);
            }
        }

    }
}