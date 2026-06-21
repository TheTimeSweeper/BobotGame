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
        protected override TimedStateParams stateParams => CurrentComboHit switch
        {
            0 => StateInfo.BPC_HitParams,
            1 => StateInfo.BPC_HitParams2,
            2 => StateInfo.BPC_HitParams3
        }; protected override BasicMeleeParams meleeParams => CurrentComboHit switch
        {
            0 => StateInfo.BPC_MeleeParams1,
            1 => StateInfo.BPC_MeleeParams2,
            2 => StateInfo.BPC_MeleeParams3
        };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

        public override void OnEnter()
        {
            base.OnEnter();
            //fixedMotorDriver.OverrideVelocity = Vector3.zero;
            PlayTimedAnimation();
            //animator.Play(CurrentComboHit < 1 ? "Punch" : "Punch2", 0, 0);
            //animator.SetFloat("punch.playbackRate", StateInfo.BPC_AnimationSpeed / castEndTime);
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
