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

        [SerializeField, Tooltip("currently only maxhealth, damage, basemovespeed, maxMana, manaRegen, healthRegen, staminaRegen")]
        public CharacterStats statsToChange;

        public virtual void OnApply(CharacterBody body)
        {
            if (pooledEffect != null)
            {
                EffectManager.SpawnEffect(pooledEffect.effectIndex, body.transform.position, Quaternion.identity, body.transform);
            }

            if (statsToChange.MaxHealth > 0)
            {
                body.stats.MaxHealth.ApplyMultiplyModifier(statsToChange.MaxHealth, buffID);
                body.CommonComponents.HealthComponent.UpdateMaxHealth(body.stats.MaxHealth, true);
            }

            if (statsToChange.Damage > 0)
            {
                body.stats.Damage.ApplyMultiplyModifier(statsToChange.Damage, buffID);
            }

            if (statsToChange.MoveSpeed > 0)
            {
                body.stats.MoveSpeed.ApplyMultiplyModifier(statsToChange.MoveSpeed, buffID);
            }

            if (statsToChange.HealthRegenPercent > 0)
            {
                body.stats.HealthRegenPercent.ApplyMultiplyModifier(statsToChange.HealthRegenPercent, buffID);
            }
            //todo the rest above
            TryUpdateStat(statsToChange.StaminaRegen, body.stats.StaminaRegen);

            bool TryUpdateStat(VariableNumberStat vari, VariableNumberStat vari2)
            {
                if (vari > 0)
                {
                    vari2.ApplyMultiplyModifier(vari, buffID);
                    return true;
                }
                return false;
            }

            //jam and so on and so forth
        }

        public virtual void OnUnapply(CharacterBody body)
        {
            //todo bobot buffs back?
        }
    }
}