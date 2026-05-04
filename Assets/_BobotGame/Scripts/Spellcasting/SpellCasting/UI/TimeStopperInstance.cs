using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting.UI
{
    public class TimeStopperInstance : MonoBehaviour
    {
        public static List<TimeStopperInstance> ReadOnlyInstancesList = new List<TimeStopperInstance>();
        public static float UnpausedTime = 1;

        private void OnEnable()
        {
            ReadOnlyInstancesList.Add(this);
            OnUpdasteInstanceList();
        }

        private void OnDisable()
        {
            ReadOnlyInstancesList.Remove(this);
            OnUpdasteInstanceList();
        }

        public static void OnUpdasteInstanceList()
        {
            Time.timeScale = ReadOnlyInstancesList.Count > 0 ? 0 : UnpausedTime;
        }
    }
}