using SpellCasting;

namespace ActiveStates.Elements.Fire
{
    public class FireSwipe : ElementThrowProjectile
    {

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 15);
        }
    }
}