using ActiveStates.Characters;
using SpellCasting;
using System;
using UnityEngine;

namespace ActiveStates.Bobots
{
    public class BobotKick : BasicMeleeAttack, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }  
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        protected override TimedStateParams stateParams => StateInfo.Kick_params;

        public override void OnEnter()
        {
            base.OnEnter();
            PlayKickAnimation();
        }

        private void PlayKickAnimation()
        {
            animator.Play("Kick", 0, 0);
            animator.SetFloat("kick.playbackRate", StateInfo.Kick_AnimationSpeed * stateParams.baseCastEndTimeFraction);
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

        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            //todo bobot what is this mess. it just cost me like 20 minutes
            ////EnterSwing();
            //Vector3 scale = new Vector3(currentComboHit == 1 ? -1 : 1, 1, 1);
            //Vector3 aimOut = inputBank.AimOut;
            //aimOut.y = 0;
            //EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, base.characterModel.ChildLocator.LocateByName(effectOriginName).transform.position, Util.DirectionQuaternion(aimOut), scale, characterModel.CharacterDirection.transform);
        }

    }
}
