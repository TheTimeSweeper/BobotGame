using SpellCasting;
using SpellCasting.Projectiles;

namespace ActiveStates.Elements.Fire
{
    public class FireSwirl : ElementSpawnProjectile
    {
        protected override ProjectileController prefab => ((ElementTypeFire)elementType).PoolPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 17);
        }
    }
}