using JetBrains.Annotations;
using UnityEngine;

namespace SpellCasting.AI
{
    [CreateAssetMenu(menuName = "SpellCasting/AIGesturer/Click", fileName = "AIGestureClick")]
    public class AIGesturerClick : AIGesture
    {
        [Range(0,1)]
        public float crouchChance;

        public float delayAfterCrouch;

        public override ScriptableObjectBehavior GetBehavior()
        {
            return new AIClickBehavior { infoObject = this };
        }

        public class AIClickBehavior : AIGestureBehavior<AIGesturerClick>
        {
            private Vector3 initialPosition;

            private bool _inputted;

            public override void OnEnter(AIBrain brain)
            {
                base.OnEnter(brain);

                if (InfoObject.inputIndex2 != -1 && Random.value < InfoObject.crouchChance)
                {
                    brain.AIInputController.Hold(InfoObject.inputIndex2, -1);
                }
            }

            public override bool OnFixedUpdate(AIBrain brain)
            {
                bool end = base.OnFixedUpdate(brain);

                if (initialPosition == Vector3.zero)
                {
                    initialPosition = brain.CurrentTargetPosition;
                }

                brain.AIInputController.CurrentAimPosition = brain.CurrentTargetPosition;
                brain.AIInputController.OverrideGesturePosition = initialPosition;

                if (!_inputted && fixedAge > InfoObject.delayAfterCrouch)
                {
                    _inputted = true;
                    brain.AIInputController.JustPress(InfoObject.inputIndex);
                }

                return end;
            }
            public override void End(AIBrain brain)
            {
                base.End(brain);
                //todo bobot, ugh
                brain.AIInputController.Hold(InfoObject.inputIndex2, 1);
            }
        }
    }
}
