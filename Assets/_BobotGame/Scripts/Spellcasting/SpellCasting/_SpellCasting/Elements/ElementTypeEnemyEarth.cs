using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/ElementType/EnemyEarth", fileName = "ElementEnemyEarth")]
    public class ElementTypeEnemyEarth : ElementType
    {
        [SerializeField, Header("Enemy Fire")]
        public CharacterBody MinionPrefab;
    }
}