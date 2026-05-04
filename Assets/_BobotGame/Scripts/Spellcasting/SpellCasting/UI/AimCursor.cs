using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting.UI
{

    public class AimCursor : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
        }

        void Update()
        {
            UpdateScreen();
            _rectTransform.anchoredPosition = Util.ScreenToCanvasPosition(AimMouseManager.AimMousePosition);
        }

        private void UpdateScreen()
        {
        }
    }
}