using ActiveStates;
using ActiveStates.Characters;
using System;
using UnityEngine;

namespace SpellCasting
{
    [RequireComponent(typeof(CharacterBody))]
    public class GenericHurtReaction : MonoBehaviour, IHasCommonComponents, IUIBarProvider
    {
        [SerializeField]
        private CommonComponentsHolder commonComponents;
        public CommonComponentsHolder CommonComponents => commonComponents;

        //[SerializeField]
        //private float knockbackRespectThreshold;
        //[SerializeField]
        //private float hitStunThreshold;
        //[SerializeField]
        //private float hitMoveThreshold;

        private float _lastStunTimer;
        private float _currentStunTimer;
        private float _totalTimeStunned;
        public float CurrentStunTime => _currentStunTimer;
        //public float stunDecayTime = 1;
        [Tooltip("The amount of stun time gathered before the curve x value should reach 1. I cannot think of a better name or maybe setup for this.")]
        public float stunResistanceMaxTime = 1;
        public AnimationCurve stunResistanceCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private float _knockbackTime;
        private Vector3 _knockback;

        private float bodyStunFactor
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

        private void FixedUpdate()
        {
            if (_knockbackTime > 0)
            {
                _knockbackTime -= Time.deltaTime;
                //todo bobot a separate kind of knockback that overrides main velocity rather than this, which is nice for littler knockback
                commonComponents.FixedMotorDriver.AddedMotion2 = _knockback;
                _knockback = Util.ExpDecayLerp(_knockback, Vector3.zero, 6, Time.deltaTime);
            }
            else
            {
                _knockback = Vector3.zero;
            }
            _lastStunTimer = _currentStunTimer;
            _currentStunTimer = Mathf.Max(0, _currentStunTimer - Time.deltaTime);
            if(_currentStunTimer > 0)
            {
                _totalTimeStunned += Time.deltaTime;
            }
            else
            {
                _totalTimeStunned = 0;
            }
        }
        private void HealthComponent_OnDamageTaken(GetDamagedData damagedInfo)
        {
            //if (damagedInfo.DamagingInfo.Knockback.sqrMagnitude >= (knockbackRespectThreshold * knockbackRespectThreshold)
            //    || damagedInfo.DamagingInfo.DamageValue >= hitStunThreshold)
            //{
            //    SetHitstunState(damagedInfo, true);
            //}
            //else if (damagedInfo.DamagingInfo.DamageValue >= hitMoveThreshold)
            //{
            //    SetHitstunState(damagedInfo, false);
            //}

            if (damagedInfo.DamagingInfo.wasBlocked)
            {
                return;
            }

            float stunAdded = AddNewStunValue(damagedInfo.DamagingInfo.StunTime * bodyStunFactor);

            if(_lastStunTimer <= 0 && _currentStunTimer > 0)
            {
                SetHitstunState(damagedInfo, _currentStunTimer, true);
            }

            //todo bobot fix magic number for knockback time
            _knockbackTime = stunAdded * knockbackFactor;
            _knockback += damagedInfo.DamagingInfo.Knockback * knockbackFactor;
        }

        private float AddNewStunValue(float stunValueToAdd)
        {
            float currentStunResistance = stunResistanceCurve.Evaluate(_totalTimeStunned / stunResistanceMaxTime);
            float finalStunToAdd = (1 - currentStunResistance) * stunValueToAdd;
            _currentStunTimer += finalStunToAdd;
            Debug.Log($"finalStunToAdd {finalStunToAdd.ToString("0.00")}. resistance {currentStunResistance.ToString("0.00")}. total Stun {_totalTimeStunned.ToString("0.00")}. ");
            return finalStunToAdd;
        }

        public void SetHitstunState(GetDamagedData damagedInfo, float newStunTime, bool all)
        {
            commonComponents.StateMachineLocator.SetStates(
                new StunnedState
                {
                    StunTime = newStunTime,
                },
                InterruptPriority.HITSTUN,
                true,
                all);
        }

        public float GetUICurrentValue()
        {
            return _currentStunTimer;
        }

        public float GetUIMaxValue()
        {
            return stunResistanceMaxTime;
        }

        public bool GetUIShouldShow()
        {
            return _currentStunTimer > 0;
        }
    }
}