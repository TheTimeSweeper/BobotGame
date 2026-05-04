using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/GestureType/Swipe", fileName = "GestureSwipe")]
    public class SwipeGesture : AimGesture
    {
        [SerializeField]
        private float threshold = 0.2f;

        public override ScriptableObjectBehavior GetBehavior()
        {
            return new SwipeGestureBehavior { infoObject = this };
        }

        public class SwipeGestureBehavior : GestureBehavior<SwipeGesture>
        {
            public override bool QualifyGesture(InputBank bank, InputState inputState)
            {
                bank.DebugSwipeMag = bank.GestureDistance;

                return bank.GestureDistance > InfoObject.threshold && bank.GestureDelta != Vector3.zero;
            }
        }
    }
}