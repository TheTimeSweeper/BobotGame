using ActiveStates.Characters;
using SpellCasting;
using SpellCasting.Projectiles;
using UnityEngine;

namespace ActiveStates.Heroes.Mars
{
    public class Mars10ComboWithStab : MarsComboBase
    {
        protected override void ModifyHit2()
        {
            return;
        }
        public override void ModifyNextState(ref ActiveState state)
        {
            if (hits == 1)
            {
                state = new MarsStab();
            }
            base.ModifyNextState(ref state);
        }
    }

    public class MarsStab : BasicMeleeAttack
    {
        protected override string hitboxName => "StabHitbox";

        protected override float damageCoefficient => 2;

        protected override float baseDuration => 1.6f;
        protected override float baseCastStartTimeFraction => 8f / 30f / 1.6f;
        protected override float baseCastEndTimeFraction => 14f / 30f / 1.6f;
        protected override float baseOtherStateInterruptTimeFraction => 0.5f;
        protected override float baseMovementInterruptTimeFraction => 1;

        public override void OnEnter()
        {
            base.OnEnter();
            animator.SetFloat("stab.playbackRate", 1);
            animator.Play("Stab");
            animator.Update(0f);
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            Vector3 scale = new Vector3(0.4736159f, 1, 1.95f);
            EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, transform.position, Util.DirectionQuaternion(inputBank.AimOut), scale, characterModel.CharacterDirection.transform);

            scale = new Vector3(-0.4736159f, 1, 1.95f);
            EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, transform.position, Util.DirectionQuaternion(inputBank.AimOut), scale, characterModel.CharacterDirection.transform);
        }
    }

    public class Mars11ComboWithProjectile : MarsComboBase
    {
        protected override void ModifyHit2()
        {
            return;
        }
        public override void ModifyNextState(ref ActiveState state)
        {
            if (hits == 1)
            {
                state = new MarsStabProjectile();
            }
            base.ModifyNextState(ref state);
        }
    }

    public class MarsStabProjectile : MarsStab
    {
        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            ProjectileController projectile = Object.Instantiate(GetComponent<Fuck>().projectilePrefab, transform.position, Util.DirectionQuaternion(inputBank.AimOut));
            projectile.Init(new FireProjectileData
            {
                AimDirection = inputBank.AimOut * 60,
                Damage = 20,
                OwnerBody = characterBody,
                OwnerObject = gameObject,
                StartPosition = transform.position,
                TeamIndex = teamComponent.TeamIndex
            });
        }
    }

    public class Mars12ComboWithVolley : MarsComboBase
    {
        protected override void ModifyHit2()
        {
            return;
        }
        public override void ModifyNextState(ref ActiveState state)
        {
            if (hits == 1)
            {
                state = new MarsStabVolley();
            }
            base.ModifyNextState(ref state);
        }
    }

    public class MarsStabVolley : MarsStab
    {
        public int hits;

        protected override float baseDuration => 0.6f;

        public override void OnEnter()
        {
            base.OnEnter();
            animator.SetFloat("stab.playbackRate", 1.6f/0.6f);
            animator.Play("Stab");
            animator.Update(0f);
        }

        protected override void OnCastExit()
        {
            base.OnCastExit();

            hits++;
            if (hits < 3)
            {
                Machine.SetState(new MarsStabVolley { hits = hits });
            }
        }
    }
}
