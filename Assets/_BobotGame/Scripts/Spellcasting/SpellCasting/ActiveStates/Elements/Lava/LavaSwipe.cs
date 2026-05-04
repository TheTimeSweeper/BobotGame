using ActiveStates.Elements.Water;
using SpellCasting;

namespace ActiveStates.Elements.Lava
{
    public class LavaSwipe : WaterSwipe
    {
        protected override DamageTypeIndex GetDamageType()
        {
            return DamageTypeIndex.NONE;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 17);
        }
    }
}