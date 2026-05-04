using ActiveStates;
using ActiveStates.Elements;
using System;
using UnityEngine;

namespace SpellCasting
{
    public class ElementMassManipulationBehavior : ElementInputBehavior
    {
        public ElementMassManipulationBehavior(CommonComponentsHolder commonComponents_) : base(commonComponents_) { }

        private ElementMass _currentMass;

        private float _heldTim;

        public override void FixedUpdate()
        {
            if (currentCastingElement == null)
                return;

            if (_currentMass != null && !_currentMass.Casted && _currentMass.Active)
            {
                if (_currentMass.ElementType != currentCastingElement)
                {
                    _currentMass.Fizzle();
                    return;
                }
                if (input.JustReleased(this))
                {
                    ElementActionState actionState = inputBank.GetFirstQualifiedElementAction(currentCastingElement.ElementActions);
                    if(actionState != null)
                    {
                        if (!commonComponents.CharacterBody.TrySpendMana(currentCastingElement.BaseElement, actionState.BaseManaCost))
                        {
                            EffectManager.SpawnEffect(EffectIndex.NOMANA, caster.transform.position, Quaternion.identity, caster.transform);
                            actionState = null;
                        }
                    }

                    if (actionState != null)
                    {
                        inputBank.ResetGestures();
                        BaseElementMassState newState = currentCastingElement.CreateElementMassState(actionState.GestureState, _currentMass);
                        if (newState != null)
                        {
                            _currentMass.ActiveStateMachine.SetState(newState);
                        }
                    }
                    else
                    {
                        BaseElementMassState newState = currentCastingElement.CreateElementMassState(currentCastingElement.MassLetGoState, _currentMass);
                        if (newState != null)
                        {
                            _currentMass.ActiveStateMachine.SetState(newState);
                        }
                    }
                }
            }

            if (input.JustPressed(this))
            {
                _heldTim = 0;
            }

            if (input.Down)
            {
                _heldTim += Time.fixedDeltaTime;

                if ((_currentMass == null || _currentMass.Casted || !_currentMass.Active) /*&& _heldTim > 0.05f*/)
                {
                    if(currentCastingElement == null)
                    {
                        Util.LogWarning("what the fuck");
                    }

                    _currentMass = UnityEngine.Object.Instantiate(currentCastingElement.ElementMassPrefab);

                    _currentMass.Init(currentCastingElement);
                    _currentMass.ActiveStateMachine.CommonComponents = commonComponents;
                    _currentMass.ActiveStateMachine.SetState(currentCastingElement.CreateElementMassState(currentCastingElement.MassSpawnState, _currentMass));
                    _currentMass.transform.position = inputBank.AimPoint;

                    _currentMass.SetPosition(inputBank.AimPoint, commonComponents.transform);
                }
            }
        }

        //public override void FixedUpdate()
        //{
        //    //    if (input.Down)
        //    //    {
        //    //        if (currentMass == null || currentMass.Casted)
        //    //        {
        //    //            currentMass = UnityEngine.Object.Instantiate(elementType.ElementPrefab);
        //    //            currentMass.Init(elementType);

        //    //            currentMass.ActiveStateMachine.setState(elementType.CreateElementMassState(elementType.SpawnState, currentMass, inputBank));
        //    //            currentMass.transform.position = inputBank.AimPoint;
        //    //        }

        //    //        currentMass.SetPosition(inputBank.AimPoint);
        //    //    }
        //}

        public override void OnManipluatorExit()
        {
            if(_currentMass != null)
            {
                _currentMass.Fizzle();
            }
        }

    }
}