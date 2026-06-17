using UnityEngine;

namespace SpellCasting.Projectiles
{
    [RequireComponent(typeof(ProjectileController))]
    public class ProjectileDisableCollisionsWithOwner : MonoBehaviour, IProjectileSubComponent, IProjectileDormant
    {
        public FireProjectileData ProjectileData { get; set; }

        [SerializeField]
        private Collider[] colliders;

        void Awake()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
        }

        public void ProjectileWake()
        {
            HurtBox[] hurtBoxes = ProjectileData.OwnerBody.CommonComponents.HurtBoxLocator.GetAll();

            for (int i = 0; i < colliders.Length; i++)
            {
                for (int h = 0; h < hurtBoxes.Length; h++)
                {
                    Physics.IgnoreCollision(hurtBoxes[h].collider, colliders[i]);
                }
                colliders[i].enabled = true;
            }
        }
    }
}