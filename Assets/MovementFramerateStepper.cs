using UnityEngine;
namespace BobotGame
{
    public class MovementFramerateStepper : MonoBehaviour
    {
        [SerializeField]
        private float frameRate;

        private Vector3 _startingLocalPosition;
        private Vector3 _position;

        private float _timer;

        private void Awake()
        {
            _startingLocalPosition = transform.localPosition;
        }

        private void LateUpdate()
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                transform.localPosition = _startingLocalPosition;
                _position = transform.position;

                while (_timer < 0)
                {

                    _timer += 1f / frameRate;
                }
            }
            transform.position = _position;
        }
    }
}
