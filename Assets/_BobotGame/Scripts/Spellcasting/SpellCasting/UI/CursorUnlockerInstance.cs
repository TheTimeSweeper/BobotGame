using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting.UI
{
    //generalize this system
    public class CursorUnlockerInstance : MonoBehaviour
    {
        public static List<CursorUnlockerInstance> ReadOnlyInstancesList = new List<CursorUnlockerInstance>();

        private void OnEnable()
        {
            ReadOnlyInstancesList.Add(this);

        }
        private void OnDisable()
        {
            ReadOnlyInstancesList.Remove(this);
        }
    }
}