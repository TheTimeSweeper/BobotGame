using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/Item", fileName = "Item")]
    public class PowerItem : ScriptableObject
    {
        [SerializeField]
        public string displayName;

        [SerializeField]
        public string description;

        [SerializeField]
        public Sprite icon;

        [SerializeField]
        public BuffInfo buffToApply;

        [SerializeField]
        public float buffDuration = -1;
    }
}