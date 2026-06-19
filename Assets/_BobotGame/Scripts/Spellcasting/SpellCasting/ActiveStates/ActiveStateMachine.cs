
using SpellCasting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveStates
{
    public class ActiveStateMachine : MonoBehaviour, IHasCommonComponents, ILabeled
    {
        [SerializeField]
        private string label;
        public string Label => label;

        [SerializeField]
        private CommonComponentsHolder commonComponents;
        public CommonComponentsHolder CommonComponents { get => commonComponents; set { commonComponents = value; } }

        [SerializeField]
        private SerializableActiveState DefaultState = new SerializableActiveState(typeof(IdleState));

#if UNITY_EDITOR
        [SerializeField]
        private string debug_currentState;
#endif
        public bool Destroyed { get; set; }

        private Queue<ActiveState> _queuedStates = new Queue<ActiveState>();
        private ActiveState _currentlyRunningState;
        public ActiveState CurrentState => _currentlyRunningState;

        private void Start()
        {
            if (_currentlyRunningState != null)
                return;

            _currentlyRunningState = new IdleState();

            if (string.IsNullOrEmpty(DefaultState.activeStateName))
                return;

            var defaultState = ActiveStateCatalog.InstantiateState(DefaultState);
            if (defaultState != null)
            {
                //Util.Log($"{label} entering state {DefaultState.activeStateName}");
                SetState(ActiveStateCatalog.InstantiateState(DefaultState));
            }
        }

        void FixedUpdate()
        {
            _currentlyRunningState.OnFixedUpdate();
        }
        void Update()
        {
            _currentlyRunningState.OnUpdate();
        }

        public void SetState(ActiveState newState)
        {
            if (newState == null)
            {
                Debug.LogError("Tried to enter a null state", this);
#if UNITY_EDITOR
                debug_currentState = null;
#endif
                return;
            }

            ExitCurrentState(ref newState);

            _currentlyRunningState = newState;

#if UNITY_EDITOR
            debug_currentState = _currentlyRunningState?.GetType().ToString();
#endif
            EnterCurrentState();
        }

        private void ExitCurrentState(ref ActiveState newState)
        {
            if (_currentlyRunningState != null)
            {
                _currentlyRunningState.ModifyNextState(ref newState);
                _currentlyRunningState.OnExit();
            }
        }

        private void EnterCurrentState()
        {
            _currentlyRunningState.Machine = this;
            if(commonComponents && commonComponents.stateInfoHolder && _currentlyRunningState is IHasStateInfoBase currentState)
            {
                currentState.SetStateInfo(commonComponents.stateInfoHolder);
            }
            _currentlyRunningState.OnEnter();
        }

        public void SetStateToDefault(bool clearQueue = false)
        {
            SetState(ActiveStateCatalog.InstantiateState(DefaultState));
            if (clearQueue)
            {
                _queuedStates.Clear();
            }
        }
        public void EndState(ActiveState state)
        {
            if (_currentlyRunningState != state)
            {
                Debug.LogError($"trying to end a state ({state.GetType()}) that is not the current state ({_currentlyRunningState.GetType()})");
                return;
            }
            EndState();
        }
        public void EndState()
        {
            if (_queuedStates.Count > 0)
            {
                SetState(_queuedStates.Dequeue());
            }
            else
            {
                SetStateToDefault();
            }
        }
        public void QueueState(ActiveState newState)
        {
            _queuedStates.Enqueue(newState);
        }
        public bool TryInterruptState(ActiveState activeState, InterruptPriority priority) => TryInterruptState(activeState, priority, out _);
        public bool TryInterruptState(ActiveState activeState, InterruptPriority priority, out ActiveState state)
        {
            if(_currentlyRunningState == null || _currentlyRunningState.GetMinimumInterruptPriority() <= priority)
            {
                SetState(activeState);

                state = activeState;
                return true;
            }

            state = null;
            return false;
        }

        void OnDestroy()
        {
            Destroyed = true;
            if (_currentlyRunningState != null)
            {
                _currentlyRunningState.OnExit(true);
                _currentlyRunningState = null;
            }
        }
    }
}
