using SpellCasting.Projectiles;
using SpellCasting;
using UnityEngine;

namespace ActiveStates.Elements.Air
{
    public class AirSwirl : ElementSpawnProjectile
    {
        protected override ProjectileController prefab => ((ElementTypeAir)elementType).SuccPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 12);
        }
    }
}