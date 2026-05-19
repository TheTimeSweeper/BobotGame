using UnityEngine;
namespace SpellCasting
{
    public class CharacterMaster : MonoBehaviour
    {
        [SerializeField]
        private bool dontDestroy;
        [SerializeField]
        private CharacterBody body;
        public CharacterBody Body => body;

        bool hadBody;
        //fine, inventory

        private void Awake()
        {
            if (dontDestroy)
            {
                transform.parent = null;
                Object.DontDestroyOnLoad(gameObject);
            }
        }
        private void Update()
        {
            if (body)
            {
                hadBody = true;
                transform.position = body.transform.position;
            }
            else
            {
                if (!hadBody)
                {
                    Destroy(gameObject);
                }
            }        }
    }
}