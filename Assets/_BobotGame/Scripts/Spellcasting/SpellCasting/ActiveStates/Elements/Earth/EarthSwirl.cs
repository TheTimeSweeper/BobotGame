using SpellCasting;
using SpellCasting.Projectiles;

namespace ActiveStates.Elements.Earth
{

    public class EarthSwirl : ElementSpawnProjectile
    {
        protected override ProjectileController prefab => elementType.ExplosionPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 13);
        }

        protected override DamageTypeIndex GetDamageType()
        {
            return DamageTypeIndex.STUN;
        }
    }
}