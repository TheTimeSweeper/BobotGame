using System;
using ActiveStates;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private float bufferTimer = -1;
        private InputState bufferedState;

        private CommonComponentsHolder commonComponents;

        public void FixedUpdate()
        {
            if (skillInfo == null)
                return;
            UpdateBuffer();
            RechargeCooldown();
        }

        private void UpdateBuffer()
        {
            if(bufferTimer >= 0 && bufferedState != null)
            {
                bufferTimer -= Time.deltaTime;
                TryCastSkill(bufferedState);
            }
        }

        private void RechargeCooldown()
        {
            cooldownTimer -= Time.deltaTime * cooldownSpeed;
            if (cooldownTimer < 0)
            {
                if (autoCast)
                {
                    InputSkill(null);
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

        public void InputSkill(InputState inputState)
        {
            if((!skillInfo.justPress && inputState.Down) || (skillInfo.justPress && inputState.JustPressed(this)))
            {
                bufferedState = inputState;
                bufferTimer = skillInfo.bufferTime;
            }
        }

        public bool TryCastSkill(InputState inputState)
        {
            if (skillInfo == null)
                return false;

            if (cooldownTimer > 0)
                return false;

            cooldownTimer = cooldownTime;

            if(machine.TryInterruptState(skillInfo.state, skillInfo.interruptingPriority, out ActiveState state)){
                if (state is IStateFromInput stateFromInput)
                {
                    stateFromInput.input = inputState;
                }
                bufferTimer = 0;
                bufferedState = null;
                return true;
            }

            return false;
        }
    }
}