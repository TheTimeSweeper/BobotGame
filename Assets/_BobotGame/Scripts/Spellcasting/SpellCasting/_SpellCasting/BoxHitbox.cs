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
            return Physics.OverlapBox(transform.position, transform.lossyScale * 0.5f, transform.rotation, LayerInfo.CommonMasks.Hittable);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = new Color(1, 0, 0, 0.1f);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);

            Gizmos.color = new Color(1, 0, 0, 0.6f);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}