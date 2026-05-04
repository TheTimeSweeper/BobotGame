using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Slider healthSlider;

        [SerializeField]
        private Image delayedSlider;
        [SerializeField]
        private TMP_Text healthText;

        [SerializeField]
        private bool diplayMax;

        private HealthComponent _healthComponent;

        public void Init(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (_healthComponent == null)
            {
                gameObject.SetActive(false);
                return;
            }
            if (_healthComponent.Ded)
            {
                gameObject.SetActive(false);
            }

            if (healthSlider != null)
            {
                healthSlider.value = _healthComponent.Health / _healthComponent.MaxHealth;

                if (delayedSlider != null)
                {
                    delayedSlider.fillAmount = Util.ExpDecayLerp(delayedSlider.fillAmount, healthSlider.value, 3, Time.deltaTime);
                }
            }
            if (healthText != null)
            {
                if (diplayMax)
                {
                    healthText.text = $"{_healthComponent.Health.ToString("0")}/{_healthComponent.MaxHealth.ToString("0")}";
                }else
                {
                    healthText.text = _healthComponent.Health.ToString("0");
                }
            }
        }

    }
}