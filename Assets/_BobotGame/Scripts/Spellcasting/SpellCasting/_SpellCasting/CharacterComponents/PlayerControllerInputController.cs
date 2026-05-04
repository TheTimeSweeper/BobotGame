using UnityEngine;

namespace SpellCasting
{
    public class PlayerControllerInputController : InputController
    {
        Vector3 _lastGesture;
        Vector3 _aimCenter;

        protected override Vector3 GetAimPosition()
        {
            if (inputBank.CurrentPrimaryInput != null && inputBank.CurrentPrimaryInput.JustPressed(this))
            {
                _aimCenter = transform.InverseTransformPoint(aimOriginPosition.position +  GetPositionFromRightJoystick() * maxInputRange/2);
            }
            return aimOriginPosition.position + _aimCenter + GetPositionFromRightJoystick() * maxInputRange/2;
        }

        private static Vector3 GetPositionFromRightJoystick()
        {
            return new Vector3(Input.GetAxis("RightStickX"), 0, -Input.GetAxis("RightStickY"));
        }

        protected override void SetbuttonInputs()
        {
            inputBank.M1.UpdateInput(Input.GetAxis("TriggerR") < -0.5f);
            inputBank.M2.UpdateInput(Input.GetAxis("TriggerL") < -0.5f);
            inputBank.Space.UpdateInput(Input.GetButton("BumperL"));
            inputBank.Shift.UpdateInput(Input.GetButton("BumperR"));
            inputBank.E.UpdateInput(Input.GetButton("X"));
        }

        protected override Vector3 GetMovementInput()
        {
            return new Vector3(Input.GetAxis("HorizontalJoy"), 0, -Input.GetAxis("VerticalJoy"));
        }

        protected override Vector3 GetGestureDelta()
        {
            return new Vector3(Input.GetAxis("RightStickX"), -Input.GetAxis("RightStickY"));
        }

        protected override Vector3 GetAimDirection()
        {
            return GetPositionFromRightJoystick();
        }

        protected override Vector3 GetGesturePosition()
        {
            Vector3 aim = Util.ExpDecayLerp(_lastGesture, GetAimPosition() * 40, 20, Time.deltaTime);
            _lastGesture = aim;

            return new Vector3(aim.x, aim.z, 0);
        }
    }
}