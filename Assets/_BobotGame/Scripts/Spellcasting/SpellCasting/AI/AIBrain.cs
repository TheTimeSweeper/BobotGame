using ActiveStates;
using ActiveStates.AI;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpellCasting.AI
{

    public class AIBrain : MonoBehaviour
    {
        [SerializeField]
        private AIGesture[] gestureBehaviors;

        [SerializeField]
        public AIInputController AIInputController;

        [SerializeField]
        public Transform defaultAimPoint;

        [SerializeField]
        private TeamTargetType teamTargetType = TeamTargetType.OTHER;

        [SerializeField]
        private TeamComponent teamComponent;

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


        public Vector3 CurrentTargetPosition => CurrentTargetBody != null ? CurrentTargetBody.transform.position + Vector3.up* defaultAimPoint.transform.position.y : defaultAimPoint.position;

        protected virtual void FixedUpdate()
        {
            if(aiStateMachine.CurrentState is IdleState)
            {
                aiStateMachine.SetState(new Search { Brain = this });
            }
        }

        public virtual AIGestureBehavior RollGesture()
        {
            return gestureBehaviors[UnityEngine.Random.Range(0, gestureBehaviors.Length)].GetBehavior() as AIGestureBehavior;
        }

        public virtual CharacterBody SearchForTarget()
        {
            return CharacterBodyTracker.FindBodyByTeam(gameObject, teamComponent.TeamIndex, teamTargetType, searchDistance * searchDistance);
            
        }
    }
}
