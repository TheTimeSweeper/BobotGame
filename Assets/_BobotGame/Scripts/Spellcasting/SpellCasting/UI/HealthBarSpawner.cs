using UnityEngine;

namespace SpellCasting.UI
{
    public class HealthBarSpawner : MonoBehaviour
    {
        [SerializeField]
        private HealthBar prefab;

        [SerializeField]
        private HealthComponent healthComponent;

        private void Reset()
        {
            healthComponent = GetComponent<HealthComponent>();
        }

        void Start()
        {
            Instantiate(prefab, transform.position, Quaternion.identity, transform).Init(healthComponent);    
        }
    }
}