using ActiveStates;
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
        public Caster Caster;
        public ManaComponent ManaComponent;
        public FixedMotorDriver FixedMotorDriver;
        public CharacterModel CharacterModel;
        public StateMachineLocator StateMachineLocator;
        public TeamComponent TeamComponent;
        public Animator Animator;
        public SkillController SkillController;
        public ExpController expController;

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
            Caster = GetComponent<Caster>();
            ManaComponent = GetComponent<ManaComponent>();
            FixedMotorDriver = GetComponent<FixedMotorDriver>();
            CharacterModel = GetComponentInChildren<CharacterModel>();
            StateMachineLocator = GetComponent<StateMachineLocator>();
            TeamComponent = GetComponent<TeamComponent>();
            Animator = GetComponent<Animator>();
            SkillController = GetComponent<SkillController>();
            InputBank = GetComponentInChildren<InputBank>();
        }
    }
}