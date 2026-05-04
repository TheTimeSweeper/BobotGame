using ActiveStates;
using System;
using System.Xml.Schema;
namespace SpellCasting
{
    public class Wiard : AssignableElementCaster
    {
        public override ElementType TryGetInputElement(InputState currentPrimaryInput)
        {
            ElementType elementType = base.TryGetInputElement(currentPrimaryInput);
            if (MainGame.Instance.SavedData.UnlockedElements.Contains(elementType))
            {
                return elementType;
            }
            return null;
        }
    }
}