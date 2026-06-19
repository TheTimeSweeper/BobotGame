using UnityEngine;

namespace SpellCasting
{
    public class TransformRoll : MonoBehaviour
    {
        private Vector3 lastposition;
        private float radius;

        void Awake()
        {
            lastposition = transform.position;
            radius = (transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 3;
        }

        void Update()
        {
            if (transform.position != lastposition)
            {
                float distance = (lastposition - transform.position).magnitude;
                float theta = Mathf.Atan(distance / radius) * 57.2958f;
                transform.Rotate(Vector3.Cross(Vector3.up, transform.position - lastposition), theta, Space.World);

                lastposition = transform.position;
            }
        }
    }
}