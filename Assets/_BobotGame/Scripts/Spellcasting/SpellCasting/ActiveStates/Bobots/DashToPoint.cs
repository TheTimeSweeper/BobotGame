using UnityEngine;
using ActiveStates.Characters;
using SpellCasting;
using System;

namespace ActiveStates.Bobots
{
    public class DashToPoint : BasicTimedState, IHasStateInfo<BobotGameDevStateInfo>
    {
        protected override float baseCastStartTimeFraction => StateInfo.Dash_StartTimeFraction;
        protected override float baseDuration => StateInfo.Dash_Duration;
        protected float dashTime => StateInfo.Dash_DashTime;

        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        private Vector3? goalVelocity;
        public override void OnEnter()
        {
            base.OnEnter();
            animator.Play("Dash");
            animator.SetFloat("dash.playbackRate", StateInfo.Dash_AnimationSpeed / duration);

            if (Util.CastHurtBox(inputBank, out var raycastHit))
            {
                Vector3 goalPosition = raycastHit.point - inputBank.AimOut * 4;
                goalPosition.y = transform.position.y;
                Vector3 goalDistance = goalPosition - transform.position;
                goalVelocity = goalDistance / dashTime;
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (fixedAge < dashTime)
            {
                fixedMotorDriver.OverrideVelocity = goalVelocity;
            }
            else
            {
                fixedMotorDriver.OverrideVelocity = null;
            }
        }
        public override void OnExit(bool machineDed = false)
        {
            fixedMotorDriver.OverrideVelocity = null;
            base.OnExit(machineDed);
        }
    }
}
