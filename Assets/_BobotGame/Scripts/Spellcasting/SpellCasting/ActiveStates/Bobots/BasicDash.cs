using UnityEngine;
using ActiveStates.Characters;
using SpellCasting;
using System;

namespace ActiveStates.Bobots
{
    public class BasicDash : GenericTimedState, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        //todo basicdashaparameters
        protected override TimedStateParams stateParams => StateInfo.DASH_params;
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

            fixedMotorDriver.OverrideVelocity = direction * curve.Evaluate(fixedAge / stateParams.baseCastEndTimeFraction) * StateInfo.Dash_DashSpeed;
        }

        //public override InterruptPriority GetMinimumInterruptPriority()
        //{
        //    return InterruptPriority.STATE_LOW;
        //}
    }
}
