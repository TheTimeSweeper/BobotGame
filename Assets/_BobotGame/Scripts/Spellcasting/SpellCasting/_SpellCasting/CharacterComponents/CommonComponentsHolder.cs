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
        public CharacterBody CharacterBody;
        public InputBank InputBank;
        public HealthComponent HealthComponent;
        public StaminaComponent StaminaComponent;
        public FixedMotorDriver FixedMotorDriver;
        public CharacterModel CharacterModel;
        public StateMachineLocator StateMachineLocator;
        public HurtBoxLocator HurtBoxLocator;
        public TeamComponent TeamComponent;
        public Animator Animator;
        public SkillController SkillController;
        public StateInfoHolder StateInfoHolder;
        public GenericHurtReaction GenericHurtReaction;
    }
}