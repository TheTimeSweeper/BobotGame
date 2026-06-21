using UnityEngine;
using ActiveStates.Characters;
using SpellCasting;
using System;

namespace ActiveStates.Bobots
{
    public class DashToPoint : GenericTimedState, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        protected override TimedStateParams stateParams => StateInfo.DASH_params;

        private Vector3? goalVelocity;
        public override void OnEnter()
        {
            base.OnEnter();
            PlayTimedAnimation();
            //animator.Play("Dash");
            //animator.SetFloat("dash.playbackRate", StateInfo.Dash_AnimationSpeed / duration);

            //if (Util.CastHurtBox(inputBank, out var raycastHit))
            Ray bodyRay = inputBank.GetBodyRay();
            Vector3 goalPosition = bodyRay.origin + bodyRay.direction * 100;
            if (Physics.Raycast(bodyRay, out RaycastHit raycastHit, 100, LayerInfo.CommonMasks.WorldOrBody))
            {
                goalPosition = raycastHit.point - inputBank.AimOut * 4;
            }
            goalPosition.y = transform.position.y;
            Vector3 goalDistance = goalPosition - transform.position;
            goalVelocity = goalDistance / castEndTime;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (fixedAge < castEndTime)
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
