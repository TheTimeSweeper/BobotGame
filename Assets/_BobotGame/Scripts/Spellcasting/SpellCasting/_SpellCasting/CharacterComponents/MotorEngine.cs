using UnityEngine;

namespace SpellCasting
{
    public abstract class MotorEngine : MonoBehaviour
    {
        public abstract void FixedMove(Vector3 movement);
        public virtual void Teleport(Vector3 destination) {
            transform.position = destination;
        }
    }
}
