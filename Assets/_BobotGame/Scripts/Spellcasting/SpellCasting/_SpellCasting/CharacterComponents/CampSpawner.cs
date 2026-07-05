using UnityEngine;

namespace SpellCasting
{
    public class CampSpawner : MonoBehaviour
    {
        public MasterTable spawnTable;

        public TeamIndex TeamIndex;

        public int spawnCount;
        public Transform[] spawnPoints;

        private void Start()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                int eye = i % spawnPoints.Length;
                CharacterMaster newMaster = spawnTable.SpawnObject(spawnPoints[eye].position);
                newMaster.CachedTeamIndex = TeamIndex;
            }
        }
    }
}
