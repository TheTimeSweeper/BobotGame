using SpellCasting;
using UnityEngine;

namespace ActiveStates.Elements.Enemy
{
    //public class SimpleFireball :
}

namespace ActiveStates.Elements
{
    public class ElementThrowProjectile : BaseElementMassState 
    {
        public override void OnEnter()
        {
            base.OnEnter();

            if(animator != null)
            {
                //animator.Update(0f);
                animator.PlayInFixedTime("Cast", -1, 0);
            }

            for (int i = 0; i < elementMass.SubMasses.Count; i++)
            {
                Vector3 originalPosition = elementMass.SubMasses[i].transform.position;
                Vector3 relativePOsition = originalPosition - elementMass.CenterPosition;

                elementMass.SubMasses[i].JAMActivateProjectile(characterBody,
                    /*(elementMass.CenterPositionRaw - elementMass.CenterPosition).normalized*/inputBank.AimDirection * elementType.MassVelocityMultiplier,
                    GetPositionInlineWithDirectionPerpendicularly() + relativePOsition,
                    originalPosition,
                    GetDamageType());
            }

            elementMass.Casted = true;
        }

        protected virtual DamageTypeIndex GetDamageType()
        {
            return DamageTypeIndex.NONE;
        }

        protected virtual Vector3 GetPositionInlineWithDirectionPerpendicularly()
        {
            return elementMass.CenterPosition; // pack it up boys we failed
            return elementMass.CenterPositionRaw - inputBank.AimDirection * inputBank.GestureDistance * 0.1f;
            //Vector3 CMinusR = elementMass.CenterPosition - elementMass.CenterPositionRaw;

            //float theta = Vector3.Angle(CMinusR, inputBank.AimDirection);

            //float distanceToFinal = Mathf.Cos(theta) * CMinusR.magnitude;

            //return (distanceToFinal * -inputBank.AimDirection) + elementMass.CenterPositionRaw;
        }
    }
}