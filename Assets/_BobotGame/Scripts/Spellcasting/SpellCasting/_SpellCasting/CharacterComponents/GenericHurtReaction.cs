using ActiveStates;
using ActiveStates.Characters;
using UnityEngine;

namespace SpellCasting
{
    [RequireComponent(typeof(CharacterBody))]
    public class GenericHurtReaction : MonoBehaviour, IHasCommonComponents
    {
        [SerializeField]
        private CommonComponentsHolder commonComponents;
        public CommonComponentsHolder CommonComponents => commonComponents;

        [SerializeField]
        private float knockbackRespectThreshold;
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
                                                  //todo bobot a separate kind of knockback that overrides main velocity rather than this, which is nice for littler knockback
                commonComponents.FixedMotorDriver.AddedMotion2 = _knockback;
                _knockback = Util.ExpDecayLerp(_knockback, Vector3.zero, 6, Time.deltaTime);
            }
        }

        private void HealthComponent_OnDamageTaken(GetDamagedData damagedInfo)
        {
            if (damagedInfo.DamagingInfo.Knockback.sqrMagnitude >= (knockbackRespectThreshold * knockbackRespectThreshold)
                || damagedInfo.DamagingInfo.DamageValue >= hitStunThreshold)
            {
                SetHitstunState(damagedInfo, true);
            }
            else if (damagedInfo.DamagingInfo.DamageValue >= hitMoveThreshold)
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
                             //todo bobot fix magic number for knockback time
            _knockbackTime = 1 * knockbackFactor;
            _knockback = damagedInfo.DamagingInfo.Knockback * knockbackFactor;
        }
    }
}