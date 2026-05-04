using SpellCasting.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{

    public class PlayerInputControllerTopDown : InputController
    {
        [SerializeField]
        private CameraController cameraController;

        protected override Vector3 GetAimPosition()
        {
            return cameraController.GetAimPointFromMouse(Input.mousePosition);
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

        protected override Vector3 GetGesturePosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.x /= Screen.width;
            mousePosition.y /= Screen.height;
            return mousePosition * 100;
        }
    }
}