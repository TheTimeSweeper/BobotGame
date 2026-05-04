using UnityEngine;

namespace SpellCasting.AI
{
    [CreateAssetMenu(menuName = "SpellCasting/AIGesturer/Swirl", fileName = "AIGestureSwirl")]
    public class AIGesturerSwirl : AIGesture
    {
        [SerializeField]
        private bool targetSelf;

        [SerializeField]
        private float offsetDistance;

        [SerializeField]
        private float angularVelocity;

        public override ScriptableObjectBehavior GetBehavior()
        {
            return new AISwirlBehavior { infoObject = this };
        }

        public class AISwirlBehavior : AIGestureBehavior<AIGesturerSwirl>
        {
            private Vector3 initialPosition;
            private Vector3 offest = Vector3.forward;
            private float angle;
            private float velocity;

            public override bool OnFixedUpdate(AIBrain brain)
            {
                bool end = base.OnFixedUpdate(brain);

                brain.AIInputController.OverrideGesturePosition = Vector3.zero;

                if (initialPosition == Vector3.zero)
                {
                    if(InfoObject.targetSelf)
                    {
                        initialPosition = brain.transform.position;
                    }
                    else
                    {
                        initialPosition = brain.CurrentTargetPosition;
                    }
                    ;
                }

                brain.AIInputController.downInputs[InfoObject.inputIndex] = true;

                if (InfoObject.inputIndex2 != -1)
                {
                    brain.AIInputController.downInputs[InfoObject.inputIndex2] = true;
                }

                Quaternion quaternion = Quaternion.Euler(0, InfoObject.angularVelocity, 0);
                offest = quaternion * offest;

                brain.AIInputController.CurrentAimPosition = initialPosition + offest * InfoObject.offsetDistance;

                return end;
            }
        }
    }
}
