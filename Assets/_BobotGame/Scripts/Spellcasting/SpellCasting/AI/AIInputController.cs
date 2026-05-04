using UnityEngine;
using UnityEngine.Windows;

namespace SpellCasting
{
    public class AIInputController : InputController
    {
        public bool[] downInputs = new bool[4];
        public int[] inputDownFrames = new int[4];

        public Vector3 MoveDirection { get; set; }

        private Vector3 _currentFixedAimPosition;
        public Vector3 CurrentAimPosition { get => _currentFixedAimPosition;
            set {
                _lastFixedAimPosition = _currentFixedAimPosition;
                _currentFixedAimPosition = value;
            } 
        }
        private Vector3 _lastFixedAimPosition;

        private Vector3 _overrideGesturePosition;
        public Vector3 OverrideGesturePosition
        {
            get => _overrideGesturePosition;
            set
            {
                _lastOverrideGesturePosition = _overrideGesturePosition;
                _overrideGesturePosition = value;
            }
        }
        private Vector3 _lastOverrideGesturePosition;

        private float _lastFixedTime;

        [SerializeField]
        private float gestureSpeedMultiplier;

        private void FixedUpdate()
        {
            for (int i = 0; i < inputDownFrames[i]; i++)
            {
                inputDownFrames[i]--;
                if(inputDownFrames[i] == 0)
                {
                    downInputs[i] = false;
                }
            }
            _lastFixedTime = Time.time;
        }

        public void JustPress(int input)
        {
            downInputs[input] = true;
            inputDownFrames[input] = 3;
        }

        protected override Vector3 GetAimPosition()
        {
            //smooth between fixedupdates
            float timeSinceLastDeltaTIme = Time.time - _lastFixedTime;

            Vector3 lerpedPosition = Vector3.Lerp(_lastFixedAimPosition, CurrentAimPosition, timeSinceLastDeltaTIme / Time.fixedDeltaTime);

            return lerpedPosition;
        }

        protected override Vector3 GetGesturePosition()
        {
            Vector3 gesture;
            if(OverrideGesturePosition != Vector3.zero)
            {
                float timeSinceLastDeltaTIme = Time.time - _lastFixedTime;

                Vector3 lerpedPosition = Vector3.Lerp(_lastOverrideGesturePosition, OverrideGesturePosition, timeSinceLastDeltaTIme / Time.fixedDeltaTime);

                gesture = lerpedPosition;
            } 
            else
            {
                gesture = transform.TransformPoint(GetAimPosition());
            }

            return new Vector3(gesture.x, gesture.z, 0) * gestureSpeedMultiplier;
        }

        protected override Vector3 GetMovementInput()
        {
            return transform.InverseTransformDirection(MoveDirection);
        }

        protected override void SetbuttonInputs()
        {
            inputBank.M1.UpdateInput(downInputs[0]);
            inputBank.M2.UpdateInput(downInputs[1]);
            inputBank.Shift.UpdateInput(downInputs[2]);
            inputBank.Space.UpdateInput(downInputs[3]);
        }
    }
}