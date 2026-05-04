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

        private float cooldownTimer;
        private ActiveStateMachine machine;

        public void Recharge()
        {
            if (skillInfo == null)
                return;

            cooldownTimer -= Time.fixedDeltaTime * cooldownSpeed;
            if(cooldownTimer < 0)
            {
                if (skillInfo.autoCast)
                {
                    TryCastSkill();
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

        public void TryCastSkill()
        {
            if (skillInfo == null)
                return;

            if (cooldownTimer > 0)
                return;

            cooldownTimer = cooldownTime;

            machine.TryInterruptState(skillInfo.state, skillInfo.interruptingPriority);
        }
    }
}