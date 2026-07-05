using UnityEngine;

namespace SpellCasting
{
    public class EnsureGroundOnAwake : MonoBehaviour
    {
        public enum Prefer
        {
            HIGHEST = 10,
            CLOSEST = 20,
        }
        public float checkHeight;
        public Prefer prefer = Prefer.HIGHEST;

        private void Awake()
        {
            float highestY = float.NegativeInfinity;
            float closestDistance = float.PositiveInfinity;
            float closestY = 0;
            RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * checkHeight, Vector3.down, checkHeight*2, LayerInfo.Default.layerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].point.y > highestY)
                {
                    highestY = hits[i].point.y;
                }
                float yDist = (hits[i].point - transform.position).y;
                if (Mathf.Abs(yDist) < Mathf.Abs(closestDistance))
                {
                    closestDistance = yDist;
                    closestY = hits[i].point.y;
                }
            }

            switch (prefer)
            {
                case Prefer.CLOSEST:
                    if (closestDistance != float.PositiveInfinity)
                    {
                        transform.position = new Vector3(transform.position.x, closestY, transform.position.z);
                    }

                    break;
                case Prefer.HIGHEST:
                    if (highestY != float.NegativeInfinity)
                    {
                        transform.position = new Vector3(transform.position.x, highestY, transform.position.z);
                    }
                    break;
            }
        }
    }
}
