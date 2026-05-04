using UnityEngine;

namespace SpellCasting
{
    public class PlayerInputDecider : MonoBehaviour
    {
        [SerializeField]
        private InputController notJoystickController;
        [SerializeField]
        private InputController yesJoystickController;

        public static bool IsJoystick;
        private static bool wasJoystick;
        private void Start()
        {
            wasJoystick = !IsJoystick;
        }
        private void Update()
        {
            if(IsJoystick != wasJoystick)
            {
                wasJoystick = IsJoystick;

                notJoystickController.enabled = !IsJoystick;
                yesJoystickController.enabled = IsJoystick;
            }
        }
    }
}