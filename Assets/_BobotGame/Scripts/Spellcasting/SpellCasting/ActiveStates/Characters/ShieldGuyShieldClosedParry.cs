using SpellCasting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveStates.Characters
{
    public class ShieldGuyShieldClosedParry : BasicMeleeAttack, IHasStateInfo<BobotGameDevStateInfo>
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        protected override TimedStateParams stateParams => StateInfo.SGT_primary_time;
        protected override BasicMeleeParams meleeParams => StateInfo.SGT_primary_melee;

        private bool parrying => Mathf.Abs(fixedAge - castStartTime) < StateInfo.SGT_ParryWindow;

        private List<CharacterBody> pre_parries;

        public override void OnEnter()
        {
            base.OnEnter();

            healthComponent.OnDamageTaken += HealthComponent_OnDamageTaken;
        }
        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            healthComponent.OnDamageTaken -= HealthComponent_OnDamageTaken;
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            for (int i = 0; i < pre_parries.Count; i++)
            {
                ParryBody(pre_parries[i]);
            }
            pre_parries.Clear();
        }

        private void HealthComponent_OnDamageTaken(GetDamagedData getDamagedInfo)
        {
            if (parrying)
            {
                if (fixedAge < castStartTime)
                {
                    pre_parries.Add(getDamagedInfo.DamagingInfo.AttackerBody);
                }
                else
                {
                    ParryBody(getDamagedInfo.DamagingInfo.AttackerBody);
                }
            }
        }

        private void ParryBody(CharacterBody attackerBody)
        {
            attackerBody.CommonComponents.HealthComponent.TakeDamage(
                new DamagingData(
                    characterBody,
                    characterBody.stats.Damage * StateInfo.SGT_ParryDamage,
                    StateInfo.SGT_ParryStun));
        }
    }
}
