using ActiveStates;
using ActiveStates.AI;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpellCasting.AI
{
    [RequireComponent(typeof(CharacterMaster))]
    public class AIBrain : MonoBehaviour
    {
        [SerializeField]
        private AIGesture[] gestureBehaviors;

        [SerializeField]
        public AIInputController AIInputController;

        [SerializeField]
        public CharacterMaster master;

        [SerializeField]
        public Transform noTargetOverrideInterestPoint;

        [SerializeField]
        private ValidTarget teamTargetType = ValidTarget.OTHERTEAM;

        [SerializeField]
        private TeamComponent teamComponent; //todo bobot bodyTeamComponent?

        [SerializeField]
        private float searchDistance;
        [SerializeField]
        public float searchIntervalMin = 1;
        [SerializeField]
        public float searchIntervalMax = 2;

        [SerializeField]
        public float chaseTimeMinimunm = 1;

        [SerializeField]
        protected ActiveStateMachine aiStateMachine;

        private CharacterBody _targetBody;
        public CharacterBody CurrentTargetBody
        {
            get
            {
                if(_targetBody != null && !_targetBody.Ded)
                {
                    return _targetBody;
                } 
                else
                {
                    _targetBody = null;
                }
                return _targetBody;
            }
            set
            {
                _targetBody = value;
            }
        }

        private void Awake()
        {
            master.OnBodyChanged += master_OnBodyChanged;
        }
        private void OnDestroy()
        {
            master.OnBodyChanged -= master_OnBodyChanged;
        }

        void master_OnBodyChanged(CharacterBody body)
        {
            if (!body)
            {
                aiStateMachine.SetState(new IdleState());
                return;
            }
            teamComponent = body.CommonComponents.TeamComponent;
        }

        //todo bobot share with the class and ask if this is cursed lol
        public Vector3 CurrentTargetPosition =>
            CurrentTargetBody != null 
                ? CurrentTargetBody.corePosition 
                : noTargetOverrideInterestPoint 
                        ? noTargetOverrideInterestPoint.position 
                        : master.CurrentBody 
                            ? master.CurrentBody.transform.position + master.CurrentBody.transform.forward 
                            : transform.position + transform.forward;

        protected virtual void FixedUpdate()
        {
            if(master.CurrentBody &&  aiStateMachine.CurrentState is IdleState)
            {
                aiStateMachine.SetState(new Search { Brain = this });
            }
            AIInputController.CurrentAimPosition = CurrentTargetPosition;
        }

        public virtual AIGestureBehavior RollGesture()
        {
            return gestureBehaviors[UnityEngine.Random.Range(0, gestureBehaviors.Length)].GetBehavior() as AIGestureBehavior;
        }

        public virtual CharacterBody SearchForTarget()
        {
            var index = teamComponent? teamComponent.TeamIndex : TeamIndex.NEUTRAL;
            return CharacterBodyTracker.FindBodyByTeam(gameObject, index, teamTargetType, searchDistance * searchDistance);
            
        }
    }
}
