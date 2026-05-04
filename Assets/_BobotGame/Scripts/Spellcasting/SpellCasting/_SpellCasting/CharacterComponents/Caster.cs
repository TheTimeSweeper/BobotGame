using System;
using System.Collections.Generic;
using UnityEngine;
namespace SpellCasting
{
    public class Caster : MonoBehaviour
    {
        //jam not quite sure how shitty this is lol
        public Dictionary<InputState, ElementType> InputToElement = new Dictionary<InputState, ElementType>();

        protected List<ElementInputBehavior> _elementInputBehaviors = new List<ElementInputBehavior>();
        public List<ElementInputBehavior> ElementInputBehaviors => _elementInputBehaviors;

        public ElementType CurrentCastingElement { get; set; }
        public InputState CurrentCastingInput { get; set; }

        public virtual ElementType TryGetInputElement(InputState currentPrimaryInput)
        {
            return InputToElement.TryGetValueDefault(currentPrimaryInput);
        }
    }
}