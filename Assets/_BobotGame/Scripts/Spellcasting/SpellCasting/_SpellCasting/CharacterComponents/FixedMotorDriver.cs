using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SpellCasting
{
    public class FixedMotorDriver : MonoBehaviour/*, IHasCommonComponents*/
    {
        [SerializeField]
        public MotorEngine engine;

        [SerializeField]
        private float controlledAcceleration= 100;

        [SerializeField]
        private float idleInertia = 10;

        //[Header("Knockback")]
        //[SerializeField]
        //private CommonComponentsHolder optionalCommonComponents;
        //public CommonComponentsHolder CommonComponents => optionalCommonComponents;

        public Vector3 DesiredVelocity
        {
            get => DesiredDirection.normalized * DesiredSpeed;
            set
            {
                DesiredDirection = value.normalized;
                DesiredSpeed = value.magnitude;
            }
        }
        
        public Vector3 DesiredDirection { get; set; }
        public float DesiredSpeed { get; set; }
        public Vector3 AddedMotion { get; set; }
        public Vector3 AddedMotion2 { get; set; }
        public Vector3? OverrideVelocity { get; set; }
        public Vector3 FinalVelocity { get; protected set; }

        private Vector3 _deltaPosition;
        private Vector3 _currentVelocity;
        private Vector3 _desiredVelocity;

        private Vector3 _lastPosition;

        //private bool _subscribedToTakeDamage;

        //private void Awake()
        //{
        //    if (optionalCommonComponents && optionalCommonComponents.HealthComponent)
        //    {
        //        SubscribeToTakeDamage(true);
        //    }
        //}

        //void SubscribeToTakeDamage(bool shouldSubscribe)
        //{
        //    if(shouldSubscribe == _subscribedToTakeDamage)
        //    {
        //        return;
        //    }
        //    _subscribedToTakeDamage = shouldSubscribe;

        //    if(optionalCommonComponents && optionalCommonComponents.HealthComponent)
        //    {
        //        if (shouldSubscribe)
        //        {
        //            optionalCommonComponents.HealthComponent.OnDamageTaken += OnTakeDamage_KnockBack;
        //        } 
        //        else
        //        {
        //            optionalCommonComponents.HealthComponent.OnDamageTaken -= OnTakeDamage_KnockBack;
        //        }
        //    }
        //}

        //private void OnTakeDamage_KnockBack(GetDamagedData getDamagedInfo)
        //{
        //    //set stunned state
        //    //apply velocity
        //    //wait I already did this
        //}

        void FixedUpdate()
        {
            _deltaPosition = transform.position - _lastPosition;
            if (DesiredDirection.sqrMagnitude > 1) DesiredDirection = DesiredDirection.normalized;
            _desiredVelocity = DesiredDirection * DesiredSpeed;
            float lerpValue = DesiredDirection == Vector3.zero ? idleInertia : controlledAcceleration;
            _currentVelocity = Util.ExpDecayLerp(_currentVelocity, _desiredVelocity, lerpValue, Time.fixedDeltaTime);

            //jam faster deceleration value for fuckin uhh stopping and moving opposite direction

            Vector3 movement = _currentVelocity;

            if (OverrideVelocity.HasValue)
            {
                movement = OverrideVelocity.Value;
                OverrideVelocity = null;
            }

            if (AddedMotion != Vector3.zero)
            {
                movement += AddedMotion;
                AddedMotion = Vector3.zero;
            }

            //jam list of additions?
            if (AddedMotion2 != Vector3.zero)
            {
                movement += AddedMotion2;
                AddedMotion2 = Vector3.zero;
            }

            FinalVelocity = movement;

            engine.FixedMove(movement);

            _lastPosition = transform.position;
        }
    }
}
