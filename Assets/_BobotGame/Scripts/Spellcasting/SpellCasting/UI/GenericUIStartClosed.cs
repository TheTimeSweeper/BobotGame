using UnityEngine;

namespace SpellCasting.UI
{
    public class GenericUIStartClosed : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}