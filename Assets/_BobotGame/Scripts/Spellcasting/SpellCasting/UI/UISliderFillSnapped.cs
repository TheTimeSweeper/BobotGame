using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(Slider))]
    public class UISliderFillSnapped : MonoBehaviour
    {

        [SerializeField]
        private Slider slider;
        [SerializeField]
        private List<float> snapPoints = new List<float> { 0, 0.25f, 0.5f, 0.75f, 1 };
        [SerializeField]
        private float snapThreshold = 0.04f;

        void Reset()
        {

            slider = GetComponent<Slider>();
        }

        void Update()
        {

            float snappedVal = getSliderSnapped(slider.value);
            slider.value = snappedVal;
        }

        private float getSliderSnapped(float val)
        {

            for (int i = 0; i < snapPoints.Count; i++)
            {

                float point = snapPoints[i];

                if (Util.AboutEqual(val, Mathf.Clamp(val, point - snapThreshold / 2, point + snapThreshold / 2)))
                    val = point;
            }

            return val;
        }

    }
}