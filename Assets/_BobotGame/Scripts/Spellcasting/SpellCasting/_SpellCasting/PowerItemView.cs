using TMPro;
using UnityEngine;

namespace SpellCasting
{
    public class PowerItemView : MonoBehaviour
    {
        [SerializeField]
        private PowerItem itemInfo;

        [SerializeField]
        private SpriteRenderer rend;

        [SerializeField]
        private TMP_Text text;

        void Awake()
        {
            rend.sprite = itemInfo.icon;
            text.text = itemInfo.description;
        }
    } 
}