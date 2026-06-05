using UnityEngine;
using ActiveStates.Characters;
using SpellCasting;
using System;
using UnityEditor;

namespace ActiveStates.Bobots
{

    public class BobotPunchCombo : GenericMeleeCombo, IHasStateInfo<BobotGameDevStateInfo>
    {
        protected override int comboHits => 2;
        protected override string hitboxName => "PunchHitbox";
        protected override float damageCoefficient => StateInfo.BPC_Damage;
        protected override float baseCastStartTimeFraction => StateInfo.BPC_StartTimeFraction;
        protected override float baseCastEndTimeFraction => StateInfo.BPC_EndTimeFraction;
        protected override float baseDuration => StateInfo.BPC_Duration;
        protected override float baseOtherStateInterruptTimeFraction => StateInfo.BPC_OtherStateInterruptTimeFraction;
        protected override float baseMovementInterruptTimeFraction => 1;
        protected override float positionShift => 0;
        
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        public override void OnEnter()
        {
            base.OnEnter();
            fixedMotorDriver.OverrideVelocity = Vector3.zero;
            animator.Play("Punch");
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
    }
}
