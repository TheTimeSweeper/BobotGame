using JetBrains.Annotations;
using UnityEngine;

namespace SpellCasting.AI
{
    [CreateAssetMenu(menuName = "SpellCasting/AIGesturer/Click", fileName = "AIGestureClick")]
    public class AIGesturerClick : AIGesture
    {
        public override ScriptableObjectBehavior GetBehavior()
        {
            return new AIClickBehavior { infoObject = this };
        }

        public class AIClickBehavior : AIGestureBehavior<AIGesturerClick>
        {
            private Vector3 initialPosition;

            private bool _inputted;

            public override bool OnFixedUpdate(AIBrain brain)
            {
                bool end = base.OnFixedUpdate(brain);

                if (initialPosition == Vector3.zero)
                {
                    initialPosition = brain.CurrentTargetPosition;
                }

                brain.AIInputController.CurrentAimPosition = brain.CurrentTargetPosition;
                brain.AIInputController.OverrideGesturePosition = initialPosition;
                //brain.AIInputController.downInputs[InfoObject.inputIndex] = true;

                if (!_inputted)
                {
                    _inputted = true;
                    brain.AIInputController.JustPress(InfoObject.inputIndex);

                    if (InfoObject.inputIndex2 != -1)
                    {
                        //brain.AIInputController.downInputs[InfoObject.inputIndex2] = true;
                        brain.AIInputController.JustPress(InfoObject.inputIndex2);
                    }
                }

                return end;
            }
        }
    }
}
