using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class ProjectileAlignToDirection : MonoBehaviour, IProjectileSubComponent
    {
        public FireProjectileData ProjectileData { get; set; }

        void Start ()
        {
            transform.LookAt(transform.position + ProjectileData.AimDirection);
        }
    }
}