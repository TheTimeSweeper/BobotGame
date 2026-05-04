namespace ActiveStates.Elements
{
    public class BasicElementGrow : BaseElementMassState
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            elementMass.Grow(elementType.MassGrowthMultiplier);

            elementMass.SetPosition(inputBank.AimPoint, caster.transform);

            for (int i = 0; i < elementMass.SubMasses.Count; i++)
            {
                elementMass.SubMasses[i].SetPosition(elementMass.CenterPosition);
            }
        }
    }
}
