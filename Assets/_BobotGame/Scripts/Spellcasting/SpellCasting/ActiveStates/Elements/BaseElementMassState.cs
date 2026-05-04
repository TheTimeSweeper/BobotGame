using SpellCasting;
using UnityEngine;

namespace ActiveStates.Elements
{
    public class BaseElementMassState : ActiveState
    {
        public ElementMass elementMass;
        public ElementType elementType;

        public void Init(ElementMass elementMass, ElementType elementType)
        {
            this.elementMass = elementMass;
            this.elementType = elementType;
            
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (caster == null && Machine != null && !Machine.Destroyed)
            {
                Machine.SetState(ActiveStateCatalog.InstantiateState(elementType.MassLetGoState));
            }
        }

        protected void AddMass(Vector3 positionShift)
        {
            ElementSubMass firstMass = elementMass.SubMasses[0];
            ElementSubMass newMass;
            elementMass.SubMasses.Add(newMass =
                UnityEngine.Object.Instantiate(
                    firstMass,
                    firstMass.transform.position + positionShift,
                    Quaternion.identity,// Quaternion.LookRotation(inputBank.AimDirection, Vector3.up),
                    firstMass.transform.parent));
            newMass.Offset = positionShift;
            elementMass.Grow(0);
        }
    }
}