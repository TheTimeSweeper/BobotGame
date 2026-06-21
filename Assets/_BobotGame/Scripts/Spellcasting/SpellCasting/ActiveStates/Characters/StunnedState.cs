using UnityEngine;

namespace ActiveStates.Characters
{
    public class StunnedState : BodyState
    {
        public float StunTime;

        public GameObject EffectPrefab;
        private GameObject _effectObject;
        private float countDown;
        public override void OnEnter()
        {
            base.OnEnter();

            if (EffectPrefab != null)
            {
                _effectObject = Object.Instantiate(EffectPrefab, transform.position, Quaternion.identity, transform);
            }
            countDown = StunTime;

            animator.Play("Stunned");
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            countDown -= Time.deltaTime;

            if (genericHurtReaction)
            {
                countDown = genericHurtReaction.CurrentStunTime;
            }

            if (countDown <= 0)
            {
                EndState();
            }
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            if (_effectObject != null)
            {
                Object.Destroy(_effectObject);
            }
            animator.SetTrigger("cancel");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.STUN;
        }
        public override ActiveState Clone()
        {
            return new StunnedState { StunTime = StunTime, EffectPrefab = EffectPrefab };
        }
    }
}