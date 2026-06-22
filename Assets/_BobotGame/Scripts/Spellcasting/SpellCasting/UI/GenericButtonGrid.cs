using UnityEngine;

namespace SpellCasting.UI
{
    public class GenericButtonGrid : MonoBehaviour
    {
        [SerializeField]
        private GenericButton buttonPrefab;

        [SerializeField]
        private Transform buttonParent;

        [SerializeField]
        private ButtonBehavior[] buttonBehaviors;

        private void Start()
        {
            for (int i = 0; i < buttonBehaviors.Length; i++)
            {
                CreateButton(buttonBehaviors[i]);
            }
        }

        private void CreateButton(ButtonBehavior buttonBehavior)
        {
            GenericButton button = Instantiate(buttonPrefab, buttonParent);
            button.button.onClick.AddListener(buttonBehavior.OnButtonClick);
            button.text.text = buttonBehavior.GetName();
        }
    }
}