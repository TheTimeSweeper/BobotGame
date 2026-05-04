using SpellCasting;
using SpellCasting.Projectiles;
using UnityEngine;

namespace ActiveStates.Elements.Air
{
    public class AirShake : ElementSpawnProjectile
    {
        protected override ProjectileController prefab => ((ElementTypeAir)elementType).BlowPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 12);
        }
    }
}