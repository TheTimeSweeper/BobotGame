using UnityEngine;
using SpellCasting;
using System;

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
            base.OnExit(machineDed);
            characterBody.stats.MoveSpeed.RemoveModifier("Crouch");
            characterModel.transform.SetLocalPositionAndRotation(Vector3.zero, characterModel.transform.rotation);
        }
    }
}
