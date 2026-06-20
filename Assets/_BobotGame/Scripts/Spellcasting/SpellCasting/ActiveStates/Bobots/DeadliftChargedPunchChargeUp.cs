using ActiveStates.Characters;
using SpellCasting;
using System;
using UnityEngine;

namespace ActiveStates.Bobots
{

    public class DeadliftChargedPunchChargeUp : BasicTimedState, IHasStateInfo<BobotGameDevStateInfo>, IStateFromInput
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        public InputState input { get ; set ; }

        //protected override float baseDuration => StateInfo.CPunch_holdGiveupTime;
        //protected override float baseCastStartTimeFraction => 0;
        //protected override float baseCastEndTimeFraction => (StateInfo.CPunch_damageMax / StateInfo.CPunch_holdGiveupTime) / characterBody.stats.AttackSpeed;
        //protected override bool attackSpeedAffected => false;

        protected override TimedStateParams timedStateParams => throw new NotImplementedException();

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

        protected override TimedStateParams timedStateParams => throw new NotImplementedException();

        //protected override string hitboxName => "KickHitbox";
        //protected override string effectOriginName => "PunchEffectOrigin";
        //protected override float baseDuration => StateInfo.CPunch_Duration;
        //protected override float baseCastStartTimeFraction => StateInfo.CPunch_StartTimeFraction;
        //protected override float baseCastEndTimeFraction => StateInfo.CPunch_EndTimeFraction;
        //protected override float damageCoefficient => Mathf.Lerp(StateInfo.CPunch_damageMin, StateInfo.CPunch_damageMax, chargeAmount);
        //protected override float baseOtherStateInterruptTimeFraction => StateInfo.Kick_OtherStateInterruptTimeFraction;
        //protected override float baseMovementInterruptTimeFraction => StateInfo.Kick_baseMovementInterruptTimeFraction;
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

        private void PlayKickAnimation()
        {
            animator.Play("ThrowPunch");
            //    animator.Play("Kick", 0, 0);
            //    animator.SetFloat("kick.playbackRate", StateInfo.Kick_AnimationSpeed * baseCastEndTimeFraction);
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
