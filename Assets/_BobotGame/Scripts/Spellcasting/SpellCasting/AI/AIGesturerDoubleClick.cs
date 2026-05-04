using UnityEngine;

namespace SpellCasting.AI
{
    [CreateAssetMenu(menuName = "SpellCasting/AIGesturer/DoubleClick", fileName = "AIGestureDoubleClick")]
    public class AIGesturerDoubleClick : AIGesture
    {
        public override ScriptableObjectBehavior GetBehavior()
        {
            return new AIDoubleClickBehavior { infoObject = this };
        }

        public class AIDoubleClickBehavior : AIGestureBehavior<AIGesturerDoubleClick>
        {
            private Vector3 initialPosition;

            private float _clickTim = 0.1f;
            private int _clicks;

            public override bool OnFixedUpdate(AIBrain brain)
            {
                bool end = base.OnFixedUpdate(brain);

                if (initialPosition == Vector3.zero)
                {
                    initialPosition = brain.CurrentTargetPosition;
                }

                brain.AIInputController.CurrentAimPosition = brain.CurrentTargetPosition;
                brain.AIInputController.OverrideGesturePosition = initialPosition;

                _clickTim -= Time.fixedDeltaTime;
                if (_clickTim < 0 && _clicks < 2)
                {
                    _clicks++;
                    _clickTim = 0.3f;
                    brain.AIInputController.JustPress(InfoObject.inputIndex);
                }
                //brain.AIInputController.downInputs[InfoObject.inputIndex] = true;

                //if (InfoObject.inputIndex2 != -1)
                //{
                //    brain.AIInputController.just[InfoObject.inputIndex2] = true;
                //}

                return end;
            }
        }
    }
}
