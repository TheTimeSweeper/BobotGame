using SpellCasting;
using UnityEngine;

namespace ActiveStates.Characters
{
    public class ConstantOverlap : ActiveState
    {
        protected virtual string hitboxName => "BasicHitbox";
        protected virtual float damageCoefficient => 1;

        private OverlapAttack _attack;
        private float _hitTim;
        private bool _returnedState;

        public override void OnEnter()
        {
            base.OnEnter();

            _attack = new OverlapAttack
            {
                Damage = damageCoefficient * characterBody.stats.Damage,
                Hitbox = characterModel.HitboxLocator.LocateByName(hitboxName),
                OwnerGameObject = gameObject,
                OwnerBody = characterBody,
                Team = teamComponent.TeamIndex,
                OverrideKnockbackDirection = characterModel.transform.forward,
                KnockbackForce = 0.4f
            };
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if(_hitTim > 0)
            {
                //bootleg ai when I just spent so long fuckin making AI work
                _hitTim -= Time.fixedDeltaTime;
                fixedMotorDriver.DesiredSpeed = 0;
                _attack.ResetHits();
                return;
            }
            if (!_returnedState)
            {
                _returnedState = true;
                stateMachineLocator.MainStateMachine.SetStateToDefault();
            }

            if (_attack.Fire())
            {
                _hitTim = 1;
                stateMachineLocator.MainStateMachine.SetState(new IdleState());
                _returnedState = false;
            }
        }
    }
}
