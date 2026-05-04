using SpellCasting;
using SpellCasting.Projectiles;
using UnityEngine;

namespace ActiveStates.Elements.Fire
{
    public class FireShake : ElementSpawnProjectile
    {
        protected override ProjectileController prefab => ((ElementTypeFire)elementType).ExplosionPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 16);
        }
    }
}