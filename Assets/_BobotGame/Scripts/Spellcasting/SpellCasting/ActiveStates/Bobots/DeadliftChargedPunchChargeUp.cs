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
            animator.Play("ChargePunch");
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
                animator.Play("BufferEmpty", 1);
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

        //todo bobot bring back the easy fuclkin properties...
            //guess I'll fuckin just add simpleoverride conditionals for each of these like i did the baseduration etc
        protected float chargedDamageCoefficient => Mathf.Lerp(StateInfo.CPunch_damageMin, StateInfo.CPunch_damageMax, chargeAmount);
        //protected override float damageCoefficient => Mathf.Lerp(StateInfo.CPunch_damageMin, StateInfo.CPunch_damageMax, chargeAmount);
        protected float chargedknockbackCoefficient => Mathf.Lerp(StateInfo.CPunch_knockbackMin, StateInfo.CPunch_knockbackMax, chargeAmount);
        //protected override float knockbackCoefficient => Mathf.Lerp(StateInfo.CPunch_knockbackMin, StateInfo.CPunch_knockbackMax, chargeAmount);

        //protected override float preAttackMoveShift => StateInfo.BPC_positionShift;

        public float chargeAmount = 0.5f;

        public DeadliftChargedPunchRelease()
        {
        }
        public DeadliftChargedPunchRelease(float charge)
        {
            chargeAmount = charge;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            PlayKickAnimation();
        }
        protected override void ModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.ModifyOverlapAttack(overlapAttack);
            overlapAttack.Damage = chargedDamageCoefficient * characterBody.stats.Damage;
            //the fact that I had to look at the base class assignment of this find out what to put here is a big fuckin problem
            overlapAttack.KnockbackForceCoefficient = chargedknockbackCoefficient;
        }

        private void PlayKickAnimation()
        {
            animator.Play("ThrowPunch", 1, 0);
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
