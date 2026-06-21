using SpellCasting;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AnimationUtils;

namespace ActiveStates
{
    public abstract class ActiveState
    {
        public ActiveStateMachine Machine;
        public GameObject gameObject => Machine.gameObject;
        public Transform transform => Machine.transform;
        public CommonComponentsHolder components => Machine.CommonComponents;

        protected CharacterBody characterBody => Machine.CommonComponents.CharacterBody;
        protected InputBank inputBank => Machine.CommonComponents.InputBank;
        protected HealthComponent healthComponent => Machine.CommonComponents.HealthComponent;
        protected StaminaComponent staminaComponent => Machine.CommonComponents.StaminaComponent;
        protected FixedMotorDriver fixedMotorDriver => Machine.CommonComponents.FixedMotorDriver;
        protected CharacterModel characterModel => Machine.CommonComponents.CharacterModel;
        protected StateMachineLocator stateMachineLocator => Machine.CommonComponents.StateMachineLocator;
        protected HurtBoxLocator hurtBoxLocator => Machine.CommonComponents.HurtBoxLocator;
        protected TeamComponent teamComponent => Machine.CommonComponents.TeamComponent;
        //jam should be on charactermodel
        protected Animator animator => Machine.CommonComponents.Animator;
        protected SkillController skillController => Machine.CommonComponents.SkillController;
        protected StateInfoHolder stateInfoHolder => Machine.CommonComponents.StateInfoHolder;
        protected GenericHurtReaction genericHurtReaction => Machine.CommonComponents.GenericHurtReaction;

        private float _fixedAge;
        protected float fixedAge => _fixedAge;

        private Dictionary<Type, Component> _cachedComponents;// = new Dictionary<Type, Component>();

        protected void EndState()
        {
            Machine.EndState(this);
        }

        public virtual void OnFixedUpdate()
        {
            _fixedAge += Time.fixedDeltaTime;
        }
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit(bool machineDed = false) { }
        public virtual void ModifyNextState(ref ActiveState state) { }

        public virtual InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.STATE_ANY;
        }

        protected T GetComponent<T>() where T : Component
        {
            if (_cachedComponents == null)
            {
                _cachedComponents = new Dictionary<Type, Component>();
            }
            if(_cachedComponents.TryGetValue(typeof(T), out Component cachedComponent))
            {
                return (T)cachedComponent;
            }
            T component = Machine.GetComponent<T>();
            if(component)
            {
                _cachedComponents.Add(typeof(T), component);
            }
            return component;
        }


        protected bool TryGetComponent<T>(out T result) where T : Component
        {
            T component = GetComponent<T>();

            if(component)
            {
                result = component;
                return true;
            }
            result = null;
            return false;
        }

        public virtual ActiveState Clone(){ return ActiveStateCatalog.InstantiateState(this.GetType()); }


        public virtual void PlayAnimation(AnimationStateStringOrInt animationState)
            => PlayAnimationOnAnimator(animator, animationState);
        public virtual void PlayAnimation( LayerStringOrInt layerIndex, AnimationStateStringOrInt animationState)
            => PlayAnimationOnAnimator(animator, layerIndex, animationState, -1, -1);
        public virtual void PlayAnimation( LayerStringOrInt layerIndex, AnimationStateStringOrInt animationState, AnimationStateStringOrInt playbackRateParam, float duration)
            => PlayAnimationOnAnimator(animator, layerIndex, animationState, playbackRateParam, duration);

    }
}
