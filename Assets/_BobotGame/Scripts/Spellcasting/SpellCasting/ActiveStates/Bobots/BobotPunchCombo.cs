using UnityEngine;
using ActiveStates.Characters;
using SpellCasting;

namespace ActiveStates.Bobots
{
    public class BobotPunchCombo : GenericMeleeCombo
    {
        protected override int comboHits => 2;
        protected override string hitboxName => "PunchHitbox";
        protected override float damageCoefficient => 1;
        protected override float baseCastStartTimeFraction => 0.3f;
        protected override float baseCastEndTimeFraction => 0.6f;
        protected override float baseDuration => 1f;
        protected override float baseOtherStateInterruptTimeFraction => 0.6f;
        protected override float baseMovementInterruptTimeFraction => 1;
        protected override float positionShift => 0;
        protected float dashTime => 0.2f;

        private Vector3? goalVelocity;

        public override void OnEnter()
        {
            base.OnEnter();
            fixedMotorDriver.OverrideVelocity = Vector3.zero;

            if(Util.CastHurtBox(inputBank, out var raycastHit))
            {
                Vector3 goalPosition = raycastHit.point - inputBank.AimOut * 3;
                goalPosition.y = transform.position.y;
                Vector3 goalDistance = goalPosition - transform.position;
                goalVelocity = goalDistance / dashTime;
                animator.Play("Dash");
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

        private void EnterSwing()
        {
            if (hits == 1)
            {
                ModifyHit2();
            }

            animator.Play(hits == 1 ? "Punch2" : "Punch");
            animator.SetFloat("punch.playbackRate", 1 / castEndTime);
        }

        protected override void OnCastUpdate()
        {
            base.OnCastUpdate();
        }
        protected override void OnCastExit()
        {
            base.OnCastExit();
        }

        protected virtual void ModifyHit2()
        {
            duration *= 1.2f;
            otherStateInterruptTime *= 1.2f;
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            EnterSwing();
            Vector3 scale = new Vector3(hits == 1 ? -1 : 1, 1, 1);
            EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, transform.position, Util.DirectionQuaternion(inputBank.AimOut), scale, characterModel.CharacterDirection.transform);
        }
        public override void OnExit(bool machineDed = false)
        {
            fixedMotorDriver.OverrideVelocity = null;
            base.OnExit(machineDed);
        }
    }
}
