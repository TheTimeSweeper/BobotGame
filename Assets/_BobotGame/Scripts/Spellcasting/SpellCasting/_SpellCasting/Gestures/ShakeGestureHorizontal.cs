using System.Runtime.CompilerServices;
using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/GestureType/Shake", fileName = "GestureShake")]
    public class ShakeGestureHorizontal : AimGesture
    {
        [SerializeField]
        private float maxYGive = 3;

        [SerializeField]
        private int minShakes = 3;

        [SerializeField]
        private float minXTurn = 3;

        public override ScriptableObjectBehavior GetBehavior()
        {
            return new ShakeGestureBehavior { infoObject = this };
        }

        public class ShakeGestureBehavior : GestureBehavior<ShakeGestureHorizontal>
        {
            private Vector3 _lastDelta;
            private Vector3 _lastExtreme;

            private float _graceTimer = 0.5f;

            private int _turns;

            private bool _lastQualified;

            public override void ResetGesture()
            {
                _lastDelta = Vector3.zero;
                _lastExtreme = Vector3.zero;
                _turns = 0;
            }

            public override bool QualifyGesture(InputBank bank, InputState inputState)
            {
                _graceTimer -= Time.deltaTime;

                if (bank.GestureDelta == Vector3.zero)
                {
                    if (_graceTimer < 0)
                    {
                        _turns = 0;
                        bank.DebugShakeTurns = _turns;
                    }
                    else
                    {
                        return _lastQualified;
                    }
                }
                else
                {
                    _graceTimer = 0.5f;
                }

                int direction = bank.GestureDelta.x > 0 ? 1 : -1;

                //is previous direction opposite current direction
                if (_lastDelta.x * -direction > 0)
                {
                    if (Mathf.Abs(bank.GesturePosition.x - _lastExtreme.x) > InfoObject.minXTurn)
                    {
                        _turns++;
                    }
                    _lastExtreme = bank.GesturePosition;
                }

                _lastDelta = bank.GestureDelta;

                if (Mathf.Abs(bank.GesturePosition.y - _lastExtreme.y) > InfoObject.maxYGive)
                {
                    _turns = 0;
                }

                bank.DebugShakeTurns = _turns;

                _lastQualified = _turns >= InfoObject.minShakes;

                return _lastQualified;
            }
        }
    }
}