using UnityEngine;
using UnityEngine.Windows;

namespace SpellCasting
{
    public class AIInputController : InputController
    {
        public bool[] downInputs = new bool[4];
        public int[] inputDownFrames = new int[4];

        public float aimStrength= 20;

        public Vector3 MoveDirection { get; set; }

        private Vector3 _currentFixedAimPosition;
        public Vector3 CurrentAimPosition { get => _currentFixedAimPosition;
            set {
                _lastFixedAimPosition = _currentFixedAimPosition;//todo bobot, interrogate how this _lastfixed was supposed to work ohhhhhhh the lerp was fuckinnnnnn interpolation yeah I need to fix that for wizard game might just abandon it here
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


        [SerializeField]
        private float gestureSpeedMultiplier;

        private void FixedUpdate()
        {
            for (int i = 0; i < inputDownFrames[i]; i++)
            {
                if (inputDownFrames[i] < 0)
                {
                    continue;
                }
                inputDownFrames[i]--;
                if(inputDownFrames[i] == 0)
                {
                    downInputs[i] = false;
                }
            }
        }

        public void JustPress(int input)
        {
            downInputs[input] = true;
            inputDownFrames[input] = 3;
        }
        public void Hold(int input, int frames = -1)
        {
            downInputs[input] = true;
            inputDownFrames[input] = frames;
        }
        public void Toggle(int input)
        {
            downInputs[input] = !downInputs[input];
            inputDownFrames[input] = downInputs[input] ? -1 : 0;
        }

        protected override Vector3 GetAimPosition()
        {
            Vector3 lerpedPosition = Util.ExpDecayLerp(_lastFixedAimPosition, CurrentAimPosition, aimStrength, Time.deltaTime);
            Util.DebugDrawPoint(lerpedPosition, Color.magenta);
            return lerpedPosition;
        }

        protected override Vector3 GetGesturePosition()
        {
            Vector3 gesture = default;
            if(OverrideGesturePosition != Vector3.zero)
            {
                //todo bobot restore gestures if needed
                //float timeSinceLastDeltaTIme = Time.time - _lastFixedTime;

                //Vector3 lerpedPosition = Vector3.Lerp(_lastOverrideGesturePosition, OverrideGesturePosition, timeSinceLastDeltaTIme / Time.fixedDeltaTime);

                //gesture = lerpedPosition;
            } 
            else
            {
                //gesture = transform.TransformPoint(GetAimPosition());
            }

            return new Vector3(gesture.x, gesture.z, 0) * gestureSpeedMultiplier;
        }

        protected override Vector3 GetMovementInput()
        {
            return transform.InverseTransformDirection(MoveDirection);
        }

        protected override void SetbuttonInputs()
        {
            inputBank.Primary.UpdateInput(downInputs[0]);
            inputBank.Block.UpdateInput(downInputs[1]);
            inputBank.Shift.UpdateInput(downInputs[2]);
            inputBank.Space.UpdateInput(downInputs[3]);
        }
    }
}