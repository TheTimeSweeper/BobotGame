using SpellCasting;

namespace ActiveStates.Elements.Earth
{

    public class EarthSwipe : ElementThrowProjectile
    {
        protected override DamageTypeIndex GetDamageType()
        {
            return DamageTypeIndex.EARTH | DamageTypeIndex.STUN;
            
        }

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 14);
        }
    }
}