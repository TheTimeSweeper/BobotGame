using SpellCasting;

namespace ActiveStates.Elements.Light
{
    public class LightSwipe : BaseElementMassState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            if (animator != null)
            {
                //animator.Update(0f);
                animator.PlayInFixedTime("Cast", -1, 0);
            }

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 18);

            healthComponent.Heal(new SpellCasting.HealingData { HealValue = 0.3f * characterBody.stats.MaxHealth});
            elementMass.Fizzle();
        }
    }
}