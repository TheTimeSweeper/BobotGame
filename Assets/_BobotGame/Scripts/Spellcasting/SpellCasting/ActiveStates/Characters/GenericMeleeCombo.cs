using SpellCasting;
using Unity.VisualScripting;

namespace ActiveStates.Characters
{
    public class GenericMeleeCombo : BasicMeleeAttack
    {
        protected virtual int comboHits => 1;
        protected override string hitboxName => "BasicHitbox";
        protected override float damageCoefficient => 0.5f;
        protected override float baseDuration => 0.5f;
        protected override float baseCastStartTimeFraction => 0.1f;
        protected override float baseCastEndTimeFraction => 0.4f;
        protected override float baseOtherStateInterruptTimeFraction => 0.5f;
        protected override float baseMovementInterruptTimeFraction => 0.6f;

        public int hits;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            //if(fixedAge > otherStateInterruptTime)
            //{
            //    SetNextState();
            //}
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
            if(manaComponent != null)
            {
                manaComponent.SiphonMana();
            }
        }

        //protected override void SetNextState()
        //{
        //    if (hits < comboHits)
        //    {
        //        //jam ugly
        //        Machine.SetState(ActiveStateCatalog.InstantiateState(this.GetType().ToString()));
        //    }
        //    else
        //    {
        //        base.SetNextState();
        //    }
        //}

        //protected override void OnMovementInterrupt()
        //{
        //    if (hits < comboHits)
        //    {
        //        //jam ugly
        //        Machine.SetState(ActiveStateCatalog.InstantiateState(this.GetType().ToString()));
        //    }
        //    else
        //    {
        //        base.OnMovementInterrupt();
        //    }
        //}

        public override void ModifyNextState(ref ActiveState state)
        {
            base.ModifyNextState(ref state);

            GenericMeleeCombo comboState;
            if((comboState = state as GenericMeleeCombo) != null)
            {
                comboState.hits = (hits + 1) % comboHits;
                comboState.aimDirection = aimDirection; 
            }

        }
    }
}
