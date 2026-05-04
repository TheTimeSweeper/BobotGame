using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpellCasting.UI
{
    internal class SkillIcon : MonoBehaviour
    {
        public TMP_Text name;
        public Button button;

        internal void Init(SkillInfo skillToUpgrade, Action onComplete)
        {
            name.text = skillToUpgrade.displayName;
            //description.text = skillinfo.description
            //icon = skillinfo.icon
            button.onClick = new Button.ButtonClickedEvent();
            if (onComplete != null)
            {
                button.onClick.AddListener(() => onComplete());
            }
        }
    }
}
