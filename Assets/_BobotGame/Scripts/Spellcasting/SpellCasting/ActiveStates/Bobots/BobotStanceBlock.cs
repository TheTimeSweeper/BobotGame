using SpellCasting;
using System;
using ActiveStates.Characters;

namespace ActiveStates.Bobots
{
    public class BobotStanceBlock : BasicTimedState, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        public float height => StateInfo.Crouch_Height;

        public InputState input { get; set; }

        protected override float baseDuration => StateInfo.block_duration;

        public override void OnEnter()
        {
            base.OnEnter();

            animator.Play("Block");
            animator.SetFloat("block.playbackRate", StateInfo.block_AnimationMultiplier / duration);

            characterBody.stats.MoveSpeed.ApplyMultiplyModifier(0.8f, "Block");
            //todo bobot: BLASPHEMY
            gameObject.GetComponent<TEMPBobotCrouchController>().isBlocking = true;
        }
        public override void OnExit(bool machineDed = false)
        {
            characterBody.stats.MoveSpeed.RemoveModifier("Block");
            gameObject.GetComponent<TEMPBobotCrouchController>().isBlocking = false;
            base.OnExit(machineDed);
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.STATE_MED;
        }
    }
}
