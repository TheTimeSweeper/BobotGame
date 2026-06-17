using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class ProjectileGrapple : MonoBehaviour, IProjectileSubComponent
    {
        public FireProjectileData ProjectileData { get ; set ; }

        [SerializeField]
        private Rigidbody selfRigidBody;

        public float pullForce;

        private FixedMotorDriver targetMotor;
        private Transform pullPoint;

        private void OnCollisionEnter(Collision collision)
        {
            transform.parent = collision.transform;
            selfRigidBody.isKinematic = true;

            if (collision.transform.TryGetComponent<HurtBox>(out var hurtbox))
            {
                targetMotor = hurtbox.HealthComponent.CommonComponents?.FixedMotorDriver;
            }

            if (targetMotor)
            {
                pullPoint = ProjectileData.OwnerBody.transform;
            }
            else
            {
                targetMotor = ProjectileData.OwnerBody.CommonComponents.FixedMotorDriver;
                pullPoint = transform;
            }
        }

        private void FixedUpdate()
        {
            if (targetMotor)
            {
                targetMotor.OverrideVelocity = (pullPoint.transform.position - targetMotor.transform.position).normalized * pullForce;
            }
        }
    }
}