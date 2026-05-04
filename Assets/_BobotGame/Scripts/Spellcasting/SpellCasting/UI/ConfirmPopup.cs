using SpellCasting.UI;
using System;
using System.Collections;
using System.Threading.Tasks.Sources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    public class ConfirmPopup : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text messageText;

        [SerializeField]
        private TMP_Text yesText;
        [SerializeField]
        private Button yesButton;

        [SerializeField]
        private TMP_Text noText;
        [SerializeField]
        private Button noButton;

        public static void Open(string message, Action OnConfirm = null) => Open(message, "yes", "no", OnConfirm);
        public static void Open(string message, string yes = "yes", string no = "no", Action OnConfirm = null)
        {
            Instantiate(Resources.Load<ConfirmPopup>("ConfirmPopup"), MainUICanvas.Instance?.Center).Init(message, yes, no, OnConfirm);
        }

        private void Init(string message, string yes, string no, Action onConfirm)
        {
            messageText.text = message;
            InitButton(yes, yesText, yesButton, onConfirm);
            InitButton(no, noText, noButton);
        }

        private void InitButton(string label, TMP_Text text, Button button, Action onConfirm = null)
        {
            if (string.IsNullOrEmpty(label))
            {
                button.gameObject.SetActive(false);
                return;
            }
            text.text = label;

            button.onClick.AddListener(Button);

            void Button()
            {
                onConfirm?.Invoke();
                Close();
            }
        }

        public IEnumerator FuckinClose()
        {
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }
        private void Close()
        {
            Destroy(gameObject);
            StartCoroutine(FuckinClose());
        }
    }
}