using UnityEngine;
using ActiveStates.Characters;
using SpellCasting;

namespace ActiveStates.Heroes.Mars
{
    public class MarsComboBase : GenericMeleeCombo
    {
        protected override int comboHits => 2;
        protected override string hitboxName => "SwordHitbox";
        protected override float damageCoefficient => 1;
        protected override float baseCastStartTimeFraction => 7f / 34f;
        protected override float baseCastEndTimeFraction => 17f / 34f;
        protected override float baseDuration => 0.8f;
        protected override float baseOtherStateInterruptTimeFraction => 0.5f;
        protected override float baseMovementInterruptTimeFraction => 1;

        public override void OnEnter()
        {
            base.OnEnter();
            EnterSwing();
        }

        private void EnterSwing()
        {
            if (hits == 1)
            {
                ModifyHit2();
            }

            animator.Play(hits == 1 ? "Swing2" : "Swing1");
        }

        protected virtual void ModifyHit2()
        {
            duration *= 2;
            otherStateInterruptTime *= 2;
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            Vector3 scale = new Vector3(hits == 1 ? -1 : 1, 1, 1);
            EffectManager.SpawnEffect(EffectIndex.SWIPE_LEFT, transform.position, Util.DirectionQuaternion(inputBank.AimOut), scale, characterModel.CharacterDirection.transform);
        }
    }
}
