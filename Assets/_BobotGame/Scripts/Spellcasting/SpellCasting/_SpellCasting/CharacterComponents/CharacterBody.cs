using System.Collections.Generic;
using UnityEngine;
namespace SpellCasting
{
    [System.Serializable]
    public class CharacterStats
    {
        public float BaseMaxHealth = 100;
        public float BaseHealthRegenPercent = 0.169f;
        public float BaseDamage = 10;
        public float BaseAttackSpeed = 1;
        public float BaseStunFactor = 1;
        public float BaseKnockbackFactor = 1;

        public float BaseMaxMana = 100;
        public float BaseManaRegeneration = 1;
        public float BaseManaCostMultiplier = 1;

        public float BaseCastRange = 20;

        public float BaseMoveSpeed = 20;
        public float BaseJumpHeight;

        public VariableNumberStat MaxHealth;
        public VariableNumberStat HealthRegenPercent;
        public VariableNumberStat Damage;
        public VariableNumberStat AttackSpeed;
        public VariableNumberStat StunFactor;
        public VariableNumberStat KnockbackFactor;

        public VariableNumberStat MaxFireMana;
        public VariableNumberStat MaxEarthMana;
        public VariableNumberStat MaxWaterMana;
        public VariableNumberStat MaxAirMana;
        public VariableNumberStat ManaRegeneration;
        public VariableNumberStat ManaCostMultiplier;

        public VariableNumberStat CastRange;

        public VariableNumberStat MoveSpeed;
        public VariableNumberStat JumpHeight;

        public void Init()
        {
            MaxHealth = new VariableNumberStat(BaseMaxHealth);
            HealthRegenPercent = new VariableNumberStat(BaseHealthRegenPercent);

            Damage = new VariableNumberStat(BaseDamage);
            AttackSpeed = new VariableNumberStat(BaseAttackSpeed);
            StunFactor = new VariableNumberStat(BaseStunFactor);
            KnockbackFactor = new VariableNumberStat(BaseKnockbackFactor);

            MaxFireMana = new VariableNumberStat(BaseMaxMana);
            MaxEarthMana = new VariableNumberStat(BaseMaxMana);
            MaxWaterMana = new VariableNumberStat(BaseMaxMana);
            MaxAirMana = new VariableNumberStat(BaseMaxMana);
            ManaRegeneration = new VariableNumberStat(BaseManaRegeneration);
            ManaCostMultiplier = new VariableNumberStat(BaseManaCostMultiplier);

            //not inplemented
            CastRange = new VariableNumberStat(BaseCastRange);

            MoveSpeed = new VariableNumberStat(BaseMoveSpeed);
            JumpHeight = new VariableNumberStat(BaseJumpHeight);
        }
    }

    [RequireComponent(typeof(CommonComponentsHolder))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(StateMachineLocator))]
    [RequireComponent(typeof(TeamComponent))]
    public class CharacterBody : MonoBehaviour, IHasCommonComponents
    {
        List<BuffInfo> activebuffs = new List<BuffInfo>();

        List<PowerItem> Inventory = new List<PowerItem>();

        public CharacterStats stats;

        [SerializeField]
        private CommonComponentsHolder commonComponents;
        public CommonComponentsHolder CommonComponents => commonComponents;

        [SerializeField]
        private bool dontDestroyOnLoad;

        public TeamIndex teamIndex => commonComponents.TeamComponent.TeamIndex;

        public bool Ded => commonComponents.HealthComponent.Ded;

        public float FireMana {get => CommonComponents.ManaComponent.FireMana; set => CommonComponents.ManaComponent.FireMana = value; }
        public float EarthMana { get => CommonComponents.ManaComponent.EarthMana; set => CommonComponents.ManaComponent.EarthMana = value; }
        public float WaterMana { get => CommonComponents.ManaComponent.WaterMana; set => CommonComponents.ManaComponent.WaterMana = value; }
        public float AirMana { get => CommonComponents.ManaComponent.AirMana; set => CommonComponents.ManaComponent.AirMana = value; }

        [ContextMenu("ReInitStats")]
        void Awake()
        {
            stats.Init();
            commonComponents.HealthComponent.Init(stats.MaxHealth);

            if (commonComponents.ManaComponent)
            {
                FireMana = stats.MaxFireMana;
                EarthMana = stats.MaxEarthMana;
                AirMana = stats.MaxAirMana;
                WaterMana = stats.MaxWaterMana;
            }

            if (dontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(this);
            }
        }

        //jam uh yea
        public void GiveItem(PowerItem powerItem)
        {
            if(powerItem.buffDuration > 0)
            {
                AddTimedBuff(powerItem.buffToApply, powerItem.buffDuration);
            } 
            else
            {
                Inventory.Add(powerItem);
                AddBuff(powerItem.buffToApply);
            }
        }

        public void AddBuff(BuffInfo buff)
        {
            buff.OnApply(this);
            activebuffs.Add(buff);
        }

        public void AddTimedBuff(BuffInfo buff, float time)
        {
            BuffManager.AddBuffTimer(this, buff, time);
            AddBuff(buff);
        }

        public bool HasBuff(BuffInfo buff)
        {
            return activebuffs.Contains(buff);
        }

        public void Removebuff(BuffInfo buff)
        {
            if (activebuffs.Contains(buff))
            {
                buff.OnUnapply(this);
                activebuffs.Remove(buff);
            }
        }

        ///jam should probably be in manacomponent
        public bool TrySpendMana(ElementTypeIndex currentCastingElement, float manaCost)
        {
            if(commonComponents.ManaComponent == null)
                return true;

            manaCost *= stats.ManaCostMultiplier;

            bool success = GetCanMana(currentCastingElement, manaCost);

            if (success)
            {
                switch (currentCastingElement)
                {
                    case ElementTypeIndex.FIRE:
                        FireMana -= manaCost;
                        return true;
                    case ElementTypeIndex.EARTH:
                        EarthMana -= manaCost;
                        return true;
                    case ElementTypeIndex.WATER:
                        WaterMana -= manaCost;
                        return true;
                    case ElementTypeIndex.AIR:
                        AirMana -= manaCost;
                        return true;
                    case ElementTypeIndex.METAL:
                        FireMana -= manaCost;
                        EarthMana -= manaCost;
                        return true;
                    case ElementTypeIndex.LAVA:
                        FireMana -= manaCost;
                        WaterMana -= manaCost;
                        return true;
                    case ElementTypeIndex.LIGHTNING:
                        FireMana -= manaCost;
                        AirMana -= manaCost;
                        return true;
                    case ElementTypeIndex.ICE:
                        WaterMana -= manaCost;
                        EarthMana -= manaCost;
                        return true;
                    case ElementTypeIndex.SAND:
                        AirMana -= manaCost;
                        EarthMana -= manaCost;
                        return true;
                    case ElementTypeIndex.LIGHT:
                        AirMana -= manaCost;
                        WaterMana -= manaCost;
                        return true;
                    default:
                        return true;
                }
            }

            return false;
        }

        private bool GetCanMana(ElementTypeIndex currentCastingElement, float manaCost)
        {
            switch (currentCastingElement)
            {
                default:
                    return true;
                case ElementTypeIndex.FIRE:
                    if (FireMana > (manaCost * stats.ManaCostMultiplier))
                    {
                        return true;
                    }
                    break;
                case ElementTypeIndex.EARTH:
                    if (EarthMana > (manaCost * stats.ManaCostMultiplier))
                    {
                        return true;
                    }
                    break;
                case ElementTypeIndex.WATER:
                    if (WaterMana > (manaCost * stats.ManaCostMultiplier))
                    {
                        return true;
                    }
                    break;
                case ElementTypeIndex.AIR:
                    if (AirMana > (manaCost * stats.ManaCostMultiplier))
                    {
                        return true;
                    }
                    break;
                case ElementTypeIndex.METAL:
                    return GetCanMana(ElementTypeIndex.FIRE, manaCost) && GetCanMana(ElementTypeIndex.EARTH, manaCost);
                case ElementTypeIndex.LAVA:
                    return GetCanMana(ElementTypeIndex.FIRE, manaCost) && GetCanMana(ElementTypeIndex.WATER, manaCost);
                case ElementTypeIndex.LIGHTNING:
                    return GetCanMana(ElementTypeIndex.FIRE, manaCost) && GetCanMana(ElementTypeIndex.AIR, manaCost);
                case ElementTypeIndex.ICE:
                    return GetCanMana(ElementTypeIndex.EARTH, manaCost) && GetCanMana(ElementTypeIndex.WATER, manaCost);
                case ElementTypeIndex.SAND:
                    return GetCanMana(ElementTypeIndex.EARTH, manaCost) && GetCanMana(ElementTypeIndex.AIR, manaCost);
                case ElementTypeIndex.LIGHT:
                    return GetCanMana(ElementTypeIndex.WATER, manaCost) && GetCanMana(ElementTypeIndex.AIR, manaCost);
            }
            return false;
        }
    }
}