using UnityEngine;
namespace SpellCasting
{
    public class CharacterMaster : MonoBehaviour
    {
        [SerializeField]
        private bool dontDestroy;
        [SerializeField]
        private CharacterBody body;
        public CharacterBody Body;

        //fine, inventory

        private void Awake()
        {
            if (dontDestroy)
            {
                transform.parent = null;
                Object.DontDestroyOnLoad(gameObject);
            }
        }
    }
}