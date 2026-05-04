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
        protected Transform aimOriginPosition;

        [SerializeField]
        protected float maxInputRange;

        private Vector3 _currentAimPosition;
        private Vector3 _lastAimPosition;

        private Vector3 _lastUnrestrictedAimPosition;
        private Vector3 _currentAimDirection;

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
        }

        protected virtual void Update()
        {
            if (inputBank != null)
            {
                UpdateAimPoint();
                UpdateGesturePosition();
                GetGestureDelta();
                inputBank.AimPoint = _currentAimPosition;
                inputBank.AimDirection = GetAimDirection();
                inputBank.GestureDelta = _gestureDelta;
                inputBank.GesturePosition = _currentGesturePosition;
                //UpdateDebug();

                SetbuttonInputs();
                inputBank.LocalMoveDirection = GetMovementInput();
                inputBank.GlobalMoveDirection = transform.TransformDirection(inputBank.LocalMoveDirection);
                inputBank.AimMoveDirection = aimOriginPosition.InverseTransformDirection(inputBank.GlobalMoveDirection);
            }
        }
        //jam now it's messy
        protected virtual Vector3 GetAimDirection()
        {
            return _currentAimDirection;
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

            _currentAimDirection = (_currentAimPosition - _lastUnrestrictedAimPosition).normalized;
            _lastUnrestrictedAimPosition = _currentAimPosition;
            Debug.DrawLine(_currentAimPosition, _lastUnrestrictedAimPosition);
            Debug.DrawLine(_lastUnrestrictedAimPosition, _lastUnrestrictedAimPosition + Vector3.up * 10);
            Debug.DrawLine(_currentAimPosition, _currentAimPosition + Vector3.up * 10);

            //jam this should probably be in input bank or caster
            Vector3 aimDistance = _currentAimPosition - aimOriginPosition.position;
            if (Vector3.Magnitude(aimDistance) > maxInputRange)
            {
                _currentAimPosition = aimOriginPosition.position + aimDistance.normalized * maxInputRange;
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