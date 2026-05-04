using ActiveStates;
using ActiveStates.Characters;
using UnityEngine;

namespace SpellCasting
{
    [RequireComponent(typeof(CharacterBody))]
    public class GenericHurtReaction : MonoBehaviour
    {
        [SerializeField]
        private CommonComponentsHolder commonComponents;

        [SerializeField]
        private float hitStunThreshold;
        [SerializeField]
        private float hitMoveThreshold;

        private float _knockbackTime;
        private Vector3 _knockback;

        private float stunFactor
        {
            get
            {
                if(commonComponents?.CharacterBody!= null)
                {
                    return commonComponents.CharacterBody.stats.StunFactor;
                }
                return 1;
            }
        }

        private float knockbackFactor
        {
            get
            {
                if (commonComponents?.CharacterBody != null)
                {
                    return commonComponents.CharacterBody.stats.KnockbackFactor;
                }
                return 1;
            }
        }

        private void Reset()
        {
            commonComponents = GetComponent<CommonComponentsHolder>();
        }

        private void Awake()
        {
            commonComponents.HealthComponent.OnDamageTaken += HealthComponent_OnDamageTaken;
        }

        private void Update()
        {
            if (_knockbackTime > 0 && _knockback != Vector3.zero)
            {
                _knockbackTime -= Time.deltaTime;

                commonComponents.FixedMotorDriver.AddedMotion2 = _knockback;
                _knockback = Util.ExpDecayLerp(_knockback, Vector3.zero, 6, Time.fixedDeltaTime);
            }
        }

        private void HealthComponent_OnDamageTaken(GetDamagedData damagedInfo)
        {
            if (damagedInfo.DamagingInfo.DamageValue > hitStunThreshold)
            {
                SetHitstunState(damagedInfo, true);
            }
            else if (damagedInfo.DamagingInfo.DamageValue > hitMoveThreshold)
            {
                SetHitstunState(damagedInfo, false);
            }
        }

        public void SetHitstunState(GetDamagedData damagedInfo, bool all)
        {
            if(all)
            {
                commonComponents.StateMachineLocator.SetStates(
                    new StunnedState
                    {
                        StunTime = stunFactor,
                    },
                    InterruptPriority.HITSTUN,
                    true,
                    all);
            }

            _knockbackTime = 0.5f * commonComponents.CharacterBody.stats.KnockbackFactor;
            _knockback = damagedInfo.DamagingInfo.Knockback * commonComponents.CharacterBody.stats.KnockbackFactor;
        }
    }
}