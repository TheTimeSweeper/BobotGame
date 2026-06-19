using UnityEngine;

namespace SpellCasting
{
    public class PlayerInputControllerFirstDemo : InputController
    {
        protected override Vector3 GetAimPosition()
        {
            if (CharacterBodyTracker.FindPrimaryPlayer())
            {
                return CharacterBodyTracker.FindPrimaryPlayer().transform.position;
            }
            return default;
        }

        protected override Vector3 GetGesturePosition()
        {
            return default;
        }

        protected override Vector3 GetMovementInput()
        {
            return default;
        }

        protected override void SetbuttonInputs()
        {
            inputBank.Block.UpdateInput(Input.GetKeyDown(KeyCode.G));
            inputBank.Shift.UpdateInput(Input.GetKeyDown(KeyCode.T));
            inputBank.Primary.UpdateInput(false);
            inputBank.Heavy.UpdateInput(false);
            inputBank.Space.UpdateInput(false);
        }
    }
}