using UnityEngine;

namespace SpellCasting.UI
{

    public class PlayerHud : MonoBehaviour
    {
        [SerializeField]
        private ElementHud[] elementHuds;
        [SerializeField]
        private HealthBar healthBar;

        //[SerializeField]
        //private InventoryHud inventoryHud;

        private CharacterBody _currentBody;

        private void Update()
        {
            if(_currentBody == null)
            {
                _currentBody = CharacterBodyTracker.FindPrimaryPlayer();
                if(_currentBody != null)
                {
                    InitHuds();
                }
            }
        }

        private void InitHuds()
        {
            //elementHuds[0].Init(_currentBody.CommonComponents, _currentBody.CommonComponents.InputBank.M1);
            //elementHuds[1].Init(_currentBody.CommonComponents, _currentBody.CommonComponents.InputBank.M2);
            //elementHuds[2].Init(_currentBody.CommonComponents, _currentBody.CommonComponents.InputBank.Space);
            //elementHuds[3].Init(_currentBody.CommonComponents, _currentBody.CommonComponents.InputBank.Shift);

            healthBar.Init(_currentBody.CommonComponents.HealthComponent);
        }
    }
}
