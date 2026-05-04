using System;
using UnityEngine;

namespace SpellCasting
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera currentCamera;

        [SerializeField]
        private Transform cameraX;
        public Transform CameraX => cameraX;

        [SerializeField]
        private Transform cameraY;
        public Transform CameraY => cameraY;

        [SerializeField]
        private Transform cameraPoint;
        public Transform CameraPoint => cameraPoint;

        [SerializeField]
        private Transform aimOrigin;
        public Transform AimOrigin => aimOrigin;

        public Vector3 GetAimPointFromMouse(Vector3 mousePosition)
        {
            var ray = currentCamera.ScreenPointToRay(mousePosition);
            return ray.GetPoint((cameraPoint.position.y - aimOrigin.position.y) / Mathf.Cos(Vector3.Angle(ray.direction, Vector3.down) * Mathf.Deg2Rad));
        }

        public Vector3 GetMousePointFromWorld() => GetMousePointFromWorld(aimOrigin.transform.position);
        public Vector3 GetMousePointFromWorld(Vector3 position)
        {
            return currentCamera.WorldToScreenPoint(position);
        }

        void OnDestroy()
        {
            //if (currentCamera != null)
            //{
            //    currentCamera.transform.parent = null;
            //}
        }
    }
}