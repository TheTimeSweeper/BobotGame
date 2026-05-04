using UnityEngine;
namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/BuffInfo/Default", fileName = "Buff")]
    public class BuffInfo : ScriptableObject
    {
        [SerializeField]
        private string buffID;

        [SerializeField]
        private EffectPooled pooledEffect;

        [SerializeField]
        private Sprite icon;

        [SerializeField, Tooltip("currently only maxhealth, damage, basemovespeed, maxMana, manaRegen, healthRegen")]
        public CharacterStats statsToChange;

        public virtual void OnApply(CharacterBody body)
        {
            if (pooledEffect != null)
            {
                EffectManager.SpawnEffect(pooledEffect.effectIndex, body.transform.position, Quaternion.identity, body.transform);
            }

            if (statsToChange.BaseMaxHealth > 0)
            {
                body.stats.MaxHealth.ApplyMultiplyModifier(statsToChange.BaseMaxHealth, buffID);
                body.CommonComponents.HealthComponent.UpdateMaxHealth(body.stats.MaxHealth, true);
            }

            if (statsToChange.BaseDamage > 0)
            {
                body.stats.Damage.ApplyMultiplyModifier(statsToChange.BaseDamage, buffID);
            }

            if (statsToChange.BaseMoveSpeed > 0)
            {
                body.stats.MoveSpeed.ApplyMultiplyModifier(statsToChange.BaseMoveSpeed, buffID);
            }

            if (statsToChange.BaseMaxMana > 0)
            {
                body.stats.MaxFireMana.ApplyMultiplyModifier(statsToChange.BaseMaxMana, buffID);
                body.stats.MaxEarthMana.ApplyMultiplyModifier(statsToChange.BaseMaxMana, buffID);
                body.stats.MaxWaterMana.ApplyMultiplyModifier(statsToChange.BaseMaxMana, buffID);
                body.stats.MaxAirMana.ApplyMultiplyModifier(statsToChange.BaseMaxMana, buffID);
            }

            if (statsToChange.BaseManaRegeneration > 0)
            {
                body.stats.ManaRegeneration.ApplyMultiplyModifier(statsToChange.BaseManaRegeneration, buffID);
            }

            if (statsToChange.BaseHealthRegenPercent > 0)
            {
                body.stats.HealthRegenPercent.ApplyMultiplyModifier(statsToChange.BaseHealthRegenPercent, buffID);
            }

            //jam and so on and so forth
        }

        public virtual void OnUnapply(CharacterBody body)
        {
            if (statsToChange.BaseMaxHealth > 0)
            {
                body.stats.MaxHealth.RemoveModifier(buffID);
                body.CommonComponents.HealthComponent.UpdateMaxHealth(body.stats.MaxHealth, false);
            }

            if (statsToChange.BaseDamage > 0)
            {
                body.stats.Damage.RemoveModifier(buffID);
            }

            if (statsToChange.BaseMoveSpeed > 0)
            {
                body.stats.MoveSpeed.RemoveModifier(buffID);
            }

            if (statsToChange.BaseManaRegeneration > 0)
            {
                body.stats.ManaRegeneration.RemoveModifier(buffID);
            }

            if (statsToChange.BaseHealthRegenPercent > 0)
            {
                body.stats.HealthRegenPercent.RemoveModifier(buffID);
            }
        }
    }
}