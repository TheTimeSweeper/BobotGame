using UnityEngine;

namespace SpellCasting.AI
{
    public abstract class AIGesture : ScriptableObject
    {
        [SerializeField]
        public float duration;

        [SerializeField]
        private SkillButton skillButton;//todo bobot input index not stupid
        [SerializeField]
        private SkillButton skillButton2 = SkillButton.None;

        public int inputIndex => (int)skillButton;//todo bobot input index not stupid
        public int inputIndex2 => (int)skillButton2;

        [SerializeField]
        public float CloseDistance;

        [SerializeField]
        public bool StrafeWhileClose;

        public abstract ScriptableObjectBehavior GetBehavior();
    }
}
