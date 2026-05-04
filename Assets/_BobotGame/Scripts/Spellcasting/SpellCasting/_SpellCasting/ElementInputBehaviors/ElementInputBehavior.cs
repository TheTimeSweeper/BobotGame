using System;

namespace SpellCasting
{
    public abstract class ElementInputBehavior
    {
        protected CommonComponentsHolder commonComponents;
        protected Caster caster;
        protected InputBank inputBank;

        protected ElementType currentCastingElement => caster.CurrentCastingElement;
        protected InputState input => caster.CurrentCastingInput;

        public ElementInputBehavior(CommonComponentsHolder commonComponents_)
        {
            Init(commonComponents_);
        }

        protected void Init(CommonComponentsHolder commonComponents_)
        {
            commonComponents = commonComponents_;
            inputBank = commonComponents.InputBank;
            caster = commonComponents.Caster;
        }

        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnManipluatorExit() { }
    }
}