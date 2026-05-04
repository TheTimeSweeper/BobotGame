using UnityEngine;

namespace SpellCasting.AI
{
    public abstract class AIGesture : ScriptableObject
    {
        [SerializeField]
        public float duration;

        [SerializeField]
        public int inputIndex;
        [SerializeField]
        public int inputIndex2 = -1;

        [SerializeField]
        public float CloseDistance;

        [SerializeField]
        public bool StrafeWhileClose;

        public abstract ScriptableObjectBehavior GetBehavior();
    }
}
