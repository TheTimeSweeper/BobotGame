using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public abstract class InputController : MonoBehaviour
    {
        [SerializeField]
        protected InputBank inputBank;

        [SerializeField]
        protected Transform bodyAimOriginPosition;

        [SerializeField]
        protected Transform forwardDirectionTransform;

        public Transform forwardDirectionReferenceTransform => forwardDirectionTransform ? forwardDirectionTransform : transform;

        [SerializeField]
        protected float maxInputRange;

        private Vector3 _currentAimPosition;
        private Vector3 _lastAimPosition;

        private Vector3 _lastUnrestrictedAimPosition;
        private Vector3 _currentAimPositionDelta;

        private Vector3 _currentGesturePosition;
        private Vector3 _lastGesturePosition;
        private Vector3 _gestureDelta;


        [SerializeField]
        private bool debug;

        private Vector3 _debugLastMousePosition;

        public List<Vector3> deltaEnds = new List<Vector3>();
        public List<Vector3> deltaStarts = new List<Vector3>();

        private int debugstep;

        protected virtual void Awake()
        {
            _currentAimPosition = GetAimPosition();
            _lastAimPosition = _currentAimPosition;

            _currentGesturePosition = GetGesturePosition();
            _lastGesturePosition = _currentGesturePosition;

            if (inputBank)
            {
                inputBank.AimOrigin = bodyAimOriginPosition;
            }
        }

        protected virtual void Update()
        {
            if (inputBank != null)
            {
                UpdateAimPoint();
                UpdateGesturePosition();
                GetGestureDelta();
                //todo bobot figure out aimorigin i'm too tired and just want to raycast
                inputBank.AimOrigin = bodyAimOriginPosition;
                inputBank.AimPoint = _currentAimPosition;
                inputBank.AimDelta = GetAimDelta();
                inputBank.GestureDelta = _gestureDelta;
                inputBank.GesturePosition = _currentGesturePosition;
                //UpdateDebug();

                SetbuttonInputs();
                inputBank.LocalMoveDirection = GetMovementInput();
                inputBank.GlobalMoveDirection = forwardDirectionReferenceTransform.TransformDirection(inputBank.LocalMoveDirection);
                inputBank.AimMoveDirection = bodyAimOriginPosition.InverseTransformDirection(inputBank.GlobalMoveDirection);
            }
        }
        //jam now it's messy
        protected virtual Vector3 GetAimDelta()
        {
            return _currentAimPositionDelta;
        }

        protected virtual Vector3 GetGestureDelta()
        {
            return (_currentGesturePosition - _lastGesturePosition);
        }

        private void UpdateGesturePosition()
        {
            Vector3 mousePosition = GetGesturePosition();
            _currentGesturePosition = mousePosition;

            _gestureDelta = GetGestureDelta();
            _lastGesturePosition = _currentGesturePosition;
        }

        private void UpdateAimPoint()
        {
            _currentAimPosition = GetAimPosition();
            //_currentAimDirection = (_currentAimPosition - _lastAimPosition).normalized;

            //todo this needs to be a rolling average of multiple differences, or not update every single frame
            _currentAimPositionDelta = (_currentAimPosition - _lastUnrestrictedAimPosition).normalized;
            _lastUnrestrictedAimPosition = _currentAimPosition;

            Debug.DrawLine(_currentAimPosition, _lastUnrestrictedAimPosition);
            Util.DebugDrawPoint(_lastUnrestrictedAimPosition, Color.red, 10);
            Util.DebugDrawPoint(_currentAimPosition, Color.magenta, Vector3.right, 5);
            Debug.DrawLine(_currentAimPosition, _currentAimPosition + Vector3.right * 5, Color.magenta);

            //jam this should probably be in input bank or caster
            Vector3 aimDistance = _currentAimPosition - bodyAimOriginPosition.position;
            if (Vector3.Magnitude(aimDistance) > maxInputRange)
            {
                _currentAimPosition = bodyAimOriginPosition.position + aimDistance.normalized * maxInputRange;
            }
            //AimMouseManager.AimMousePosition = cameraController.GetMousePointFromWorld(_currentAimPosition);

            _lastAimPosition = _currentAimPosition;
        }

        protected abstract void SetbuttonInputs();
        protected abstract Vector3 GetGesturePosition();
        protected abstract Vector3 GetAimPosition();
        protected abstract Vector3 GetMovementInput();


        private void UpdateDebug()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                debugstep++;
            }
            if (debugstep < deltaStarts.Count && debugstep > 0)
            {
                Debug.DrawLine(Vector3.zero, deltaEnds[debugstep], Color.red);
                Debug.DrawLine(deltaStarts[debugstep], deltaEnds[debugstep], Color.red);
            }

            for (int i = 0; i < deltaStarts.Count; i++)
            {
                Debug.DrawLine(deltaEnds[i] - Vector3.up, deltaEnds[i], new Color(1, 1, 1, 0.3f));
                Debug.DrawLine(deltaStarts[i], deltaEnds[i], Color.cyan);
            }

            if (Input.GetKey(KeyCode.G))
            {
                deltaStarts.Add(_debugLastMousePosition);
                deltaEnds.Add(inputBank.GesturePosition);
            }

            if (Input.GetKey(KeyCode.H))
            {
                deltaStarts.Clear();
                deltaEnds.Clear();
                debugstep = -1;
            }

            _debugLastMousePosition = inputBank.GesturePosition;
        }
    }
}