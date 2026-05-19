using UnityEngine;

namespace SpellCasting
{
    public class PlayerInputControllerFirstPerson : InputController
    {
        [SerializeField]
        private CameraController cameraController;

        private Quaternion lastCamRotation;
        private Vector3 lastCamPosition;

        private Vector3 lastAimPosition;

        protected override Vector3 GetAimPosition()
        {
            //todo bobot lol
            cameraController.CameraX.Rotate(Vector3.up, Input.GetAxis("Mouse X"), Space.Self);
            cameraController.CameraY.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"), Space.Self);
            if (cameraController.CameraPoint.position == lastCamPosition && cameraController.CameraPoint.rotation == lastCamRotation) {
                return lastAimPosition;
            }
            if (!Util.DebugKey)
            {
                lastCamPosition = cameraController.CameraPoint.position;
                lastCamRotation = cameraController.CameraPoint.rotation;
            }
            if (Physics.Raycast(cameraController.CameraPoint.position, cameraController.CameraPoint.forward, out var raycastHit, maxInputRange, LayerInfo.Hurtbox.layerMask.value))
            {
                if (Util.DebugKey)
                {
                    Debug.DrawLine(cameraController.CameraPoint.position, raycastHit.point);
                }
                lastAimPosition = raycastHit.point;
                return raycastHit.point;
            }
            Vector3 defaultPoint = cameraController.CameraPoint.position + (cameraController.CameraPoint.forward * maxInputRange);
            lastAimPosition = defaultPoint;
            return defaultPoint;
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