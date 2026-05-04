using ActiveStates;
using ActiveStates.Characters;
using UnityEngine;
namespace SpellCasting
{

    [CreateAssetMenu(menuName = "SpellCasting/DamageType/Stun", fileName = "DamageType")]
    public class StunningDamageType : DamageTypeInfo
    {
        [SerializeField]
        private float stunTime;

        [SerializeField]
        public float damageMultipler = 1;

        [SerializeField]
        private GameObject effectPrefab;

        [SerializeField]
        private InterruptPriority stunInterruptPriority= InterruptPriority.STUN;

        [SerializeField]
        public SerializableActiveState stunState = new SerializableActiveState(typeof(StunnedState));

        public override void OnTakeDamage(GetDamagedData getDamagedInfo)
        {
            base.OnTakeDamage(getDamagedInfo);

            if (getDamagedInfo.VictimBody != null)
            {
                StunnedState state = ActiveStateCatalog.InstantiateState<StunnedState>(stunState);
                state.StunTime = stunTime * getDamagedInfo.VictimBody.stats.StunFactor;
                state.EffectPrefab = effectPrefab;

                getDamagedInfo.VictimBody.CommonComponents.StateMachineLocator.SetStates(state, stunInterruptPriority);
            }
        }
    }
}