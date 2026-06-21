using SpellCasting;
using Unity.VisualScripting;
using UnityEngine;

namespace ActiveStates.Characters
{
    //todo bobot skilldefbehaviors
    public abstract class GenericMeleeCombo : BasicMeleeAttack, IComboState
    {
        public abstract int ComboHits { get; }
        public int CurrentComboHit { get ; set ; }

        public override void ModifyNextState(ref ActiveState state)
        {
            base.ModifyNextState(ref state);

            if(state is GenericMeleeCombo comboState)
            {
                comboState.CurrentComboHit = (CurrentComboHit + 1) % ComboHits;
                comboState.aimDirection = aimDirection; 
            }
        }

        protected override void ModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.ModifyOverlapAttack(overlapAttack);
            if (meleeParams.staminaRecoveryOnHit > 0)
            {
                overlapAttack.DamageTakenEvent += OverlapAttack_DamageTakenEvent;
            }
        }

        private void OverlapAttack_DamageTakenEvent(GetDamagedData getDamagedInfo)
        {
            StaminaComponent staminaComponent = getDamagedInfo.DamagingInfo.AttackerBody?.CommonComponents.StaminaComponent;
            if (staminaComponent)
            {
                //todo bobot stamina modification stat
                staminaComponent.RefreshStamina(meleeParams.staminaRecoveryOnHit);
            }
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            Vector3 aimOut = inputBank.AimOut;
            aimOut.y = 0;
            Transform effectOriginTransform = base.characterModel.ChildLocator.LocateByName(meleeParams.effectOriginName).transform;
            EffectManager.SpawnEffect(EffectIndex.SWIPE_RIGHT, effectOriginTransform.position, Util.DirectionQuaternion(aimOut), effectOriginTransform.lossyScale, characterModel.CharacterDirection.transform);
        }
    }
}
