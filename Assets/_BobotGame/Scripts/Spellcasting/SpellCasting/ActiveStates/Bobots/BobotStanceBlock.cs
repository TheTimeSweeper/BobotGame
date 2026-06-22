using SpellCasting;
using System;
using ActiveStates.Characters;

namespace ActiveStates.Bobots
{
    public class BobotStanceBlock : BasicTimedStateSimple, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        protected override float baseDuration => StateInfo.block_duration;

        public InputState input { get; set; }

        bool blocked;
        bool unsubscribed;

        public override void OnEnter()
        {
            base.OnEnter();

            PlayAnimation("Body", "Block", "block.playbackRate", baseDuration);            

            characterBody.stats.MoveSpeed.ApplyMultiplyModifier(0.8f, "Block");
            //todo bobot: BLASPHEMY
            gameObject.GetComponent<TEMPBobotCrouchController>().isBlocking = true;

            healthComponent.OnDamageTaken += HealthComponent_OnDamageTaken;
            characterBody.stats.KnockbackFactor.ApplyMultiplyModifier(0.2f, "block");
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if(fixedAge > StateInfo.block_duration_blocking_fraction * baseDuration)
            {
                Unsubscribe();
            }
        }

        private void Unsubscribe()
        {
            if (unsubscribed)
            {
                return;
            }
            unsubscribed = true;
            healthComponent.OnDamageTaken -= HealthComponent_OnDamageTaken;
        }

        private void HealthComponent_OnDamageTaken(GetDamagedData getDamagedInfo)
        {
            blocked = true;
        }

        public override void OnExit(bool machineDed = false)
        {
            Unsubscribe();
            characterBody.stats.KnockbackFactor.RemoveModifier("block");
            characterBody.stats.MoveSpeed.RemoveModifier("Block");
            gameObject.GetComponent<TEMPBobotCrouchController>().isBlocking = false;
            base.OnExit(machineDed);
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (blocked)
            {
                return InterruptPriority.STATE_ANY;
            }
            return InterruptPriority.STATE_MED;
        }
    }
}
