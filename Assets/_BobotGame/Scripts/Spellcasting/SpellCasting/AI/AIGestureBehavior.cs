using UnityEngine;

namespace SpellCasting.AI
{
    public abstract class AIGestureBehavior : ScriptableObjectBehavior
    {
        protected float fixedAge;

        public abstract float CloseDistasnce { get; }

        public virtual void OnEnter(AIBrain brain) { }

        public abstract bool OnFixedUpdate(AIBrain brain);

        public abstract void End(AIBrain brain);
    }

    public abstract class AIGestureBehavior<T> : AIGestureBehavior where T : AIGesture
    {
        private T _infoObject;
        public T InfoObject
        {
            get
            {
                if (_infoObject == null)
                {
                    _infoObject = base.infoObject as T;
                }
                return _infoObject;
            }
        }
        public override float CloseDistasnce { get => InfoObject.CloseDistance; }

        //jam this should have been an activestate huh

        public override bool OnFixedUpdate(AIBrain brain)
        {
            fixedAge += Time.fixedDeltaTime;

            if (fixedAge > InfoObject.duration)
            {
                End(brain);
                return true;
            }
            return false;
        }

        public override void End(AIBrain brain)
        {
            brain.AIInputController.downInputs[InfoObject.inputIndex] = false;

            if (InfoObject.inputIndex2 != -1)
            {
                brain.AIInputController.downInputs[InfoObject.inputIndex2] = false;
            }
        }
    }
}
