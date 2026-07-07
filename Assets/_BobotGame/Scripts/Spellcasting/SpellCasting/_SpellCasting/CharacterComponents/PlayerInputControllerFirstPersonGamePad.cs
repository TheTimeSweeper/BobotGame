using UnityEngine;

namespace SpellCasting
{
    public class PlayerInputControllerFirstPersonGamePad : PlayerInputControllerFirstPerson
    {
        protected override void Awake()
        {
            GamePad = true;
            base.Awake();
        }
    }
}