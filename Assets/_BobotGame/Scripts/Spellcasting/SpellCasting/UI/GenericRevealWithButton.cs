using UnityEngine;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    public class GenericRevealWithButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject toReveal;
        [SerializeField]
        private Button Button;

        private void Awake()
        {
            Button.onClick.AddListener(reveal);
        }

        private void reveal()
        {
            toReveal.SetActive(!toReveal.activeSelf);
        }
    }
}