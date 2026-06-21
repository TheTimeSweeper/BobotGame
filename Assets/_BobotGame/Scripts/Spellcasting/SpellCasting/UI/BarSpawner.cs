using UnityEngine;

namespace SpellCasting.UI
{
    //todo bobot i'm on to something but i"m getting in the weeds
    public class MasterUIBarPopulator
    {
    }

    public class BarSpawner<TUIPrefabComponent, TProviderComponent> : MonoBehaviour where TUIPrefabComponent : UIBar where TProviderComponent : MonoBehaviour, IUIBarProvider
    {
        [SerializeField]
        private TUIPrefabComponent prefab;

        [SerializeField]
        private TProviderComponent providerComponent;

        private void Reset()
        {
            providerComponent = GetComponent<TProviderComponent>();
        }

        void Start()
        {
            Instantiate(prefab, transform.position, Quaternion.identity, transform).Init(providerComponent);
        }
    }
}