using SpellCasting;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

namespace ActiveStates.Characters
{
    public abstract class BasicMeleeAttack : GenericTimedState
    {
        [System.Serializable]
        public class BasicMeleeParams : TimedStateParams
        {
            [Header("Melee")]
            public string hitboxName;
            public string effectOriginName = "";
            public float damageCoefficient = 1;
            public float stunTime = 0;
            public float knockbackCoefficient = 0;
            public float preAttackMoveShift = 0.5f;
            public float preAttackMoveShiftDecay = 16;
            public float attackMoveShift = 0.5f;
            public float attackMoveShiftDecay = 16;
            public float staminaRecoveryOnHit = 10;

            public BasicMeleeParams() : base()
            {

            }
            public BasicMeleeParams(float baseDuration) : base(baseDuration)
            {
            }
        }

        protected BasicMeleeParams meleeParams => stateParams as BasicMeleeParams;

        protected OverlapAttack overlapAttack;

        private Vector3 _preAttackShift;
        private Vector3 _attackShift;

        public Vector3 aimDirection;

        private Vector3 _startDirection;
        public override void OnEnter()
        {
            base.OnEnter();

            _preAttackShift = inputBank.GlobalMoveDirection * meleeParams.preAttackMoveShift * characterBody.stats.MoveSpeed;

            if (aimDirection == Vector3.zero)
            {
                aimDirection = inputBank.AimOut;
                //aimDirection = inputBank.GlobalMoveDirection;
            }

            SetAimForward();

            overlapAttack = new OverlapAttack
            {
                Damage = meleeParams.damageCoefficient * characterBody.stats.Damage,
                Hitbox = characterModel.HitboxLocator.LocateByName(meleeParams.hitboxName),
                OwnerGameObject = gameObject,
                OwnerBody = characterBody,
                Team = teamComponent.TeamIndex,
                OverrideKnockbackDirection = characterModel.transform.forward,
                KnockbackForceCoefficient = meleeParams.knockbackCoefficient,
                StunTime = meleeParams.stunTime
            };
            ModifyOverlapAttack(overlapAttack);

            //EffectManager.SpawnEffect(EffectIndex.SOUND_FAST, transform.position, null, 11);


            //characterModel.CharacterDirection.OverrideLookDirection(aimDirection, duration);

            _startDirection = inputBank.AimOut;
        }

        protected virtual void ModifyOverlapAttack(OverlapAttack overlapAttack)
        {

        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            _attackShift = inputBank.GlobalMoveDirection * meleeParams.attackMoveShift * characterBody.stats.MoveSpeed;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (!hasCasted)
            {
                fixedMotorDriver.AddedMotion2 = _preAttackShift;
                _preAttackShift = Util.ExpDecayLerp(_preAttackShift, Vector3.zero, meleeParams.preAttackMoveShiftDecay, Time.deltaTime);
            }
        }

        protected override void OnCastFixedUpdate()
        {
            base.OnCastFixedUpdate();

            characterModel.CharacterDirection.OverrideLookDirection(_startDirection, 0.1f);

            fixedMotorDriver.AddedMotion = _attackShift;
            _attackShift = Util.ExpDecayLerp(_attackShift, Vector3.zero, meleeParams.attackMoveShiftDecay, Time.deltaTime);

            if (overlapAttack.Fire())
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
