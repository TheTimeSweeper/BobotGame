using UnityEngine;
namespace SpellCasting
{
    public class CharacterMaster : MonoBehaviour
    {
        [SerializeField]
        private bool dontDestroy;

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