using UnityEngine;

namespace SpellCasting.AI
{
    [CreateAssetMenu(menuName = "SpellCasting/AIGesturer/Nothing", fileName = "AIGestureNothing")]
    public class AiGestureNothing : AIGesture
    {
        public override ScriptableObjectBehavior GetBehavior()
        {
            return new AINothingBehavior { infoObject = this };
        }

        public class AINothingBehavior : AIGestureBehavior<AiGestureNothing> { }
    }
}
