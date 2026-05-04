using SpellCasting;

namespace ActiveStates.Elements
{
    public class BasicElementMelee : BaseElementMassState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            elementMass.Fizzle();
            //characterModel.CharacterDirection.OverrideDirection
            stateMachineLocator.MainStateMachine.TryInterruptState(ActiveStateCatalog.InstantiateState(elementType.BodyMeleeState), InterruptPriority.STATE_ANY);
        }
    }
}
