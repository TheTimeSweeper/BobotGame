using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/GestureType/Nothing", fileName = "GestureNothing")]
    public class NothingAimGesture : AimGesture
    {
        public override ScriptableObjectBehavior GetBehavior()
        {
            return new NothingGestureBehavior();
        }

        public class NothingGestureBehavior : GestureBehavior<NothingAimGesture>
        {
            public override bool QualifyGesture(InputBank bank, InputState inputState)
            {
                return bank.GestureDelta == Vector3.zero;
            }
        }
    }
}