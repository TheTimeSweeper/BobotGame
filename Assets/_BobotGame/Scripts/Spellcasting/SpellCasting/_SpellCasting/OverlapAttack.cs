using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SpellCasting
{
    public class OverlapAttack
    {
        public GameObject OwnerGameObject { get; set; }
        public CharacterBody OwnerBody { get; set; }
        public Hitbox Hitbox { get; set; }
        public float Damage { get; set; }
        public DamageTypeIndex DamageType { get; set; }
        public TeamIndex Team { get; set; }
        public TeamTargetType TeamTargeting { get; set; } = TeamTargetType.OTHER;
        public DamagingData DamageInfo { get; set; }
        public Vector3 OverrideKnockbackDirection { get; set; }
        public Vector3 KnockbackCenter { get;  set; }
        public float KnockbackForce { get; set; }

        private List<HealthComponent> _alreadyHitTargets = new List<HealthComponent>();
        public List<HealthComponent> HitTargets => _alreadyHitTargets;

        public List<HurtBox> _hitResults;

        public bool Fire() => Fire(out _, false);
        public bool Fire(out List<HurtBox> hitResults) => Fire(out hitResults, true);
        public bool Fire(out List<HurtBox> hitResults, bool returnResults)
        {
            bool hit = false;

            if (returnResults)
            {
                if (_hitResults == null)
                {
                    _hitResults = new List<HurtBox>();
                }
                _hitResults.Clear();
                hitResults = _hitResults;
            }
            else
            {
                hitResults = null;
            }

            Collider[] colliders = Hitbox.DoOverlap();

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];
                HurtBox hurtbox = collider.GetComponent<HurtBox>();
                if (hurtbox != null)
                {
                    HealthComponent healthComponent = hurtbox.HealthComponent;
                    if (healthComponent == null)
                        continue;

                    if (_alreadyHitTargets.Contains(healthComponent))
                        continue;

                    //if team is undefined, just hit everything
                    if (Team != default)
                    {

                        GameObject thisGameObject = OwnerGameObject;
                        IHasCommonComponents targetObject = healthComponent;
                        TeamTargetType teamTargeting = TeamTargeting;
                        bool validHit = Util.ShouldTargetByTeam(thisGameObject, targetObject, Team, teamTargeting);
                        if (!validHit)
                        {
                            continue;
                        }
                    }

                    if (returnResults)
                    {
                        hitResults.Add(hurtbox);
                    }

                    _alreadyHitTargets.Add(healthComponent);
                    hit = true;

                    if (DamageInfo == null)
                    {
                        DamageInfo = new DamagingData
                        {
                            AttackerObject = OwnerGameObject,
                            AttackerBody = OwnerBody,
                            DamageValue = Damage,
                            DamageTypeIndex = DamageType,
                        };
                    }

                    if (OverrideKnockbackDirection != Vector3.zero)
                    {
                        DamageInfo.Knockback = OverrideKnockbackDirection * KnockbackForce;
                    }
                    else
                    {
                        if (KnockbackForce > 0)
                        {
                            Vector3 knock = collider.transform.position - KnockbackCenter;
                            knock.y = 0;
                            DamageInfo.Knockback = knock.normalized * KnockbackForce;
                        }
                    }

                    healthComponent.TakeDamage(DamageInfo);
                    //jam all attacks od the same inmpact effect yep
                    if (Damage > 0)
                    {
                        EffectManager.SpawnEffect(EffectIndex.HITFLASH, collider.transform.position, Quaternion.identity, collider.transform);
                        //EffectManager.SpawnEffect(EffectIndex.SOUND_FAST, collider.transform.position, null, 10);
                    }
                }
            }

            return hit;
        }

        public void ResetHits()
        {
            _alreadyHitTargets.Clear();
        }
    }
}