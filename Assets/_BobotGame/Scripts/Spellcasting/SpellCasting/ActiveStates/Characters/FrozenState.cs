using SpellCasting;

namespace ActiveStates.Characters
{
    public class FrozenState : StunnedState
    {
        private float _minFrozenTime = 0.3f;
        private bool _modifiedDamage;
        private bool _unfreezed;

        public override void OnEnter()
        {
            base.OnEnter();
            healthComponent.PreModifyDamage += HealthComponent_PreModifyDamage;
            healthComponent.OnDamageTaken += HealthComponent_OnDamageTaken;
        }

        private void HealthComponent_PreModifyDamage(GetDamagedData getDamagedInfo)
        {
            if(getDamagedInfo.DamagingInfo.DamageValue <= 0)
                return;
            if (fixedAge < _minFrozenTime)
                return;
            if (_modifiedDamage)
                return;
            _modifiedDamage = true;

            getDamagedInfo.DamagingInfo.DamageValue *= (DamageTypeCatalog.GetCatalogItem(DamageTypeIndex.FREEZE) as StunningDamageType).damageMultipler;
        }

        private void HealthComponent_OnDamageTaken(GetDamagedData getDamagedInfo)
        {
            if (getDamagedInfo.DamagingInfo.DamageValue <= 0)
                return;
            if (fixedAge < _minFrozenTime)
                return;
            if (_unfreezed)
                return;
            _unfreezed = true;

            Machine.EndState();
        }

        public override void OnExit(bool machineded = false)
        {
            base.OnExit(machineded);

            healthComponent.OnDamageTaken -= HealthComponent_OnDamageTaken;
            healthComponent.PreModifyDamage -= HealthComponent_PreModifyDamage;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return fixedAge > _minFrozenTime ? InterruptPriority.FREEZE : InterruptPriority.DEATH;
        }
    }
}