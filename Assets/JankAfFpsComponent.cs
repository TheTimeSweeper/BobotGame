using System;
using UnityEngine;
namespace BobotGame
{
    
    public class JankAfFpsComponent : MonoBehaviour
    {
        [SerializeField]
        private float frameRate;

        [SerializeField]
        private Transform rootBone;

        [SerializeField]
        private Transform[] transforms;
        
        private Vector3[] _positions;
        private Quaternion[] _rotations;

        private float _timer;

        private void OnValidate()
        {
            if (rootBone && transforms.Length == 0)
            {
                transforms = rootBone.GetComponentsInChildren<Transform>();
            }
        }
        private void Awake()
        {
            _positions = new Vector3[transforms.Length];
            _rotations = new Quaternion[transforms.Length];
            UpdateBonePositions();
        }

        private void LateUpdate()
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                UpdateBonePositions();

                while(_timer < 0) { 
                
                    _timer += 1f / frameRate;
                }
            }
            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].localPosition = _positions[i];
                transforms[i].localRotation = _rotations[i];
            }
        }

        private void UpdateBonePositions()
        {
            for (int i = 0; i < transforms.Length; i++)
            {
                _positions[i] = transforms[i].localPosition;
                _rotations[i] = transforms[i].localRotation;
            }
        }
    }
}
