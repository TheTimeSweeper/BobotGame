using UnityEngine;
using UnityEngine.Windows;

namespace SpellCasting
{
    public class ElementCursorIndicatorBehavior : ElementInputBehavior
    {
        public ElementCursorIndicatorBehavior(CommonComponentsHolder commonComponents_) : base(commonComponents_) { }

        private CursorIndicator _currentIndicator;
        private CursorIndicator currentIndicator
        {
            get
            {
                if (_currentIndicator == null)
                {
                    if (currentCastingElement == null)
                        return null;

                    _currentIndicator = Object.Instantiate(currentCastingElement.CursorIndicatorPrefab);
                    _currentIndicator.Color = currentCastingElement.ElementColor;
                    _currentIndicator.Caster = caster.transform;
                }
                return _currentIndicator;
            }
        }

        public override void FixedUpdate()
        {
            if (currentIndicator != null)
            {
                if (currentCastingElement != null && input.Down)
                {
                    _currentIndicator.transform.position = inputBank.AimPoint;
                    _currentIndicator.gameObject.SetActive(true);

                    _currentIndicator.Color = currentCastingElement.ElementColor;
                }
                else
                {
                    _currentIndicator.gameObject.SetActive(false);
                }
            }
        }

        public override void OnManipluatorExit()
        {
            base.OnManipluatorExit();
            if (_currentIndicator != null)
            {
                Object.Destroy(_currentIndicator.gameObject);
            }
        }
    }
}