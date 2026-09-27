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
        private float controlledAcceleration = 30;

        [SerializeField]
        private float idleDeceleration = 100;

        [SerializeField]
        private float physicsDeceleration = 10;

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
        public Vector3? OverrideVelocity { get; set; }//todo bobot move this to MotorEngine?
        public Vector3 AppliedVelocity { get; set; }//todo bobot move this to MotorEngine?
        public Vector3 FinalVelocity { get; protected set; }

        private Vector3 _deltaPosition;
        private Vector3 _currentControlledVelocity;
        private Vector3 _currentPhysicsVelocity;

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


            //oh no I'm doing exactly what ror2 is doing
            //jam faster deceleration value for fuckin uhh stopping and moving opposite direction
            float lerpValue = DesiredDirection == Vector3.zero ? idleDeceleration : controlledAcceleration;
            //oh no I'm gonna need ror2's acceleration adjustment as well.
            _currentControlledVelocity = Util.ExpDecayLerp(_currentControlledVelocity, DesiredVelocity, lerpValue, Time.fixedDeltaTime);


            _currentPhysicsVelocity = Util.ExpDecayLerp(_currentPhysicsVelocity, Vector3.zero, physicsDeceleration, Time.fixedDeltaTime);

            if (AppliedVelocity != Vector3.zero)
            {
                _currentPhysicsVelocity += AppliedVelocity;
                AppliedVelocity = Vector3.zero;
            }

            //todo bobot: completely separate is a bit silly on the list it goes
            Vector3 resultingMovement = _currentControlledVelocity + _currentPhysicsVelocity;

            if (OverrideVelocity.HasValue)
            {
                resultingMovement = OverrideVelocity.Value;
                OverrideVelocity = null;
            }

            if (AddedMotion != Vector3.zero)
            {
                resultingMovement += AddedMotion;
                AddedMotion = Vector3.zero;
            }

            //jam list of additions?
            if (AddedMotion2 != Vector3.zero)
            {
                resultingMovement += AddedMotion2;
                AddedMotion2 = Vector3.zero;
            }

            FinalVelocity = resultingMovement;

            engine.FixedMove(resultingMovement);

            _lastPosition = transform.position;
        }
    }
}
