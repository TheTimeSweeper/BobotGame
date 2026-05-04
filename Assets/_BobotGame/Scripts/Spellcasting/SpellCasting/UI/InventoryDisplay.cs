using UnityEngine;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    //jam ok this'll take a sec too long. maybe if I have extra time LOL
    public class InventoryDisplay: MonoBehaviour
    {
        [SerializeField]
        private Image prefab;

        private CharacterBody _body;

        public void Init(CharacterBody body_)
        {
            _body = body_;
        }

        private void Update()
        {
            if(_body != null)
            {
            }
        }
    }
}
