using UnityEngine;
namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/DamageType/Buff", fileName = "DamageType")]
    public class ApplyBuffDamageType : DamageTypeInfo
    {
        [SerializeField]
        private BuffInfo buff;

        [SerializeField]
        private float duration = -1;

        public override void OnTakeDamage(GetDamagedData damagedInfo)
        {
            base.OnTakeDamage(damagedInfo);

            if (damagedInfo.VictimBody == null)
                return;

            if(duration > 0)
            {
                damagedInfo.VictimBody.AddTimedBuff(buff, duration);
            } else
            {
                damagedInfo.VictimBody.AddBuff(buff);
            }
        }
    }
}