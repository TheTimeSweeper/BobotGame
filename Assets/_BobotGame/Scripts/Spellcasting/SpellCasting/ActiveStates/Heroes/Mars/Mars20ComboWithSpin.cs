using ActiveStates.Characters;
using SpellCasting;
using UnityEngine;

namespace ActiveStates.Heroes.Mars
{
    public class Mars20ComboWithSpin : MarsComboBase
    {
        protected override void ModifyHit2()
        {
            return;
        }
        public override void ModifyNextState(ref ActiveState state)
        {
            if (hits == 1)
            {
                state = new MarsSpin();
            }
            base.ModifyNextState(ref state);
        }
    }

    public class MarsSpin : BasicMeleeAttack
    {
        protected override string hitboxName => "SpinHitbox";

        protected override float damageCoefficient => 2;

        protected override float baseDuration => 1.6f;
        protected override float baseCastStartTimeFraction => 0.1f;
        protected override float baseCastEndTimeFraction => 0.2f;
        protected override float baseOtherStateInterruptTimeFraction => 0.5f;
        protected override float baseMovementInterruptTimeFraction => 1;

        public override void OnEnter()
        {
            base.OnEnter();
            animator.Play("Spin");
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            Vector3 scale = GetEffectScale();
            Quaternion rotation = Util.DirectionQuaternion(characterModel.CharacterDirection.transform.forward);
            EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, transform.position, rotation, scale, characterModel.CharacterDirection.transform);

            rotation = Util.DirectionQuaternion(characterModel.CharacterDirection.transform.right);
            EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, transform.position, rotation, scale, characterModel.CharacterDirection.transform);

            rotation = Util.DirectionQuaternion(-characterModel.CharacterDirection.transform.right);
            EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, transform.position, rotation, scale, characterModel.CharacterDirection.transform);

            rotation = Util.DirectionQuaternion(-characterModel.CharacterDirection.transform.forward);
            EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, transform.position, rotation, scale, characterModel.CharacterDirection.transform);

        }

        protected virtual Vector3 GetEffectScale()
        {
            return new Vector3(1.823421f, 1, 1.186244f);
        }
    }

    public class Mars21ComboBigSpin : MarsComboBase
    {
        protected override void ModifyHit2()
        {
            return;
        }
        public override void ModifyNextState(ref ActiveState state)
        {
            if (hits == 1)
            {
                state = new MarsBigSpin();
            }
            base.ModifyNextState(ref state);
        }
    }

    internal class MarsBigSpin : MarsSpin
    {
        protected override string hitboxName => "BigSpinHitbox";
        protected override float baseDuration => 2f;

        //todo HitboxAndFXScale object
        protected override Vector3 GetEffectScale()
        {
            return new Vector3(3.288714f, 1.315729f, 2.2109f);
        }
    }


    public class Mars22ComboDoubleSpin : MarsComboBase
    {
        protected override void ModifyHit2()
        {
            return;
        }
        public override void ModifyNextState(ref ActiveState state)
        {
            if (hits == 1)
            {
                state = new MarsDoubleSpin();
            }
            base.ModifyNextState(ref state);
        }
    }

    internal class MarsDoubleSpin : MarsSpin
    {
        public int hits;
        protected override float baseDuration => 0.8f;

        protected override void OnCastExit()
        {
            base.OnCastExit();

            hits++;
            if (hits < 2)
            {
                Machine.SetState(new MarsDoubleSpin { hits = hits });
            }
        }
    }
}
