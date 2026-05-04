using UnityEngine;
using UnityEngine.Events;

namespace SpellCasting.Projectiles
{
    public class ProjectileOverlapAttack : ProjectileOverlap, IProjectileDormant
    {
        [SerializeField]
        private float damageCoefficient = 1;

        [SerializeField]
        private Rigidbody knockbackDirectionRigidbody;
        [SerializeField]
        private float KnockbackForce = 0.69f;

        [SerializeField]
        private float resetInterval = -1;

        [SerializeField]
        private bool destroyOnImpact;
        [SerializeField]
        private float destroyOnImpactGraceTime = 0.2f;

        [SerializeField]
        private UnityEvent impactEvent;

        [SerializeField]
        private int impactSound = -1;

        private float _destoryOnImpactTim;
        private float _repeatTim;

        private bool _impacted;

        public void ProjectileWake()
        {
            overlapAttack = new OverlapAttack
            {
                Hitbox = hitbox,
                Damage = ProjectileData.Damage * damageCoefficient,
                DamageType = ProjectileData.DamageType,
                OwnerGameObject = ProjectileData.OwnerObject,
                OwnerBody = ProjectileData.OwnerBody,
                Team = ProjectileData.TeamIndex,
                OverrideKnockbackDirection = knockbackDirectionRigidbody ? knockbackDirectionRigidbody.linearVelocity.normalized : Vector3.zero,
                KnockbackCenter = transform.position,
                KnockbackForce = KnockbackForce
            };
            _repeatTim = resetInterval;
        }

        void FixedUpdate()
        {
            if (resetInterval > 0)
            {
                _repeatTim -= Time.fixedDeltaTime;
                if (_repeatTim < 0)
                {
                    _repeatTim += resetInterval;

                    overlapAttack.ResetHits();
                }
            }

            bool hit = overlapAttack.Fire();
            if (!_impacted)
            {
                if (hit)
                {
                    _impacted = true;
                    impactEvent.Invoke();
                    if(impactSound > 0)
                    {
                        EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, impactSound);
                    }
                }
            }
            else if (destroyOnImpact)
            {
                _destoryOnImpactTim += Time.fixedDeltaTime;
                if (_destoryOnImpactTim > destroyOnImpactGraceTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}