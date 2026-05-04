using UnityEngine;

namespace SpellCasting
{
    public class MotorEngineRigidBody : MotorEngine
    {
        [SerializeField]
        private Rigidbody rigidBody;

        public override void FixedMove(Vector3 movement)
        {
            Vector3 newVelocity = movement / Time.fixedDeltaTime;
            newVelocity.y = rigidBody.linearVelocity.y;
            rigidBody.linearVelocity = newVelocity;
            //rigidBody.MovePosition(transform.position + movement);
        }
    }
}
