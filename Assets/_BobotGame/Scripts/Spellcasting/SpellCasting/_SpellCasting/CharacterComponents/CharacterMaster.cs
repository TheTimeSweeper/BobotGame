using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
namespace SpellCasting
{
    public class CharacterMaster : MonoBehaviour
    {
        [Tooltip("Setup")]
        public CharacterBody bodyPrefab;
        [Tooltip("Parameters")]
        [SerializeField]
        private bool dontDestroyOnLoad;
        [SerializeField]
        private bool destroyOnBodyDeath;
        [SerializeField]
        private bool reviveOnBodyDeath;
        [SerializeField]
        public bool isPlayerControlled;
        [Header("Active")]
        [SerializeField, FormerlySerializedAs("body")]
        private CharacterBody currentBody;
        public CharacterBody CurrentBody => currentBody;

        public TeamIndex CachedTeamIndex {get; private set;}

        private CharacterBody lastBody;

        bool hadBody;
        public int TEMPPlayerIndex;

        public event System.Action<CharacterBody> OnBodyChanged;

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
                //todo bobot make these death behaviors or something
                if (reviveOnBodyDeath && bodyPrefab)
                {
                    currentBody = Instantiate(bodyPrefab);
                    currentBody.CommonComponents.TeamComponent.TeamIndex = CachedTeamIndex;
                }
                if (destroyOnBodyDeath) {
                    Destroy(gameObject);
                }
            }

            if(currentBody != lastBody)
            {
                OnBodyChanged?.Invoke(currentBody);
                CachedTeamIndex = currentBody.teamIndex;
                currentBody.Master = this;
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