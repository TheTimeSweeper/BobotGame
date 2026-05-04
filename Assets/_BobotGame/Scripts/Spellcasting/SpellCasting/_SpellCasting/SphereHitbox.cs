using UnityEngine;

namespace SpellCasting
{
    public class SphereHitbox : Hitbox
    {
        public override Collider[] DoOverlap()
        {
            DebugShow();

            return Physics.OverlapSphere(transform.position, transform.lossyScale.x * 0.5f, LayerInfo.Hurtbox.layerMask);
        }
    }
}