using UnityEngine;

namespace SpellCasting.UI
{

    public class PlayerHud : MonoBehaviour
    {
        [SerializeField]
        private HealthBar healthBar;
        [SerializeField]
        private StaminaBar staminaBar;

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
            healthBar.Init(_currentBody.CommonComponents.HealthComponent);
            staminaBar.Init(_currentBody.CommonComponents.StaminaComponent);
        }
    }
}
