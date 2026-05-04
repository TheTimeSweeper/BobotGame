using ActiveStates;
using ActiveStates.Characters;
using ActiveStates.Elements;
using SpellCasting.Projectiles;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace SpellCasting
{
    [System.Serializable]
    public class ElementActionState
    {
        [SerializeField]
        private AimGesture gestureType;
        public AimGesture GestureType => gestureType;

        [SerializeField]
        private SerializableActiveState gestureState;
        public SerializableActiveState GestureState => gestureState;

        [SerializeField]
        private float baseManaCost;
        public float BaseManaCost => baseManaCost;
    }

    [CreateAssetMenu(menuName = "SpellCasting/ElementType/Unspecified", fileName = "Element")]
    public class ElementType : ScriptableObject
    {

        [SerializeField]
        private ElementTypeIndex index;
        public ElementTypeIndex Index { get => index; }

        [SerializeField]
        private ElementTypeIndex baseElement;
        public ElementTypeIndex BaseElement { get => baseElement; }

        [SerializeField]
        public string DisplayName;

        [SerializeField]
        public Sprite Icon;

        [SerializeField]
        private Color elementColor;
        public Color ElementColor { get => elementColor; }

        [SerializeField]
        private List<ElementType> componentElements;
        public List<ElementType> ComponentElements => componentElements;

        [SerializeField, Space]
        private ElementMass elementMassPrefab;
        public ElementMass ElementMassPrefab { get => elementMassPrefab; }

        [SerializeField]
        private CursorIndicator cursorIndicatorPrefab;
        public CursorIndicator CursorIndicatorPrefab { get => cursorIndicatorPrefab; set => cursorIndicatorPrefab = value; }

        [SerializeField]
        private float massGrowthMultiplier;
        public float MassGrowthMultiplier { get => massGrowthMultiplier; }

        [SerializeField]
        private float massBaseMass;
        public float BaseMass { get => massBaseMass; }

        [SerializeField]
        private float massVelocityMultiplier;
        public float MassVelocityMultiplier { get => massVelocityMultiplier; }

        [SerializeField]
        private float massOffsetDistance = 1;
        public float MassOffsetDistance { get => massOffsetDistance; }

        [SerializeField, FormerlySerializedAs("spawnState")]
        private SerializableActiveState massSpawnState = new SerializableActiveState(typeof(BasicElementGrow));
        public SerializableActiveState MassSpawnState => massSpawnState;

        [SerializeField, FormerlySerializedAs("letGoState")]
        private SerializableActiveState massLetGoState = new SerializableActiveState(typeof(BasicElementLetGo));
        public SerializableActiveState MassLetGoState => massLetGoState;

        [SerializeField]
        private SerializableActiveState bodyMeleeStaterino = new SerializableActiveState(typeof(GenericMeleeCombo));
        public SerializableActiveState BodyMeleeState => bodyMeleeStaterino;
        [Space]
        [SerializeField]
        private List<ElementActionState> elementActions;
        public List<ElementActionState> ElementActions => elementActions;

        [Header("Prefabs")]
        [SerializeField]
        private ProjectileController explosionPrefab;
        public ProjectileController ExplosionPrefab => explosionPrefab;

        [SerializeField]
        private ProjectileController poolPrefab;
        public ProjectileController PoolPrefab => poolPrefab;

        public bool IsSecondary => componentElements.Count > 1;

        public SerializableActiveState FindGestureStateType(AimGesture gesture)
        {
            ElementActionState elementActionState = elementActions.Find((action) =>
                    {
                        return action.GestureType == gesture;
                    });
            return elementActionState != null ? elementActionState.GestureState : default;
        }

        public BaseElementMassState CreateElementMassState(AimGesture gesture, ElementMass elementMass)
        {
            SerializableActiveState state = FindGestureStateType(gesture);
            if (string.IsNullOrEmpty(state.activeStateName))
            {
                return null;
            }
            return CreateElementMassState(state, elementMass);
        }
        public BaseElementMassState CreateElementMassState(SerializableActiveState state, ElementMass elementMass)
        {
            BaseElementMassState newState = ActiveStateCatalog.InstantiateState<BaseElementMassState>(state);
            newState.Init(elementMass, this);
            return newState;
        }
    }
}