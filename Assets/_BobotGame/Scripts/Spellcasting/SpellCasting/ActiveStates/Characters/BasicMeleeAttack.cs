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
        protected virtual string effectOriginName { get; }
        protected abstract float damageCoefficient { get; }
        protected virtual float preAttackMoveShift => 0.5f;
        protected virtual float preAttackMoveShiftDecay => 16;
        protected virtual float attackMoveShift => 0.5f;
        protected virtual float attackMoveShiftDecay => 16;

        protected OverlapAttack attack;

        private Vector3 _preAttackShift;
        private Vector3 _attackShift;

        public Vector3 aimDirection;

        private Vector3 _startDirection;
        public override void OnEnter()
        {
            base.OnEnter();

            _preAttackShift = inputBank.GlobalMoveDirection * preAttackMoveShift * characterBody.stats.MoveSpeed;

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

            _startDirection = inputBank.AimOut;
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            _attackShift = inputBank.GlobalMoveDirection * attackMoveShift * characterBody.stats.MoveSpeed;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (!hasCasted)
            {
                fixedMotorDriver.AddedMotion2 = _preAttackShift;
                _preAttackShift = Util.ExpDecayLerp(_preAttackShift, Vector3.zero, preAttackMoveShiftDecay, Time.deltaTime);
            }
        }

        protected override void OnCastFixedUpdate()
        {
            base.OnCastFixedUpdate();

            characterModel.CharacterDirection.OverrideLookDirection(_startDirection, 0.1f);

            fixedMotorDriver.AddedMotion = _attackShift;
            _attackShift = Util.ExpDecayLerp(_attackShift, Vector3.zero, attackMoveShiftDecay, Time.deltaTime);

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
