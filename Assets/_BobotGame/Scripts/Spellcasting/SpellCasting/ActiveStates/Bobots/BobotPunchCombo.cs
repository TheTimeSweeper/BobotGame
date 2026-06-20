using UnityEngine;
using ActiveStates.Characters;
using SpellCasting;
using System;
using UnityEditor;

namespace ActiveStates.Bobots
{
    public class BobotPunchCombo : GenericMeleeCombo, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        public override int ComboHits => 3;

#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
        protected override TimedStateParams timedStateParams => CurrentComboHit switch
        {
            0 => StateInfo.BPC_HitParams,
            1 => StateInfo.BPC_HitParams2,
            2 => StateInfo.BPC_HitParams3
        };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

        //protected override string hitboxName => "PunchHitbox";
        //protected override string effectOriginName => "PunchEffectOrigin";
        //protected override float damageCoefficient => StateInfo.BPC_Damage;
        //protected override float baseCastStartTimeFraction => StateInfo.BPC_StartTimeFraction;
        //protected override float baseCastEndTimeFraction => StateInfo.BPC_EndTimeFraction;
        //protected override float baseDuration
        //{
        //    get
        //    {
        //        switch (currentComboHit)
        //        {

        //            default:
        //            case 0: return StateInfo.BPC_Duration;
        //            case 1: return StateInfo.BPC_Duration2;
        //            case 2: return StateInfo.BPC_Duration3;
        //        }
        //    }
        //}
        //protected override float baseExtraEndDelayFraction =>
        //    currentComboHit < comboHits - 1
        //        ? 0
        //        : StateInfo.BPC_EndDuration3;
        //protected override float baseOtherStateInterruptTimeFraction => StateInfo.BPC_OtherStateInterruptTimeFraction;
        //protected override float baseMovementInterruptTimeFraction => StateInfo.BPC_baseMovementInterruptTimeFraction;
        //protected override float attackMoveShift => 0;
        //protected override float preAttackMoveShift => 
        //    currentComboHit < 1
        //        ? StateInfo.BPC_positionShift
        //        : StateInfo.BPC_positionShift2;

        public override void OnEnter()
        {
            base.OnEnter();
            //fixedMotorDriver.OverrideVelocity = Vector3.zero;
            animator.Play(CurrentComboHit < 1 ? "Punch" : "Punch2", 0, 0);
            animator.SetFloat("punch.playbackRate", StateInfo.BPC_AnimationSpeed / castEndTime);
        }

        protected override void OnCastUpdate()
        {
            base.OnCastUpdate();
        }
        protected override void OnCastFixedUpdate()
        {
            base.OnCastFixedUpdate();
        }
        protected override void OnCastExit()
        {
            base.OnCastExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (fixedAge >= movementInterruptTime)
            {
                return InterruptPriority.MOVEMENT;
            }
            if (fixedAge >= otherStateInterruptTime)
            {
                return InterruptPriority.STATE_ANY;
            }
            return InterruptPriority.STATE_LOW;
        }
    }
}
