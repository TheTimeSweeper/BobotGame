using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class ProjectileOverlapPusher : ProjectileOverlap, IProjectileDormant
    {
        [SerializeField]
        private Rigidbody rigidBody;

        [SerializeField]
        private TeamTargetType teamTargeting = TeamTargetType.OTHER;

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
        }

        void FixedUpdate()
        {
            overlapAttack.Fire();
            for (int i = 0; i < overlapAttack.HitTargets.Count; i++)
            {
                CommonComponentsHolder commonComponents = overlapAttack.HitTargets[i].CommonComponents;
                if (commonComponents != null && commonComponents.FixedMotorDriver != null)
                {
                    commonComponents.FixedMotorDriver.AddedMotion = rigidBody.linearVelocity * Time.fixedDeltaTime;
                    commonComponents.FixedMotorDriver.OverrideVelocity = Vector3.zero;
                }
            }
        }
    }
}