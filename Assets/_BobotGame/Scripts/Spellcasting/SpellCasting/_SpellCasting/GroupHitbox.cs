using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class GroupHitbox : Hitbox
    {
        [SerializeField]
        private Hitbox[] SubHitboxes;

        public override Collider[] DoOverlap()
        {
            DebugShow();


            List<Collider> colliders = new List<Collider>();

            for (int i = 0; i < SubHitboxes.Length; i++)
            {
                colliders.AddRange(SubHitboxes[i].DoOverlap());
            }
            return colliders.ToArray();
        }
    }
}