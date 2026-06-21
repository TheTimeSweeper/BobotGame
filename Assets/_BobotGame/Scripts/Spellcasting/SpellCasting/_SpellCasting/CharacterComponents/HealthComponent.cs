using JetBrains.Annotations;
using System;
using System.Globalization;
using UnityEngine;
namespace SpellCasting
{

    public class HealthComponent : MonoBehaviour, IHasCommonComponents, IUIBarProvider
    {
        public delegate void ModifyDamageCallback(GetDamagedData getDamagedInfo);
        public event ModifyDamageCallback PreModifyDamage;
        public delegate void DamageTakenEvent(GetDamagedData getDamagedInfo);
        public event DamageTakenEvent OnDamageTaken;

        public delegate void ModifyHealCallback(HealingData damage);
        public event ModifyHealCallback PreModifyHeal;
        public delegate void HealTakenEvenet(float healAmount);
        public event HealTakenEvenet OnHealTaken;

        [SerializeField]
        protected float health;
        public float Health { get => health; }

        public float MaxHealth => commonComponents.CharacterBody.stats.MaxHealth;

        [SerializeField]
        protected CommonComponentsHolder commonComponents;
        public CommonComponentsHolder CommonComponents => commonComponents;

        [SerializeField]
        private DeathComponent deathComponent;

        public bool Ded => health <= 0;

        public void Init(float health)
        {
            this.health = health;

            //todo bobot not healing
            //commonComponents.CharacterBody.stats.MaxHealth.onValueChanged += MaxHealth_onValueChanged;
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
        protected virtual void TakeTheDamage(ref float healthToHit, GetDamagedData info)
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

            health = Mathf.Clamp(health + heal.HealValue, 0, MaxHealth);
            OnHealTaken?.Invoke(heal.HealValue);
        }

        //todo bobot not healing
        public void UpdateMaxHealth(float newMaxHealth, bool heal)
        {
            float maxHealthDelta = newMaxHealth - MaxHealth;
            //commonComponents.CharacterBody.stats.MaxHealth = newMaxHealth;
            if (heal)
            {
                health += maxHealthDelta;
            }
        }

        //private void MaxHealth_onValueChanged(float oldValue, float newValue)
        //{

        //}

        void FixedUpdate()
        {
            if (commonComponents != null && commonComponents.CharacterBody != null)
            {
                float regenPerSecond = commonComponents.CharacterBody.stats.HealthRegenPercent * commonComponents.CharacterBody.stats.MaxHealth;

                health = Mathf.Clamp(health + regenPerSecond * Time.fixedDeltaTime, 0, MaxHealth);
            }
        }

        public float GetUICurrentValue()
        {
            return health;
        }

        public float GetUIMaxValue()
        {
            return MaxHealth;
        }

        public bool GetUIShouldShow()
        {
            return !Ded;
        }
    }
}