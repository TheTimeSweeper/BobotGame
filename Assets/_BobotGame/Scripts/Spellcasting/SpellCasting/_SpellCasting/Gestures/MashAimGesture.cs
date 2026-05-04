using System;
using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/GestureType/Mash", fileName = "GestureMash")]
    public class MashAimGesture : AimGesture
    {
        [SerializeField]
        public int requiredMashes;

        [SerializeField]
        public float resetTime = 1;

        [SerializeField]
        public float _movedThreshold;

        public override ScriptableObjectBehavior GetBehavior()
        {
            return new MashGestureBehavior { infoObject = this };
        }

        public class MashGestureBehavior : GestureBehavior<MashAimGesture>
        {
            private int _mashes;
            private float _nextResetTime;

            private Vector3 _initialInput;

            public override bool QualifyGesture(InputBank bank, InputState inputState)
            {
                if (Time.time > _nextResetTime)
                {
                    _mashes = 0;
                    _initialInput = Vector3.zero;
                }

                if (inputState.JustPressed(this))
                {
                    _mashes++;
                    _nextResetTime = Time.time + InfoObject.resetTime;
                    if (_initialInput == default)
                    {
                        _initialInput = bank.GesturePosition;
                    }
                }

                return _mashes >= InfoObject.requiredMashes && (bank.GesturePosition - _initialInput).magnitude < InfoObject._movedThreshold;
            }

            public override void ResetGesture()
            {
                _mashes = 0;
                _initialInput = default;
            }
        }
    }
}