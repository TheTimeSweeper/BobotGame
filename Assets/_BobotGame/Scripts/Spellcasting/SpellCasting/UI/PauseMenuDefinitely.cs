using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    public class PauseMenuDefinitely : MonoBehaviour
    {
        [SerializeField]
        private GameObject menuObject;

        [SerializeField]
        private Button buttonPrefab;
        [SerializeField]
        private Button quitButton;

        private void Awake()
        {
            quitButton?.onClick.AddListener(quitClick);
            menuObject?.SetActive(false);
        }

        private void quitClick()
        {
            Application.Quit();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)/*MainGame.EscapeInput.JustPressed(this)*/)
            {
                menuObject.SetActive(!menuObject.activeInHierarchy);
            }
        }

        private void titleClick()
        {
            ConfirmPopup.Open("Do you want to reset your run?", "yes", "no", () =>
            {
                menuObject.SetActive(false);
                LevelProgressionManager.TrueReset();
            });
        }
    }
}