using Unity.VisualScripting;
using UnityEngine;

namespace SpellCasting
{
    public class FixedMotorDriver : MonoBehaviour
    {
        [SerializeField]
        public MotorEngine engine;

        [SerializeField]
        private float controlledAcceleration= 100;

        [SerializeField]
        private float idleInertia= 10;

        public Vector3 Direction { get; set; }
        public float DesiredSpeed { get; set; }
        public Vector3 AddedMotion { get; set; }
        public Vector3 AddedMotion2 { get; set; }
        public Vector3? OverrideVelocity { get; set; }
        public Vector3 FinalVeolcity { get; protected set; }

        private Vector3 _deltaPosition;
        private Vector3 _currentVelocity;
        private Vector3 _desiredVelocity;

        private Vector3 _lastPosition;

        void FixedUpdate()
        {
            _deltaPosition = transform.position - _lastPosition;
            if (Direction.sqrMagnitude > 1) Direction = Direction.normalized;
            _desiredVelocity = Direction * DesiredSpeed;
            float lerpValue = Direction == Vector3.zero ? idleInertia : controlledAcceleration;
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

            FinalVeolcity = movement;

            engine.FixedMove(movement);

            _lastPosition = transform.position;
        }
    }
}
