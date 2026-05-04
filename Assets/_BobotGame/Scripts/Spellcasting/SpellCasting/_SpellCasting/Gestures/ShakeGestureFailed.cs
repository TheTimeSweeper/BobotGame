using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class ShakeGestureFailed : AimGesture
    {

        [SerializeField]
        private int smallAngleMax = 50;
        [SerializeField]
        private int largeAngleMin = 90;

        private Vector3 _lastMouseDelta;

        private int _turns;

        private float _smallAngleSum;

        private bool _lastQualified;

        private float _graceTimer;

        private float _recentLargestDelta;
        private float _recentLargestTimer;

        public override ScriptableObjectBehavior GetBehavior()
        {
            throw new System.NotImplementedException();
        }

        public bool QualifyGesture(InputBank bank, InputState inputState)
        {
            _graceTimer -= Time.deltaTime;

            if (bank.GestureDelta.magnitude < Mathf.Epsilon)
            {
                if (_graceTimer > 0)
                {
                    return _lastQualified;
                }
                else
                {
                    _turns = 0;
                    bank.DebugShakeTurns = _turns;
                    return false;
                }
            }
            else
            {
                _graceTimer = 0.5f;
            }

            float gestureDeltaMagnitude = bank.GestureDelta.magnitude;
            if (gestureDeltaMagnitude > _recentLargestDelta)
            {
                _recentLargestDelta = gestureDeltaMagnitude;
                _recentLargestTimer = 0.1f;
            }
            _recentLargestTimer -= Time.deltaTime;
            if (_recentLargestTimer < 0)
            {
                _recentLargestDelta = 0;
            }

            float angle = Vector3.Angle(_lastMouseDelta, bank.GestureDelta);

            bool checkSmallAngle = false;
            if (bank.GestureDelta.magnitude < _recentLargestDelta * 0.3f)
            {
                _smallAngleSum += angle;
            }
            else
            {
                checkSmallAngle = true;
            }

            if (angle > smallAngleMax && angle < largeAngleMin)
            {
                _turns = 0;
            }

            if (angle > largeAngleMin || (checkSmallAngle && _smallAngleSum > largeAngleMin))
            {
                _turns++;
            }

            _lastMouseDelta = bank.GestureDelta;
            if (checkSmallAngle)
            {
                _smallAngleSum = 0;
            }

            #region debug
            if (Input.GetKey(KeyCode.G))
            {
                Debug.LogWarning(bank.GestureDelta.magnitude.ToString("0.000") + " | " + angle.ToString("0.000") + " | " + _smallAngleSum.ToString("0.000") + "\n");
            }

            bank.DebugShakeTurns = _turns;
            #endregion

            if (_turns >= 2)
            {
                _lastQualified = true;
            }
            _lastQualified = false;

            return _lastQualified;
        }
    }
}