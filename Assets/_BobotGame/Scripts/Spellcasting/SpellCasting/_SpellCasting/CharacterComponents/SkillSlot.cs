using System;
using ActiveStates;
using UnityEngine;

namespace SpellCasting
{
    [System.Serializable]
    public class SkillSlot
    {
        public SkillInfo skillInfo;
        public VariableNumberStat cooldownTime;
        public VariableNumberStat cooldownSpeed;
        public SkillButton skillButton;
        public bool autoCast;

        private float cooldownTimer;
        private ActiveStateMachine machine;

        public void Recharge()
        {
            if (skillInfo == null)
                return;

            cooldownTimer -= Time.deltaTime * cooldownSpeed;
            if(cooldownTimer < 0)
            {
                if (autoCast)
                {
                    TryCastSkill(null);
                }
            }
        }

        public void Init(CommonComponentsHolder commonComponents)
        {
            if (skillInfo == null)
                return;

            machine = commonComponents.StateMachineLocator.LocateByName(skillInfo.stateMachineName);
            cooldownTime = new VariableNumberStat(skillInfo.baseCooldown);
            cooldownSpeed = new VariableNumberStat(1);
        }

        public void TryCastSkill(InputState inputState)
        {
            if (skillInfo == null)
                return;

            if (cooldownTimer > 0)
                return;

            cooldownTimer = cooldownTime;

            if(machine.TryInterruptState(skillInfo.state, skillInfo.interruptingPriority, out ActiveState state)){
                if (state is IStateFromInput stateFromInput)
                {
                    stateFromInput.input = inputState;
                }
            }
        }
    }
}