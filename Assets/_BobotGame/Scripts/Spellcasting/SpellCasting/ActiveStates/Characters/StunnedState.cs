using UnityEngine;

namespace ActiveStates.Characters
{
    public class StunnedState : BasicTimedStateSimple
    {
        public float StunTime;

        public GameObject EffectPrefab;
        private GameObject _effectObject;
        protected override float baseDuration => StunTime;

        public override void OnEnter()
        {
            base.OnEnter();

            if (EffectPrefab != null)
            {
                _effectObject = Object.Instantiate(EffectPrefab, transform.position, Quaternion.identity, transform);
            }
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            if (_effectObject != null)
            {
                Object.Destroy(_effectObject);
            }
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