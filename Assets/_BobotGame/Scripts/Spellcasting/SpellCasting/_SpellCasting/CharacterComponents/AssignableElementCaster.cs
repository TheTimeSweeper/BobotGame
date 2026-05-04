using UnityEngine;
namespace SpellCasting
{
    public class AssignableElementCaster : Caster, IHasCommonComponents
    {
        [SerializeField]
        private InputBank inputBank;

        [SerializeField]
        private CommonComponentsHolder commonComponents;

        [SerializeField]
        private ElementType m1Element;

        [SerializeField]
        private ElementType m2Element;

        [SerializeField]
        private ElementType shiftElement;

        [SerializeField]
        private ElementType spaceElement;
        public CommonComponentsHolder CommonComponents => commonComponents;
        public ElementType M1Element
        {
            get => m1Element;
            set
            {
                //UpdateElement(m1Element, value);
                m1Element = value;
            }
        }
        public ElementType M2Element
        {
            get => m2Element;
            set
            {
                //UpdateElement(m2Element, value);
                m2Element = value;
            }
        }
        public ElementType ShiftElement
        {
            get => shiftElement;
            set
            {
                //UpdateElement(shiftElement, value);
                shiftElement = value;
            }
        }
        public ElementType SpaceElement
        {
            get => spaceElement;
            set
            {
                //UpdateElement(spaceElement, value);
                spaceElement = value;
            }
        }

        private void AddInputElement(ElementType element, InputState input)
        {
            if (element != null)
            {
                InputToElement.Add(input, element);
            }
        }

        private void Awake()
        {
            _elementInputBehaviors.Add(new ElementMassManipulationBehavior(commonComponents));
            _elementInputBehaviors.Add(new ElementSelectionBehaviour(commonComponents));
            _elementInputBehaviors.Add(new ElementCursorIndicatorBehavior(commonComponents));
            _elementInputBehaviors.Add(new ElementColorBehavior(commonComponents));

            AddInputElement(m1Element, inputBank.M1);
            AddInputElement(m2Element, inputBank.M2);
            AddInputElement(shiftElement, inputBank.Shift);
            AddInputElement(spaceElement, inputBank.Space);
        }

    }
}