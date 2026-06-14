using UnityEngine;
using ActiveStates.Characters;
using SpellCasting;
using System;

namespace ActiveStates.Bobots
{
    public class BasicDash : BasicTimedState, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        protected override float baseCastStartTimeFraction => 0;
        protected override float baseCastEndTimeFraction => 1;
        protected override float baseOtherStateInterruptTimeFraction => StateInfo.Dash_InterruptTime;
        protected override float baseDuration => StateInfo.Dash_Duration;
        protected float dashTime => StateInfo.Dash_DashTime;
        protected AnimationCurve curve => StateInfo.Dash_DashSpeedCurve;

        protected Vector3 direction;

        public override void OnEnter()
        {
            base.OnEnter();
            animator.Play("Dash");
            animator.SetFloat("dash.playbackRate", StateInfo.Dash_AnimationSpeed / duration);
            direction = inputBank.GlobalMoveDirection;
            if(inputBank.GlobalMoveDirection.sqrMagnitude <= Mathf.Epsilon)
            {
                direction = characterModel.transform.forward;
            }
            direction = direction.normalized;
        }
        protected override void OnCastFixedUpdate()
        {
            base.OnCastFixedUpdate();

            fixedMotorDriver.OverrideVelocity = direction * curve.Evaluate(fixedAge / dashTime) * StateInfo.Dash_DashSpeed;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.STATE_LOW;
        }
    }
}
