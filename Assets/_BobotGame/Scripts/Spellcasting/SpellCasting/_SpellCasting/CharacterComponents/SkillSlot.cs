using System;
using ActiveStates;
using UnityEngine;

namespace SpellCasting
{
    [System.Serializable]
    public class SkillSlot
    {
        public SkillInfo skillInfo;
        private VariableNumberStat cooldownTime;                                  //todo bobot asvalidornull
        private VariableNumberStat cooldownSpeed => commonComponents.CharacterBody ? commonComponents.CharacterBody.stats.CooldownSpeed : 0;
        public SkillButton skillButton;
        public bool autoCast;

        private float cooldownTimer;
        private ActiveStateMachine machine;

        private CommonComponentsHolder commonComponents;

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
            this.commonComponents = commonComponents;
            InitSkillInfo();
        }

        private void InitSkillInfo()
        {
            if (skillInfo == null)
                return;

            machine = this.commonComponents.StateMachineLocator.LocateByName(skillInfo.stateMachineName);
            cooldownTime = skillInfo.baseCooldown;
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