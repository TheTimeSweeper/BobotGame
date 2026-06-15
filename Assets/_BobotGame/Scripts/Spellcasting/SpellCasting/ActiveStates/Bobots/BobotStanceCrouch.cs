using UnityEngine;
using SpellCasting;
using System;
using ActiveStates.Characters;

namespace ActiveStates.Bobots
{
    public class BobotStanceCrouch : ActiveState, IHasStateInfo<BobotGameDevStateInfo>, IStateFromInput
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        public float height => StateInfo.Crouch_Height;

        public InputState input { get ; set ; }

        public override void OnEnter()
        {
            base.OnEnter();

            characterModel.transform.SetLocalPositionAndRotation(Vector3.up * height, characterModel.transform.rotation);

            characterBody.stats.MoveSpeed.ApplyMultiplyModifier(0.5f, "Crouch");
            //todo bobot: BLASPHEMY
            gameObject.GetComponent<TEMPBobotCrouchController>().isCrouched = true;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (!input.Down)
            {
                EndState();
            }
        }
        public override void OnExit(bool machineDed = false)
        {
            characterBody.stats.MoveSpeed.RemoveModifier("Crouch");
            characterModel.transform.SetLocalPositionAndRotation(Vector3.zero, characterModel.transform.rotation);
            gameObject.GetComponent<TEMPBobotCrouchController>().isCrouched = false;
            base.OnExit(machineDed);
        }
    }
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
        public override void OnFixedUpdate()
        {
            if (fixedAge < movementInterruptTime)
            {
                fixedMotorDriver.OverrideVelocity = Vector3.zero;
            }
            base.OnFixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.STATE_MED;
        }
    }
}
