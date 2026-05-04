using UnityEngine;

namespace SpellCasting
{

    public class BoxHitbox : Hitbox
    {

        public override Collider[] DoOverlap()
        {
            DebugShow();
            if (transform.lossyScale.x < 0 || transform.lossyScale.y < 0 || transform.lossyScale.z < 0)
            {
                Debug.LogError("Error in Overlapbox. Hitbox scale is negative", this);
            }
            return Physics.OverlapBox(transform.position, transform.lossyScale * 0.5f, transform.rotation, LayerInfo.Hurtbox.layerMask);
        }
    }
}