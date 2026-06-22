using System.Collections.Generic;
using UnityEngine;
namespace SpellCasting
{
    [System.Serializable]
    public class CharacterStats
    {
        public VariableNumberStat MaxHealth = 100;
        public VariableNumberStat HealthRegenPercent = 0.169f;
        public VariableNumberStat Damage = 10;
        public VariableNumberStat AttackSpeed = 1;
        public VariableNumberStat StunFactor = 1;
        public VariableNumberStat KnockbackFactor = 1;

        public VariableNumberStat MaxStamina = 100;
        public VariableNumberStat StaminaRegen = 100;
        public VariableNumberStat StaminaCostCoeff = 1;
        public VariableNumberStat CooldownSpeed = 1;

        public VariableNumberStat CastRange;

        public VariableNumberStat MoveSpeed = 20;
        public VariableNumberStat JumpHeight;

        public CharacterStats() { }

        public CharacterStats(float defaultAll)
        {
            MaxHealth = defaultAll;
            HealthRegenPercent = defaultAll;
            Damage = defaultAll;
            AttackSpeed = defaultAll;
            StunFactor = defaultAll;
            KnockbackFactor = defaultAll;
            MaxStamina = defaultAll;
            StaminaRegen = defaultAll;
            StaminaCostCoeff = defaultAll;
            CooldownSpeed = defaultAll;
            CastRange = defaultAll;
            MoveSpeed = defaultAll;
            JumpHeight = defaultAll;
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

        [SerializeField]
        private float corePositionHeightOffset;
        [SerializeField, Tooltip("Body's core position transform defaults to the transform of this component.")]
        private Transform overrideCorePositionTransform;

        public Vector3 corePosition => overrideCorePositionTransform
                ? overrideCorePositionTransform.position + Vector3.up * corePositionHeightOffset
                : transform.position + Vector3.up * corePositionHeightOffset;

        public TeamIndex teamIndex => commonComponents.TeamComponent.TeamIndex;

        public bool Ded => commonComponents.HealthComponent.Ded;

        public CharacterMaster Master { get; set; }
        public bool isPlayerControlled => Master ? Master.isPlayerControlled : false;

        [ContextMenu("ReInitStats")]
        void Awake()
        {
            commonComponents.HealthComponent.Init(stats.MaxHealth);
            commonComponents.StaminaComponent?.Init();

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

    }
}