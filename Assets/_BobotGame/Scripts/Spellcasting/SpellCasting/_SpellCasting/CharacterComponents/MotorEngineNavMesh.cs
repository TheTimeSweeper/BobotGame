using UnityEngine;

namespace SpellCasting
{
    public class MotorEngineNavMesh : MotorEngine
    {
        [SerializeField]
        private UnityEngine.AI.NavMeshAgent navMeshAgent;

        public override void FixedMove(Vector3 movement)
        {
            throw new System.NotImplementedException();
        }
    }
}
