using ActiveStates.Characters;
using SpellCasting;
using System;
using UnityEngine;

namespace ActiveStates.Bobots
{
    public class BobotKick : GenericMeleeCombo, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        protected override string hitboxName => "KickHitbox";
        protected override string effectOriginName => "KickEffectOrigin";
        protected override float damageCoefficient => StateInfo.Kick_Damage;
        protected override float baseCastStartTimeFraction => StateInfo.Kick_StartTimeFraction;
        protected override float baseCastEndTimeFraction => StateInfo.Kick_EndTimeFraction;
        protected override float baseDuration => StateInfo.Kick_Duration;
        protected override float baseOtherStateInterruptTimeFraction => StateInfo.Kick_OtherStateInterruptTimeFraction;
        protected override float baseMovementInterruptTimeFraction => StateInfo.Kick_baseMovementInterruptTimeFraction;

        protected override float preAttackMoveShift => StateInfo.BPC_positionShift;

        public override void OnEnter()
        {
            base.OnEnter();

            animator.Play("Kick", 0, 0);
            animator.SetFloat("kick.playbackRate", StateInfo.Kick_AnimationSpeed * baseCastEndTimeFraction);
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
            Vector3 scale = new Vector3(hits == 1 ? -1 : 1, 1, 1);
            Vector3 aimOut = inputBank.AimOut;
            aimOut.y = 0;
            EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, base.characterModel.ChildLocator.LocateByName(effectOriginName).transform.position, Util.DirectionQuaternion(aimOut), scale, characterModel.CharacterDirection.transform);
        }

    }
}
