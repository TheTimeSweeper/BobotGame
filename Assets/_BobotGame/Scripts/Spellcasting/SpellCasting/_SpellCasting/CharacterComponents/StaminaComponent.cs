using System;
using UnityEngine;

namespace SpellCasting
{
    public class StaminaComponent : MonoBehaviour, IHasCommonComponents
    {
        [SerializeField]
        private CommonComponentsHolder commonComponents;
        public CommonComponentsHolder CommonComponents => commonComponents;

        public VariableNumberStat MaxStamina => commonComponents.CharacterBody.stats.MaxStamina;
        private float currentStamina;

        internal void Init()
        {
            MaxStamina.onValueChanged += MaxStamina_onValueChanged;
        }

        private void MaxStamina_onValueChanged(float newValue)
        {
            currentStamina = newValue;
        }
    }
}