using SpellCasting;
using UnityEngine;

namespace ActiveStates.Elements.Air
{
    public class AirSwipe : ElementThrowProjectile
    {
        public override void OnEnter()
        {
            base.OnEnter();
            elementMass.SubMasses[0].transform.position = characterBody.transform.position;

            EffectManager.SpawnEffect(EffectIndex.SOUND_FAST, transform.position, null, 23);

            Object.Instantiate(((ElementTypeAir)elementType).GustBurstPrefab, 
                characterBody.transform.position, 
                Quaternion.LookRotation(/*elementMass.CenterPositionRaw - elementMass.CenterPosition*/inputBank.AimDirection));
        }
    }
}