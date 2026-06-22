using System;
using UnityEngine;

namespace SpellCasting.UI
{
    [CreateAssetMenu(menuName = "BobotGame/ButtonBehavior/ChangeCharacter", fileName = "butChangeCharacter_")]
    public class ChangeCharacterButtonBehavior : ButtonBehavior
    {
        [SerializeField]
        CharacterBody bodyPrefab;

        [SerializeField, Tooltip("starts at 1")]
        private int playerIndex = 1;
        public override void OnButtonClick()
        {
            CharacterMaster master = CharacterBodyTracker.FindBodyByFilter(bodyFilter)?.Master;
            if (!master)
            {
                return;
            }
            master.bodyPrefab = bodyPrefab;
            master.CurrentBody.CommonComponents.HealthComponent.Kill();
        }

        private bool bodyFilter(CharacterBody body)
        {
            if (!body)
                return false;

            return body.isPlayerControlled && body.Master.TEMPPlayerIndex == playerIndex;
        }
    }
}