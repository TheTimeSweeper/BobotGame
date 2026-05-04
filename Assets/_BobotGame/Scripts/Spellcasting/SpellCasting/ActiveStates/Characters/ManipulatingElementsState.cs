using SpellCasting;
using System.Collections.Generic;

namespace ActiveStates.Characters
{
    public class ManipulatingElementsState : ActiveState
    {
        public List<ElementInputBehavior> _elementManipulations = new List<ElementInputBehavior>();

        public override void OnEnter()
        {
            base.OnEnter();
            _elementManipulations = caster.ElementInputBehaviors;
        }

        public override void OnUpdate()
        {
            for (int i = 0; i < _elementManipulations.Count; i++)
            {
                _elementManipulations[i].Update();
            }
        }

        public override void OnFixedUpdate()
        {
            for (int i = 0; i < _elementManipulations.Count; i++)
            {
                _elementManipulations[i].FixedUpdate();
            }
        }

        public override void OnExit(bool machineded = false)
        {
            for (int i = 0; i < _elementManipulations.Count; i++)
            {
                _elementManipulations[i].OnManipluatorExit();
            }
        }
    }
}
