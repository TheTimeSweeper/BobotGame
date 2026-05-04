using System;
using UnityEngine;
namespace SpellCasting
{
    public class ManaComponent : MonoBehaviour, IHasCommonComponents
    {
        [SerializeField]
        private CommonComponentsHolder _commonComopnents;
        public CommonComponentsHolder CommonComponents => _commonComopnents;

        public CharacterStats stats => CommonComponents.CharacterBody.stats;

        //jam characterbody stats
        [SerializeField]
        public float manaReturnedOnMelee = 4;

        public float FireMana;
        public float EarthMana;
        public float WaterMana;
        public float AirMana;

        void FixedUpdate()
        {
            //jam resource pool class
            FireMana = Mathf.Clamp(FireMana + stats.ManaRegeneration * Time.fixedDeltaTime, 0, stats.MaxFireMana);
            EarthMana = Mathf.Clamp(EarthMana + stats.ManaRegeneration * Time.fixedDeltaTime, 0, stats.MaxEarthMana);
            AirMana = Mathf.Clamp(AirMana + stats.ManaRegeneration * Time.fixedDeltaTime, 0, stats.MaxAirMana);
            WaterMana = Mathf.Clamp(WaterMana + stats.ManaRegeneration * Time.fixedDeltaTime, 0, stats.MaxWaterMana);
        }

        internal void SiphonMana()
        {
            FireMana = Mathf.Clamp(FireMana + manaReturnedOnMelee, 0, stats.MaxFireMana);
            EarthMana = Mathf.Clamp(EarthMana + manaReturnedOnMelee, 0, stats.MaxEarthMana);
            AirMana = Mathf.Clamp(AirMana + manaReturnedOnMelee, 0, stats.MaxAirMana);
            WaterMana = Mathf.Clamp(WaterMana + manaReturnedOnMelee, 0, stats.MaxWaterMana);
        }
    }
}