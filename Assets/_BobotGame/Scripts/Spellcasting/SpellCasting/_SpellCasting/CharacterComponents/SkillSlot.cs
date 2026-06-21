using ActiveStates;
using ActiveStates.Characters;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpellCasting
{
    [System.Serializable]
    public class SkillSlot
    {
        public SkillInfo skillInfo;
        public SkillButton skillButton;
        public bool autoCast;

        //skill info decided
        private float cooldownTime;
        private float cooldownSpeed => bodyStats?.CooldownSpeed ?? 1;

        private float cooldownTimer;
        private ActiveStateMachine machine;
        private float staminaCost => skillInfo.baseStaminaCost * (bodyStats?.StaminaCostCoeff ?? 0);

        //buffer
        private float bufferTimer = -1;
        private InputState bufferedState;

        //current
        private CommonComponentsHolder commonComponents;
        private CharacterStats bodyStats;

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
                if (autoCast || skillInfo.autoCast)
                {
                    InputSkill(null);
                }
            }
        }

        public void Init(CommonComponentsHolder commonComponents)
        {
            this.commonComponents = commonComponents;
            bodyStats = commonComponents.CharacterBody.stats;
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

            if(staminaCost > 0 && staminaCost > commonComponents.StaminaComponent.currentStamina)
            {
                return false;
            }

            if(machine.TryInterruptState(skillInfo.state, skillInfo.interruptingPriority, out ActiveState state)){
                //success
                if (state is IStateFromInput stateFromInput)
                {
                    stateFromInput.input = inputState;
                }
                bufferTimer = 0;
                bufferedState = null;

                cooldownTimer = cooldownTime;

                if (staminaCost > 0)
                {
                    commonComponents.StaminaComponent.ConsumeStamina(staminaCost);
                }
                return true;
            }

            return false;
        }
    }
}