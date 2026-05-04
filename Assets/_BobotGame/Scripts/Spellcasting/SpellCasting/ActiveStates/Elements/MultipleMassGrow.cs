using UnityEngine;

namespace ActiveStates.Elements
{
    public class MultipleMassGrow : BasicElementGrow
    {
        protected virtual int amount => 3;

        public override void OnEnter()
        {
            base.OnEnter();

            Vector3 offset = Vector3.forward * elementType.MassOffsetDistance;

            elementMass.SubMasses[0].Offset = offset;

            for (int i = 0; i < amount - 1; i++)
            {
                offset = Quaternion.Euler(0, 360f / amount, 0) * offset;
                AddMass(offset);
            }
        }
    }
}
