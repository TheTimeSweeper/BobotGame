using ActiveStates.Characters;
using SpellCasting;
using System;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace ActiveStates.Bobots
{

    public class DeadliftChargedPunchChargeUp : GenericTimedState, IHasStateInfo<BobotGameDevStateInfo>, IStateFromInput
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        public InputState input { get ; set ; }

        public override float? simpleOverrideBaseDuration => 1;
        public override float? simpleOverrideBaseCastStartTimeFraction => 0;
        public override float? simpleOverrideBaseCastEndTimeFraction => StateInfo.CPunch_chargeTime / characterBody.stats.AttackSpeed;
        //todo bobot one step forward two steps back
        //protected override bool attackSpeedAffected => false;
        protected TimedStateParams timedStateParamse;
        protected override TimedStateParams stateParams => timedStateParamse;
        protected override void InitDurationValues()
        {
            timedStateParamse = new TimedStateParams(simpleOverrideBaseDuration, simpleOverrideBaseCastStartTimeFraction, simpleOverrideBaseCastEndTimeFraction);
            timedStateParamse.attackSpeedAffected = false;
            timedStateParamse.baseExtraEndDelayFraction = StateInfo.CPunch_holdGiveupTime;

            base.InitDurationValues();
        }
        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            PlayAnimation("YayGesture", "ChargePunch", "punch.playbackRate", castEndTime);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (!input.Down)
            {
                SetNextState();
                return;
            }
        }

        protected override void OnCastFixedUpdate()
        {
            base.OnCastFixedUpdate();

            //float chargeValue = Mathf.Clamp01(fixedAge / castEndTime);
            //play animation/sfx/vfx at charge value
        }

        protected override void SetNextState()
        {
            Machine.SetState(new DeadliftChargedPunchRelease(Mathf.Clamp01(fixedAge / castEndTime)));
        }

        public override void ModifyNextState(ref ActiveState state)
        {
            base.ModifyNextState(ref state);
            if(state is not DeadliftChargedPunchRelease)
            {
                //todo bobot really need that playanimation system, man
                animator.SetTrigger("cancel");
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.STATE_MED;
        }
    }

    public class DeadliftChargedPunchRelease : BasicMeleeAttack, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        protected override TimedStateParams stateParams => StateInfo.CPunch_ReleaseParams;
        protected override BasicMeleeParams meleeParams => finalMeleeParams;
        protected BasicMeleeParams finalMeleeParams;
        protected BasicMeleeParams meleeParams1 => StateInfo.CPunch_meleeReleaseParams;
        protected BasicMeleeParams meleeParams2 => StateInfo.CPunch_meleeReleaseParamsMaxCharge;


        //protected override float knockbackCoefficient => Mathf.Lerp(StateInfo.CPunch_knockbackMin, StateInfo.CPunch_knockbackMax, chargeAmount);

        //protected override float preAttackMoveShift => StateInfo.BPC_positionShift;

        public float chargeAmount = -1;

        public DeadliftChargedPunchRelease()
        {
        }
        public DeadliftChargedPunchRelease(float charge)
        {
            chargeAmount = charge;
        }

        public override void OnEnter()
        {
            if (chargeAmount < 0)
            {
                chargeAmount = StateInfo.CPunch_InstantPunchCharge;
            }
            if (chargeAmount < 1)
            {
                stateParams.baseExtraEndDelayFraction = 0;
            }
            else
            {
                stateParams.baseExtraEndDelayFraction = 2;
            }
            BlendMeleeParams();

            base.OnEnter();
            PlayTimedAnimation();
        }

        private void BlendMeleeParams()
        {
            finalMeleeParams = new BasicMeleeParams
            {
                hitboxName = chargeAmount < 1 ? meleeParams1.hitboxName : meleeParams2.hitboxName,
                effectOriginName = chargeAmount < 1 ? meleeParams1.effectOriginName : meleeParams2.effectOriginName,
                damageCoefficient = Mathf.Lerp(meleeParams1.damageCoefficient, meleeParams2.damageCoefficient, chargeAmount),
                stunTime = Mathf.Lerp(meleeParams1.stunTime, meleeParams2.stunTime, chargeAmount),
                knockbackCoefficient = Mathf.Lerp(meleeParams1.knockbackCoefficient, meleeParams2.knockbackCoefficient, chargeAmount),
                preAttackMoveShift = Mathf.Lerp(meleeParams1.preAttackMoveShift, meleeParams2.preAttackMoveShift, chargeAmount),
                preAttackMoveShiftDecay = Mathf.Lerp(meleeParams1.preAttackMoveShiftDecay, meleeParams2.preAttackMoveShiftDecay, chargeAmount),
                attackMoveShift = Mathf.Lerp(meleeParams1.attackMoveShift, meleeParams2.attackMoveShift, chargeAmount),
                attackMoveShiftDecay = Mathf.Lerp(meleeParams1.attackMoveShiftDecay, meleeParams2.attackMoveShiftDecay, chargeAmount),
                staminaRecoveryOnHit = Mathf.Lerp(meleeParams1.staminaRecoveryOnHit, meleeParams2.staminaRecoveryOnHit, chargeAmount),
            };
        }

        protected override void ModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.ModifyOverlapAttack(overlapAttack);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (fixedAge >= movementInterruptTime)
            {
                return InterruptPriority.MOVEMENT;
            }
            if (fixedAge >= otherStateInterruptTime)
            {
                return InterruptPriority.STATE_LOW;
            }
            return InterruptPriority.STATE_MED;
        }

        public override void OnFixedUpdate()
        {
            if (fixedAge < movementInterruptTime)
            {
                fixedMotorDriver.OverrideVelocity = Vector3.zero;
            }
            base.OnFixedUpdate();
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            //todo bobot what is this mess. it just cost me like 20 minutes
            //EnterSwing();
            //Vector3 scale = new Vector3( == 1 ? -1 : 1, 1, 1);
            //Vector3 aimOut = inputBank.AimOut;
            //aimOut.y = 0;
            //EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, base.characterModel.ChildLocator.LocateByName(effectOriginName).transform.position, Util.DirectionQuaternion(aimOut), scale, characterModel.CharacterDirection.transform);
        }
    }
}
