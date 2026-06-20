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
            cameraController.CameraX.Rotate(Vector3.up, GetXAimAxis(), Space.Self);
            cameraController.CameraY.Rotate(Vector3.right, GetYAimAxis(), Space.Self);
            if (cameraController.CameraPoint.position == lastCamPosition && cameraController.CameraPoint.rotation == lastCamRotation)
            {
                return lastAimPosition;
            }
            if (!Util.DebugKeyG)
            {
                lastCamPosition = cameraController.CameraPoint.position;
                lastCamRotation = cameraController.CameraPoint.rotation;
            }
            if (Physics.Raycast(cameraController.CameraPoint.position, cameraController.CameraPoint.forward, out var raycastHit, maxInputRange, LayerInfo.Hurtbox.layerMask.value))
            {
                if (Util.DebugKeyG)
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

        protected virtual float GetYAimAxis()
        {
            return -Input.GetAxis("Mouse Y");
        }

        protected virtual float GetXAimAxis()
        {
            return Input.GetAxis("Mouse X");
        }

        protected override Vector3 GetGesturePosition()
        {
            return Vector3.zero;
        }

        protected override void SetbuttonInputs()
        {
            inputBank.Primary.UpdateInput(Input.GetMouseButton(0));
            inputBank.Block.UpdateInput(Input.GetMouseButton(1));
            inputBank.Dash.UpdateInput(Input.GetKey(KeyCode.Space));
            inputBank.Crouch.UpdateInput(Input.GetKey(KeyCode.LeftAlt));
            inputBank.Ability.UpdateInput(Input.GetKey(KeyCode.LeftShift));
            inputBank.E.UpdateInput(Input.GetKey(KeyCode.E));
        }

        protected override Vector3 GetMovementInput()
        {
            return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
    }
}