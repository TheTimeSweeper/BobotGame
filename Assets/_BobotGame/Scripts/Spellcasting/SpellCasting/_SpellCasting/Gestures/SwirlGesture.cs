using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/GestureType/Swirl", fileName = "GestureSwirl")]
    public class SwirlGesture : AimGesture
    {
        [SerializeField]
        public float angleThreshold = 360;

        public override ScriptableObjectBehavior GetBehavior()
        {
            return new SwirlGestureBehavior { infoObject = this };
        }

        public class SwirlGestureBehavior : GestureBehavior<SwirlGesture>
        { 
            private float _totalAngleSwirled;
            private Vector3 _lastDelta;
            private float _lastDotSign;

            private float _graceTimer = 0.5f;
            private bool _lastQualified;

            private Queue<float> _lastDots = new Queue<float>();

            private List<Vector3> debugPositions = new List<Vector3>();
            private List<Vector3> debugCrosses = new List<Vector3>();

            public override void ResetGesture()
            {
                _totalAngleSwirled = 0;
            }

            public override bool QualifyGesture(InputBank bank, InputState inputState)
            {

                for (int i = 0; i < debugPositions.Count; i++)
                {
                    Debug.DrawLine(debugPositions[i], debugPositions[i] + debugCrosses[i], Color.green);
                }

                if (Input.GetKey(KeyCode.H))
                {
                    debugPositions.Clear();
                    debugCrosses.Clear();
                }

                _graceTimer -= Time.deltaTime;
                if (bank.GestureDelta == Vector3.zero)
                {
                    if (_graceTimer < 0)
                    {
                        return _lastQualified;
                    }
                    else
                    {
                        _totalAngleSwirled = 0;
                        bank.DebugSwirlTotalAngleSwirled = 0;
                        return false;
                    }
                }
                else
                {
                    _graceTimer = 0.5f;
                }

                float dot = Vector3.Dot(bank.GestureDelta, Vector3.Cross(Vector3.forward, _lastDelta));

                _lastDots.Enqueue(dot);
                if (_lastDots.Count > 10)
                {
                    _lastDots.Dequeue();
                }

                dot = _lastDots.Average();

                float angle = Vector3.Angle(_lastDelta, bank.GestureDelta);

                if (Mathf.Sign(dot) == _lastDotSign)
                {
                    _totalAngleSwirled += angle;
                }
                else
                {
                    _totalAngleSwirled = 0;
                }

                //if (Input.GetKey(KeyCode.G))
                //{
                //    Debug.LogWarning($"_lastDelta ({_lastDelta.x.ToString("0.00")}, {_lastDelta.y.ToString("0.00")}), " +
                //        $"delta ({bank.GestureDelta.x.ToString("0.00")}, {bank.GestureDelta.y.ToString("0.00")}), " +
                //        $"cross ({Vector3.Cross(Vector3.forward, _lastDelta).x.ToString("0.00")}, {Vector3.Cross(Vector3.forward, _lastDelta).y.ToString("0.00")})\n" +
                //        $"dot {dot}");
                //}

                //if (Input.GetKey(KeyCode.G))
                //{
                //    debugPositions.Add(bank.GesturePosition - bank.GestureDelta);
                //    debugCrosses.Add(Vector3.Cross(Vector3.forward, _lastDelta));
                //}

                _lastDotSign = Mathf.Sign(dot);
                _lastDelta = bank.GestureDelta;

                bank.DebugSwirlTotalAngleSwirled = _totalAngleSwirled;


                //for (int i = 0; i < debugPositions.Count; i++)
                //{
                //    Debug.DrawLine(deltaEnds[i] - Vector3.up, deltaEnds[i], new Color(1, 1, 1, 0.3f));
                //    Debug.DrawLine(debugPositions[i], debugPositions[i]+ debugDeltas[i], Color.green);
                //}

                //if (Input.GetKey(KeyCode.G))
                //{
                //    debugPositions.Add(_debugLastMousePosition);
                //    deltaEnds.Add(inputBank.GesturePosition);
                //}

                //if (Input.GetKey(KeyCode.H))
                //{
                //    debugPositions.Clear();
                //    deltaEnds.Clear();
                //    debugstep = -1;
                //}



                if (_totalAngleSwirled > InfoObject.angleThreshold)
                {
                    _lastQualified = true;
                }
                else
                {
                    _lastQualified = false;
                }
                return _lastQualified;
            }
        }
    }
}