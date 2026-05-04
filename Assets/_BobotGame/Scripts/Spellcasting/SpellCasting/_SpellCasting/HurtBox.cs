using UnityEngine;
namespace SpellCasting
{
    public class HurtBox : MonoBehaviour
    {
        [SerializeField]
        private HealthComponent healthcomponent;
        public HealthComponent HealthComponent { get => healthcomponent; }
    }
}