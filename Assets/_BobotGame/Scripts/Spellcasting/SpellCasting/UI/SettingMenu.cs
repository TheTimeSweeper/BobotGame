using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    public class SettingMenu : MonoBehaviour
    {
        [SerializeField]
        private Slider timeScaleSlider;
        [SerializeField]
        private TMP_Text timeScaleText;

        private void Start()
        {
            timeScaleSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
            BindUI();
        }

        private void OnEnable()
        {
            BindUI();
        }

        private void BindUI()
        {
            timeScaleSlider.value = AudioListener.volume;
            timeScaleText.text = $"Volume: {AudioListener.volume * 100}%";
        }

        private void OnVolumeSliderChanged(float arg0)
        {
            AudioListener.volume = arg0; BindUI();
        }
    }
}