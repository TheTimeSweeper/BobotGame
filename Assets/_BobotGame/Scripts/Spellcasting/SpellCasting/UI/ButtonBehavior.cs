using System;
using UnityEngine;

namespace SpellCasting.UI
{
    public abstract class ButtonBehavior : ScriptableObject
    {
        public abstract void OnButtonClick();


        [SerializeField]
        protected string buttonName;

        public string GetName()
        {
            return buttonName;
        }
    }
}