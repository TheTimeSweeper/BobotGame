using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class ProjectileGrapple : MonoBehaviour, IProjectileSubComponent, IProjectileInitialized
    {
        public FireProjectileData ProjectileData { get ; set ; }

        [SerializeField]
        private Rigidbody selfRigidBody;
        [SerializeField]
        private bool ignoreY;

        [SerializeField]
        private Transform ReelStretcher;
        [SerializeField]
        private string ReelPointVisualChildName;

        public float pullForce;

        private FixedMotorDriver targetMotor;
        private Transform pullPoint;
        private Transform reelPointVisualChild;

        public void ProjectileWake()
        {
            reelPointVisualChild = ProjectileData.OwnerBody.CommonComponents.CharacterModel.ChildLocator.LocateByName(ReelPointVisualChildName);
        }

        void LateUpdate()
        {
            if (!reelPointVisualChild || !ReelStretcher)
                return;
            var difference = reelPointVisualChild.position - transform.position;
            ReelStretcher.rotation = Util.DirectionQuaternion(difference);
            ReelStretcher.localScale = new Vector3(1, 1, difference.magnitude);
        }

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
                Vector3 difference = (pullPoint.transform.position - targetMotor.transform.position);
                if (ignoreY)
                {
                    difference.y = 0;
                }
                targetMotor.AddedMotion = difference.normalized * pullForce ;
            }
        }
    }
}