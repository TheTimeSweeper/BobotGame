using UnityEngine;

namespace SpellCasting.AI
{
    [CreateAssetMenu(menuName = "SpellCasting/AIGesturer/Swipe", fileName = "AIGestureSwipe")]
    public class AIGestureSwipe : AIGesture
    {
        [SerializeField]
        private AnimationCurve animationCurve;

        [SerializeField]
        private float quickness = 1;

        [SerializeField]
        private Vector3 startPosition;

        [SerializeField]
        private float throwDistance = 3;

        public override ScriptableObjectBehavior GetBehavior()
        {
            return new AISwipeBehavior { infoObject = this };
        }

        public class AISwipeBehavior: AIGestureBehavior<AIGestureSwipe>
        {
            private Vector3 _position;

            private bool _throwing;
            private float _throwTim;

            private Vector3 _throwDirection;

            public override bool OnFixedUpdate(AIBrain brain)
            {
                bool done = base.OnFixedUpdate(brain);

                //jam maybe really should have been an activestate
                if (!_throwing)
                {
                    brain.AIInputController.downInputs[InfoObject.inputIndex] = true;

                    if(InfoObject.inputIndex2 != -1)
                    {
                        brain.AIInputController.downInputs[InfoObject.inputIndex2] = true;
                    }
                    _position = brain.defaultAimPoint.TransformPoint(InfoObject.startPosition);

                    _throwDirection = (brain.CurrentTargetPosition - _position).normalized;
                    brain.AIInputController.CurrentAimPosition = _position;
                } 
                else
                {
                    brain.AIInputController.CurrentAimPosition = Vector3.Lerp(_position, _position + _throwDirection * InfoObject.throwDistance, InfoObject.animationCurve.Evaluate(_throwTim));
                    brain.AIInputController.OverrideGesturePosition = brain.AIInputController.CurrentAimPosition;
                    if (_throwTim > 0.8f)
                    {
                        brain.AIInputController.downInputs[InfoObject.inputIndex] = false;

                        if (InfoObject.inputIndex2 != -1)
                        {
                            brain.AIInputController.downInputs[InfoObject.inputIndex2] = false;
                        }
                    }

                    _throwTim += Time.fixedDeltaTime * InfoObject.quickness;
                }

                if(fixedAge > 1f)
                {
                    _throwing = true;
                }

                return done;
            }

            public override void End(AIBrain brain)
            {
                base.End(brain);
                brain.AIInputController.OverrideGesturePosition = Vector3.zero;
            }
        }
    }
}
