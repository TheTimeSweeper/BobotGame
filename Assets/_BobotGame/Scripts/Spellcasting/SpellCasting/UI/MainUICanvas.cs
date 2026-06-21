using UnityEngine;

namespace SpellCasting.UI
{
    public class MainUICanvas : MonoBehaviour
    {
        public static MainUICanvas Instance;

        [SerializeField]
        public RectTransform Center;

        [SerializeField]
        public RectTransform LowerLeftCorner;

        [SerializeField]
        public RectTransform LowerRightCorner;

        [SerializeField]
        public RectTransform LowerCenter;

        [SerializeField]
        public RectTransform UpperLeftCorner;

        private void Awake()
        {
            Instance = this;
        }
    }
}
