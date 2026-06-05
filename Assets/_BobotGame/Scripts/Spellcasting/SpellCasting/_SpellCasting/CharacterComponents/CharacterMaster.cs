using UnityEngine;
using UnityEngine.Serialization;
namespace SpellCasting
{
    public class CharacterMaster : MonoBehaviour
    {
        [Tooltip("Parameters")]
        [SerializeField]
        private bool dontDestroyOnLoad;
        [SerializeField]
        private bool destroyOnBodyDeath;
        [Header("Active")]
        [SerializeField, FormerlySerializedAs("body")]
        private CharacterBody currentBody;
        public CharacterBody CurrentBody => currentBody;

        private CharacterBody lastBody;

        bool hadBody;
        public System.Action<CharacterBody> OnBodyChanged;

        //fine, inventory

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                transform.parent = null;
                Object.DontDestroyOnLoad(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if(!currentBody && hadBody)
            {
                if (destroyOnBodyDeath) {
                    Destroy(gameObject);
                }
            }

            if(currentBody != lastBody)
            {
                OnBodyChanged(currentBody);
            }
            lastBody = currentBody;
        }

        private void Update()
        {
            if (currentBody)
            {
                hadBody = true;
                transform.position = currentBody.transform.position;
            }       
        }
    }
}