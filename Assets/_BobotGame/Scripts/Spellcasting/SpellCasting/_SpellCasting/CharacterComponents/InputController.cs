using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public abstract class InputController : MonoBehaviour
    {
        [SerializeField, Tooltip("used to autofill the inputbank and bodyAimOriginPosition fields")]//todo bobot watadahek
        protected CharacterMaster optionalMaster;

        [Header("Manual (no master or override)")]
        [SerializeField]
        protected InputBank inputBank;
        [SerializeField]
        protected Transform bodyAimOriginPosition;
        [SerializeField, Tooltip("Defaults to model transform")]
        protected Transform forwardDirectionTransform;

        public Transform forwardDirectionReferenceTransform => forwardDirectionTransform ? forwardDirectionTransform : transform;
        [Header("Parameters")]
        [SerializeField]
        protected float maxInputRange;

        private Vector3 _currentAimPosition;
        private Vector3 _lastAimPosition;

        private Vector3 _lastUnrestrictedAimPosition;
        private Vector3 _currentAimPositionDelta;


        protected virtual void Awake()
        {
            _currentAimPosition = GetAimPosition();
            _lastAimPosition = _currentAimPosition;

            if (optionalMaster)
            {
                SubscribeToEvents(true);
                if (optionalMaster.CurrentBody)
                {
                    if (!inputBank) inputBank = optionalMaster.CurrentBody.CommonComponents.InputBank;
                    if (!bodyAimOriginPosition && inputBank) bodyAimOriginPosition = inputBank.AimOrigin;
                    if (!forwardDirectionTransform) forwardDirectionTransform = optionalMaster.CurrentBody.CommonComponents.CharacterModel.transform;
                }
            }

            if (inputBank)
            {
                bodyAimOriginPosition = inputBank.AimOrigin;
                inputBank.AimOrigin = bodyAimOriginPosition;
            }
        }
        bool subscribed = false;
        private void SubscribeToEvents(bool shouldSubscribe)
        {
            if(shouldSubscribe == subscribed)
                return;
            subscribed = shouldSubscribe;

            if (optionalMaster)
            {
                if (shouldSubscribe)
                {
                    optionalMaster.OnBodyChanged += OptionalMaster_OnBodyChanged;
                }
                else
                {
                    optionalMaster.OnBodyChanged -= OptionalMaster_OnBodyChanged;
                }
            }
        }

        private void OnDestroy()
        {
            SubscribeToEvents(false);
        }

        private void OptionalMaster_OnBodyChanged(CharacterBody obj)
        {
            Awake();
        }

        protected virtual void Update()
        {
            if (inputBank != null)
            {
                UpdateAimPoint();
                //todo bobot figure out aimorigin i'm too tired and just want to raycast
                inputBank.AimOrigin = bodyAimOriginPosition;
                inputBank.AimPoint = _currentAimPosition;
                inputBank.AimDelta = GetAimDelta();
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
        protected abstract Vector3 GetAimPosition();
        protected abstract Vector3 GetMovementInput();

    }
}