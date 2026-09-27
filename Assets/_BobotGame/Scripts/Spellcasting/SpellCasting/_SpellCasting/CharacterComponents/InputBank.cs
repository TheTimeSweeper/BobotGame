using SpellCasting.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace SpellCasting
{
    public enum SkillButton
    {
        None = 0,
        PRIMARY,//M1
        BLOCK,//m2
        ABILITY,//shift
        DASH,//space
        CROUCH,//alt
        OTHER = 100,
    }
    public class InputState
    {
        public bool Down;
        public bool JustPressed(object claimant)
        {
            bool success = Down && !pressedClaimants.Contains(claimant);
            if (success)
            {
                pressedClaimants.Add(claimant);
            }
            return success;
        }
        public bool JustReleased(object claimant)
        {
            bool success = !Down && !releasedClaimants.Contains(claimant);
            if (success)
            {
                releasedClaimants.Add(claimant);
            }
            return success;
        }

        public List<object> pressedClaimants = new List<object>();
        public List<object> releasedClaimants = new List<object>();

        public void UpdateInput(bool inputDown)
        {
            if (Down == inputDown)
                return;

            if (inputDown)
            {
                releasedClaimants.Clear();
            }
            else
            {
                pressedClaimants.Clear();
            }

            Down = inputDown;
        }
    }

    public class InputBank : MonoBehaviour
    {
        public InputState Primary = new InputState();
        public InputState Block = new InputState();
        public InputState Dash = new InputState();
        public InputState Crouch = new InputState();
        public InputState Ability = new InputState();
        public InputState E = new InputState();
        /// <summary>
        /// direction relative to forward transform (usually model)
        /// </summary>
        public Vector3 LocalMoveDirection { get; set; }
        /// <summary>
        /// direction relative to aimorigin
        /// </summary>
        public Vector3 AimMoveDirection { get; set; }
        /// <summary>
        /// global
        /// </summary>
        public Vector3 GlobalMoveDirection { get; set; }
        //public InputState[] extraInputStates;
        public List<InputState> OrderedHeldInputs { get; set; } = new List<InputState>();
        public InputState CurrentPrimaryInput
        {
            get
            {
                if (OrderedHeldInputs.Count > 0)
                {
                    return OrderedHeldInputs[0];
                }
                else
                {
                    return null;
                }
            }
        }
        [SerializeField]
        private Transform _aimOrigin;
        public Transform AimOrigin {
            get => _aimOrigin ?? transform;
            set => _aimOrigin = value; 
        }
        public Vector3 AimPoint { get; set; }
        public Vector3 AimDelta { get; set; }
        public Vector3 AimOut
        {
            get
            {
                Vector3 result = AimPoint - AimOrigin.position;
                //result.y = 0; //jam this is hard-coded to top-down
                return result.normalized;
            }
        }


        private Vector3 _gestureLerpPosition;

        private List<InputState> _allInputStates;

        void Start()
        {
            _allInputStates = new List<InputState> { Primary, Block, Dash, Crouch, Ability }; 
            //AllInputStates.AddRange(extraInputStates);
        }

        public Ray GetBodyRayToPoint(Vector3 target)
        {
            return new Ray(AimOrigin.position, target - AimOrigin.position);
        }

        public Ray GetBodyRay()
        {
            return new Ray(AimOrigin.position, AimOut);
        }

        void Update()
        {
            InputState iterState;
            for (int i = 0; i < _allInputStates.Count; i++)
            {
                iterState = _allInputStates[i];
                if ((iterState.Down) && !OrderedHeldInputs.Contains(iterState))
                {
                    OrderedHeldInputs.Add(iterState);
                }
            }

            for (int i = _allInputStates.Count - 1; i >= 0; i--)
            {
                iterState = _allInputStates[i];
                if (!iterState.Down && !iterState.JustReleased(this) && OrderedHeldInputs.Contains(iterState))
                {
                    OrderedHeldInputs.Remove(iterState);
                }
            }
        }
    }
}