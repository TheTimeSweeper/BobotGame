using UnityEngine;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    public class GenericButton : MonoBehaviour
    {
        [SerializeField]
        public Button button;

        [SerializeField]
        public TMPro.TMP_Text text;

        public static implicit operator Button(GenericButton button)
        {
            return button.button;
        }
    }
}