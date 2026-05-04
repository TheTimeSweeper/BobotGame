using SpellCasting;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

namespace ActiveStates.Characters
{
    public abstract class BasicMeleeAttack : BasicTimedState
    {
        protected abstract string hitboxName { get; }
        protected abstract float damageCoefficient { get; }
        protected virtual float positionShift => 0.5f;
        protected virtual float positionShiftDeceleration => 16;

        protected OverlapAttack attack;

        private Vector3 _shift;

        public Vector3 aimDirection;

        private Vector3 _startDirection;
        public override void OnEnter()
        {
            base.OnEnter();

            _shift = inputBank.GlobalMoveDirection * positionShift * characterBody.stats.MoveSpeed;

            if (aimDirection == Vector3.zero)
            {
                aimDirection = inputBank.AimOut;
                //aimDirection = inputBank.GlobalMoveDirection;
            }

            attack = new OverlapAttack
            {
                Damage = damageCoefficient * characterBody.stats.Damage,
                Hitbox = characterModel.HitboxLocator.LocateByName(hitboxName),
                OwnerGameObject = gameObject,
                OwnerBody = characterBody,
                Team = teamComponent.TeamIndex,
                //OverrideKnockbackDirection = characterModel.transform.forward,
                //KnockbackForce = 0.4f
            };

            //EffectManager.SpawnEffect(EffectIndex.SOUND_FAST, transform.position, null, 11);


            //characterModel.CharacterDirection.OverrideLookDirection(aimDirection, duration);

            //jam effectcatalog when
            //well I did it but this works so not touching it til i need to
            if (characterModel?.particleSystemLocator)
            {
                ParticleSystem swipeParticle = characterModel.particleSystemLocator.LocateByName("SwipeParticle");
                if (swipeParticle)
                {
                    swipeParticle.Play();
                }
            }
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            _startDirection = inputBank.AimOut;
        }

        protected override void OnCastFixedUpdate()
        {
            base.OnCastFixedUpdate();

            characterModel.CharacterDirection.OverrideLookDirection(_startDirection, 0.1f);

            fixedMotorDriver.AddedMotion = _shift;
            _shift = Util.ExpDecayLerp(_shift, Vector3.zero, 11, Time.fixedDeltaTime);

            if (attack.Fire())
            {
                OnHitEnemyAuthority();
            }
        }

        protected virtual void OnHitEnemyAuthority()
        {
            //Util.PlaySound(hitSoundString, gameObject);

            //if (!hasHopped)
            //{
            //    if (characterMotor && !characterMotor.isGrounded && hitHopVelocity > 0f)
            //    {
            //        SmallHop(characterMotor, hitHopVelocity);
            //    }

            //    hasHopped = true;
            //}

            //ApplyHitstop();
        }


    }
}
