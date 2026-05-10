using UnityEngine;

namespace SpellCasting
{
    public class PlayerInputControllerFirstPerson : InputController
    {
        [SerializeField]
        private CameraController cameraController;

        protected override Vector3 GetAimPosition()
        {
            //todo bobot lol
            cameraController.CameraX.Rotate(Vector3.up, Input.GetAxis("Mouse X"), Space.Self);
            cameraController.CameraY.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"), Space.Self);
            //todo bobot raycast, solve the pierce bug lol
            return cameraController.CameraPoint.position + (cameraController.CameraPoint.forward * 100);
        }

        protected override Vector3 GetGesturePosition()
        {
            return Vector3.zero;
        }

        protected override void SetbuttonInputs()
        {
            inputBank.M1.UpdateInput(Input.GetMouseButton(0));
            inputBank.M2.UpdateInput(Input.GetMouseButton(1));
            inputBank.Space.UpdateInput(Input.GetKey(KeyCode.Space));
            inputBank.Shift.UpdateInput(Input.GetKey(KeyCode.LeftShift));
            inputBank.E.UpdateInput(Input.GetKey(KeyCode.E));
        }

        protected override Vector3 GetMovementInput()
        {
            return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
    }
}