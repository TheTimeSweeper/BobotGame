using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    public abstract class UIBar : MonoBehaviour
    {
        public abstract void Init(MonoBehaviour component);
    }
    public class UIBar<TComponent> : UIBar where TComponent : MonoBehaviour, IUIBarProvider
    {
        [SerializeField]
        private Slider healthSlider;

        [SerializeField]
        private Image delayedSlider;
        [SerializeField]
        private TMP_Text healthText;


        [SerializeField, Tooltip("the gameobject to be disabled if the bar provider decides to be hidden. leave null to autofill this gameobject")]
        private GameObject optionalAnchorGameObject;
        private GameObject currentAnchor => optionalAnchorGameObject ? optionalAnchorGameObject : gameObject;

        [SerializeField]
        private bool displayMax;

        protected TComponent _component;
        protected IUIBehindBarProvider _behindComponent;

        public void Init(TComponent component_)
        {
            _component = component_;
            if(_component is IUIBehindBarProvider)
            {
                _behindComponent = _component as IUIBehindBarProvider;
            }
            currentAnchor.SetActive(true);
        }

        public override void Init(MonoBehaviour component)
        {
            if (!component)
                return;

            Init(component as TComponent);
        }

        private void Update()
        {
            if (!_component)
            {
                currentAnchor.SetActive(false);
                return;
            }
            currentAnchor.SetActive(_component.GetUIShouldShow());
            UpdateBar();
        }

        protected virtual void UpdateBar()
        {
            if (healthSlider)
            {
                healthSlider.value = _component.GetUICurrentValue() / _component.GetUIMaxValue();

            }
            if (delayedSlider)
            {
                if (_component is IUIBehindBarProvider behindBarComponent)
                {
                    delayedSlider.fillAmount = behindBarComponent.GetUIBehindCurrentValue() / behindBarComponent.GetUIBehindMaxValue();
                }
                else
                {
                    delayedSlider.fillAmount = Util.ExpDecayLerp(delayedSlider.fillAmount, healthSlider.value, 3, Time.deltaTime);
                }
            }
            if (healthText)
            {
                if (displayMax)
                {
                    healthText.text = $"{_component.GetUICurrentValue().ToString("0")}/{_component.GetUIMaxValue().ToString("0")}";
                }
                else
                {
                    healthText.text = _component.GetUICurrentValue().ToString("0");
                }
            }
        }
    }
}