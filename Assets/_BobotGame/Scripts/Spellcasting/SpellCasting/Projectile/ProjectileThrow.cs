using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class ProjectileThrow : MonoBehaviour, IProjectileSubComponent, IProjectileDormant
    {
        public FireProjectileData ProjectileData { get; set; }

        [SerializeField]
        private Rigidbody rigidBody;

        void Reset()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        public void ProjectileWake()
        {
            transform.position = ProjectileData.StartPosition;
            transform.rotation = Quaternion.LookRotation(ProjectileData.AimDirection, Vector3.up);
            rigidBody.isKinematic = false;
            rigidBody.linearVelocity = ProjectileData.AimDirection;
        }
    }
}