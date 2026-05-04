using SpellCasting;
using SpellCasting.Projectiles;

namespace ActiveStates.Elements.Water
{
    public class WaterSwirl : ElementSpawnProjectile
    {
        protected override ProjectileController prefab => elementType.PoolPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 22);
        }

        protected override DamageTypeIndex GetDamageType()
        {
            return DamageTypeIndex.WATER;
        }
    }
}