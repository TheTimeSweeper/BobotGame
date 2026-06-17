using JetBrains.Annotations;
using System;
using System.Globalization;
using UnityEngine;
namespace SpellCasting
{

    public class HealthComponent : MonoBehaviour, IHasCommonComponents
    {
        public delegate void DamageTakenEvent(GetDamagedData getDamagedInfo);
        public event DamageTakenEvent OnDamageTaken;
        public delegate void ModifyDamageCallback(GetDamagedData getDamagedInfo);
        public event ModifyDamageCallback PreModifyDamage;

        public delegate void HealTakenEvenet(float healAmount);
        public event HealTakenEvenet OnHealTaken;
        public delegate void ModifyHealCallback(HealingData damage);
        public event ModifyHealCallback PreModifyHeal;

        [SerializeField]
        protected float health;
        public float Health { get => health; }

        [SerializeField]
        private float maxHealth;
        public float MaxHealth { get => maxHealth; }

        [SerializeField]
        protected CommonComponentsHolder commonComponents;
        public CommonComponentsHolder CommonComponents => commonComponents;

        [SerializeField]
        private DeathComponent deathComponent;

        public bool Ded => health <= 0;

        public void Init(float health)
        {
            maxHealth = health;
            this.health = health;
        }

        public virtual void TakeDamage(DamagingData damage)
        {
            CharacterBody body = null;

            if (commonComponents != null)
            {
                body = commonComponents.CharacterBody;

            }

            GetDamagedData info = new GetDamagedData
            {
                VictimHealth = this,
                VictimBody = body,
                DamagingInfo = damage
            };

            DamageTypeCatalog.PreModifyDamageAll(info);

            PreModifyDamage?.Invoke(info);

            JudgeTheDamage(info);

            DamageTypeCatalog.OnTakeDamageAll(info);

            if (Ded && deathComponent != null)
            {
                deathComponent.GetRektLol();
            }
        }

        protected virtual void JudgeTheDamage(GetDamagedData info)
        {
            TakeTheDamage(ref health, info);
            SpawnEffect(info);
        }

        //todo bobot nooooo we are doing a takedamage now nooo
        protected virtual void TakeTheDamage(ref float healthToHit, GetDamagedData info, bool callEvent = true)
        {
            healthToHit -= info.DamagingInfo.DamageValue;

            OnDamageTaken?.Invoke(info);
        }

        private void SpawnEffect(GetDamagedData info)
        {
            EffectIndex effect = info.DamagingInfo.AttackerBody.teamIndex == TeamIndex.MONSTER ? EffectIndex.DAMAGENUMBER_FROMENEMY : EffectIndex.DAMAGENUMBER;

            EffectManager.SpawnEffect(effect, transform.position, null, (int)info.DamagingInfo.DamageValue);
        }

        public void Heal(HealingData heal)
        {
            PreModifyHeal?.Invoke(heal);

            health = Mathf.Clamp(health + heal.HealValue, 0, maxHealth);
            OnHealTaken?.Invoke(heal.HealValue);
        }

        public void UpdateMaxHealth(float newMaxHealth, bool heal)
        {
            float maxHealthDelta = newMaxHealth - maxHealth;
            maxHealth = newMaxHealth;
            if (heal)
            {
                health += maxHealthDelta;
            }
        }

        void FixedUpdate()
        {
            if (commonComponents != null && commonComponents.CharacterBody != null)
            {
                float regenPerSecond = commonComponents.CharacterBody.stats.HealthRegenPercent * commonComponents.CharacterBody.stats.MaxHealth;

                health = Mathf.Clamp(health + regenPerSecond * Time.fixedDeltaTime, 0, maxHealth);
            }
        }
    }
}