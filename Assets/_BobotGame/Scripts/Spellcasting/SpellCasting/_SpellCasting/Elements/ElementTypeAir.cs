using SpellCasting.Projectiles;
using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/ElementType/Air", fileName = "ElementAir")]
    public class ElementTypeAir : ElementType
    {
        [Header("Air")]
        [SerializeField]
        private ProjectileController succPrefab;
        public ProjectileController SuccPrefab => succPrefab;

        [SerializeField]
        private ProjectileController blowPrefab;
        public ProjectileController BlowPrefab => blowPrefab;

        [SerializeField]
        private GameObject gustBurstPrefab;
        public GameObject GustBurstPrefab => gustBurstPrefab;
    }
}