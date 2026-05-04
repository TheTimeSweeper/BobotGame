using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class ProjectileOverlapKnockback : ProjectileOverlap, IProjectileDormant
    {
        [SerializeField]
        private float force = 10;

        [SerializeField]
        private float succCloseLimit = 3;

        [SerializeField]
        private TeamTargetType teamTargeting = TeamTargetType.OTHER;

        private Vector3 _center;

        public void ProjectileWake()
        {
            overlapAttack = new OverlapAttack
            {
                Hitbox = hitbox,
                OwnerGameObject = ProjectileData.OwnerObject,
                OwnerBody = ProjectileData.OwnerBody,
                Team = ProjectileData.TeamIndex,
                TeamTargeting = teamTargeting

            };
            _center = transform.position;
        }

        void FixedUpdate()
        {
            overlapAttack.Fire();
            for (int i = 0; i < overlapAttack.HitTargets.Count; i++)
            {
                CommonComponentsHolder commonComponents = overlapAttack.HitTargets[i].CommonComponents;
                if (commonComponents != null && commonComponents.FixedMotorDriver != null)
                {
                    Transform motorTransform = commonComponents.FixedMotorDriver.transform;
                    Vector3 pushVector = motorTransform.position - _center;
                    if (force < 0 && pushVector.magnitude < succCloseLimit)
                        continue;

                    commonComponents.FixedMotorDriver.AddedMotion = pushVector * force;
                    commonComponents.FixedMotorDriver.OverrideVelocity = Vector3.zero;
                }
            }
        }
    }
}