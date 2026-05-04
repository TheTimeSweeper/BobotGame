using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpellCasting.UI
{


    public class DebugMenu : MonoBehaviour
    {
        [SerializeField]
        private Button jango;
        [SerializeField]
        private Toggle jangoToggle;

        [SerializeField]
        private Slider timeScaleSlider;
        [SerializeField]
        private TMP_Text timeScaleText;

        [SerializeField]
        private BuffInfo godBuff;

        private bool _godding;

        private void Start()
        {
            jango.gameObject.SetActive(false);
            jangoToggle.gameObject.SetActive(false);

            timeScaleSlider.onValueChanged.AddListener(OnTimeScaleSliderChanged);

            timeScaleSlider.value = getInverseTimeScaleSliderValue();
            timeScaleText.text = $"Time Scale: {TimeStopperInstance.UnpausedTime.ToString("0.00")}";

            AddFunnyButton("Funny Respawn", FunnyRespawn);

            AddFunnyButton("Skip Level", LevelProgressionManager.Instance.NextLevel);

            Toggle joystickToggle = AddFunnyToggle("Controller", PlayerInputDecider.IsJoystick);
            joystickToggle.onValueChanged.AddListener(ToggleJoystick);

            AddFunnyButton("God Mode", GodMode);

            foreach (ElementType element in ElementCatalog.Instance.ElementTypesMap.Values)
            {
                AddToggleElementButton(element);
            }
        }

        private void ToggleJoystick(bool toggleValue)
        {
            PlayerInputDecider.IsJoystick = toggleValue;
        }

        private void GodMode()
        {
            _godding = !_godding;
            if (_godding)
            {
                CharacterBodyTracker.FindPrimaryPlayer().AddBuff(godBuff);
            } else
            {
                CharacterBodyTracker.FindPrimaryPlayer().Removebuff(godBuff);
            }
        }

        private void AddFunnyButton(string text, UnityAction onClick)
        {
            Button NewButton = Instantiate(jango, jango.transform.parent);
            NewButton.gameObject.SetActive(true);
            NewButton.GetComponentInChildren<TMP_Text>().text = text;
            NewButton.onClick.AddListener(onClick);
        }

        private void FunnyRespawn()
        {
            GameObject.FindWithTag("Player").GetComponent<FixedMotorDriver>().engine.Teleport(GameObject.FindWithTag("Level").transform.position + Vector3.up * 10);
        }

        private void AddToggleElementButton(ElementType value)
        {
            string toggleTitle = $"unlock {value.name}";
            bool toggleEnabled = MainGame.Instance.SavedData.UnlockedElements.Contains(value);

            Toggle newToggle = AddFunnyToggle(toggleTitle, toggleEnabled);

            newToggle.onValueChanged.AddListener(ToggleElement);

            void ToggleElement(bool toggled)
            {
                if (toggled)
                {
                    MainGame.Instance.SavedData.AddElement(value);
                }
                else
                {
                    MainGame.Instance.SavedData.RemoveElement(value);
                }
            }
        }

        private Toggle AddFunnyToggle(string toggleTitle, bool toggleEnabled)
        {
            Toggle newToggle = Instantiate(jangoToggle, jangoToggle.transform.parent);
            newToggle.gameObject.SetActive(true);

            newToggle.GetComponentInChildren<TMPro.TMP_Text>().text = toggleTitle;
            newToggle.isOn = toggleEnabled;

            return newToggle;
        }

        private float getInverseTimeScaleSliderValue()
        {
            if (TimeStopperInstance.UnpausedTime < 1)
            {
                return Mathf.Lerp(0, 0.25f, Mathf.InverseLerp(0, 1, TimeStopperInstance.UnpausedTime));
            }
            else if (TimeStopperInstance.UnpausedTime < 5)
            {
                return Mathf.Lerp(0.25f, 0.64f, Mathf.InverseLerp(1, 5, TimeStopperInstance.UnpausedTime));
            }
            else
            {
                return Mathf.Lerp(0.64f, 1, Mathf.InverseLerp(5, 50, TimeStopperInstance.UnpausedTime));
            }
        }

        public void OnTimeScaleSliderChanged(float sliderValue)
        {
            //split bar into 3 sections for time 0-1, 1-5, 5-50
            if (sliderValue < 0.25f) {

                sliderValue = Mathf.Lerp(0, 1, Mathf.InverseLerp(0f, 0.25f, sliderValue));
            } else if (sliderValue < 0.64f) {

                sliderValue = Mathf.Lerp(1, 5, Mathf.InverseLerp(0.25f, 0.64f, sliderValue));
            } else {

                sliderValue = Mathf.Lerp(5, 50, Mathf.InverseLerp(0.64f, 1f, sliderValue));
            }

            TimeStopperInstance.UnpausedTime = sliderValue;

            timeScaleText.text = $"Time Scale: {sliderValue.ToString("0.00")}";
        }
    }
}