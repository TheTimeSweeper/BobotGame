using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class ProjectileSpawnChildren : MonoBehaviour, IProjectileSubComponent
    {
        [SerializeField]
        private ProjectileController childProjectile;

        [SerializeField]
        private float interval = 0.3f;

        float _tim;

        public FireProjectileData ProjectileData { get; set; }

        void FixedUpdate ()
        {
            _tim -= Time.deltaTime;

            if (_tim <= 0)
            {
                _tim = interval;

                ProjectileController chidlProjecilte = Object.Instantiate(childProjectile, transform.position, Quaternion.identity);
                chidlProjecilte.Init(new FireProjectileData
                {
                    OwnerObject = ProjectileData.OwnerObject,
                    OwnerBody = ProjectileData.OwnerBody,
                    Damage = ProjectileData.Damage,
                    StartPosition = transform.position,
                    TeamIndex = ProjectileData.TeamIndex,
                    DamageType = ProjectileData.DamageType
                });
            }
        }
    }
}