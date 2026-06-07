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

        protected override int comboHits => 2;
        protected override string hitboxName => "PunchHitbox";
        protected override float damageCoefficient => StateInfo.BPC_Damage;
        protected override float baseCastStartTimeFraction => StateInfo.BPC_StartTimeFraction;
        protected override float baseCastEndTimeFraction => StateInfo.BPC_EndTimeFraction;
        protected override float baseDuration => StateInfo.BPC_Duration;
        protected override float baseOtherStateInterruptTimeFraction => StateInfo.BPC_OtherStateInterruptTimeFraction;
        protected override float baseMovementInterruptTimeFraction => StateInfo.BPC_baseMovementInterruptTimeFraction;
        protected override float attackMoveShift => 0;
        protected override float preAttackMoveShift => StateInfo.BPC_positionShift;

        public override void OnEnter()
        {
            base.OnEnter();
            fixedMotorDriver.OverrideVelocity = Vector3.zero;
            animator.Play("Punch", 0, 0);
            animator.SetFloat("punch.playbackRate", StateInfo.BPC_AnimationSpeed * baseCastEndTimeFraction);
        }

        private void EnterSwing()
        {
            if (hits == 1)
            {
                ModifyHit2();
            }

            //animator.Play(hits == 1 ? "Punch2" : "Punch");
            //animator.SetFloat("punch.playbackRate", 1 / castEndTime);
        }

        public override void OnFixedUpdate()
        {
            if (fixedAge < movementInterruptTime)
            {
                fixedMotorDriver.OverrideVelocity = Vector3.zero;
            }
            base.OnFixedUpdate();
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

        protected override void OnMovementInterrupt()
        {
            //don't change states. we're using the movement interrupt to just  move lol
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //if (fixedAge >= movementInterruptTime)
            //{
            //    return InterruptPriority.MOVEMENT;
            //}
            if (fixedAge >= otherStateInterruptTime)
            {
                return InterruptPriority.STATE_ANY;
            }
            return InterruptPriority.STATE_LOW;
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
    }
}
