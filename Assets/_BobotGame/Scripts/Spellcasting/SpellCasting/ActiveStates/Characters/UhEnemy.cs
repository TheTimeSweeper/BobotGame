namespace ActiveStates.Characters
{
    public class UhEnemy : BasicMeleeAttack
    {
        protected override string hitboxName => "ContactHitbox";

        protected override float damageCoefficient => 1;

        protected override float baseDuration => 0.2f;
        protected override float baseCastEndTimeFraction => 0;
        protected override float baseCastStartTimeFraction => 0;
        protected override float positionShift => 0;
    }
}
