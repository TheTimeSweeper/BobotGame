using System;
using UnityEngine;

namespace SpellCasting
{
    public class StaminaComponent : MonoBehaviour, IHasCommonComponents, IUIBarProvider
    {
        [SerializeField]
        private CommonComponentsHolder commonComponents;
        public CommonComponentsHolder CommonComponents => commonComponents;

        public VariableNumberStat MaxStamina => commonComponents.CharacterBody.stats.MaxStamina;
        public VariableNumberStat StainaRegen => commonComponents.CharacterBody.stats.StaminaRegen;
        public float currentStamina { get; set; }

        public void Init()
        {
            MaxStamina.onValueChanged += MaxStamina_onValueChanged;
        }

        private void MaxStamina_onValueChanged(float oldValue, float newValue)
        {
            currentStamina = newValue;
        }

        void FixedUpdate()
        {
            RefreshStamina(StainaRegen * Time.deltaTime);
        }

        public void ConsumeStamina(float staminaCost)
        {
            currentStamina = Mathf.Max(0, currentStamina - staminaCost);
        }

        public float GetUICurrentValue()
        {
            return currentStamina;
        }

        public float GetUIMaxValue()
        {
            return MaxStamina;
        }

        public bool GetUIShouldShow()
        {
            //todo bobot just parent this under health?
            return !commonComponents.HealthComponent.Ded;
        }

        public void RefreshStamina(float refreshAmount)
        {
            currentStamina = Mathf.Min(MaxStamina, currentStamina + refreshAmount);
        }
    }
}