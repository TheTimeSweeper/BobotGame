using ActiveStates;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace SpellCasting
{
    public class CommonComponentsHolder : MonoBehaviour
    {
        //if a class is added here, make sure to add the shorthand to ActiveState.cs
        //if this is jank please let me know why c:
        public HealthComponent HealthComponent;
        public InputBank InputBank;
        public CharacterBody CharacterBody;
        public StaminaComponent StaminaComponent;
        public FixedMotorDriver FixedMotorDriver;
        public CharacterModel CharacterModel;
        public StateMachineLocator StateMachineLocator;
        public HurtBoxLocator HurtBoxLocator;
        public TeamComponent TeamComponent;
        public Animator Animator;
        public SkillController SkillController;
        public ExpController expController;
        public StateInfoHolder stateInfoHolder;

        [HideInInspector]
        public ManaComponent ManaComponent;

        private void Reset()
        {
            Find();
        }

        [ContextMenu("Find")]
        private void Find()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "find");
#endif
            HealthComponent = GetComponent<HealthComponent>();
            InputBank = GetComponent<InputBank>();
            CharacterBody = GetComponent<CharacterBody>();
            StaminaComponent = GetComponent<StaminaComponent>();
            FixedMotorDriver = GetComponent<FixedMotorDriver>();
            CharacterModel = GetComponentInChildren<CharacterModel>();
            StateMachineLocator = GetComponent<StateMachineLocator>();
            TeamComponent = GetComponent<TeamComponent>();
            Animator = GetComponent<Animator>();
            SkillController = GetComponent<SkillController>();
            InputBank = GetComponentInChildren<InputBank>();
            HurtBoxLocator = GetComponent<HurtBoxLocator>();
        }
    }
}