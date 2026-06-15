using UnityEngine;

namespace SpellCasting
{
    public class PlayerInputControllerFirstPersonGamePad : PlayerInputControllerFirstPerson
    {
        protected override float GetXAimAxis()
        {
            return Input.GetAxis("RightStickX");
        }
        protected override float GetYAimAxis()
        {
            return Input.GetAxis("RightStickY");
        }

        protected override void SetbuttonInputs()
        {
            inputBank.Primary.UpdateInput(Input.GetAxis("TriggerR") < -0.5f);
            inputBank.Block.UpdateInput(Input.GetAxis("TriggerL") < -0.5f);
            inputBank.Space.UpdateInput(Input.GetButton("BumperL"));
            inputBank.Heavy.UpdateInput(Input.GetButton("BumperR"));
            inputBank.Shift.UpdateInput(Input.GetButton("X"));
        }

        protected override Vector3 GetMovementInput()
        {
            return new Vector3(Input.GetAxis("HorizontalJoy"), 0, -Input.GetAxis("VerticalJoy"));
        }
    }
}