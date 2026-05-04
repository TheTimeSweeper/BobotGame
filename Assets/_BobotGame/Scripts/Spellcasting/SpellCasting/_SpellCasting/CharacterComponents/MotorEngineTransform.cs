using UnityEngine;

namespace SpellCasting
{
    public class MotorEngineTransform : MotorEngine
    {
        public override void FixedMove(Vector3 movement)
        {
            transform.Translate(movement);
        }
    }
}
