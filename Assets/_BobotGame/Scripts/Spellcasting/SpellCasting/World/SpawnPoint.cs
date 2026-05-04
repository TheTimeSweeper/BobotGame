using UnityEngine;

namespace SpellCasting.World
{
    public class SpawnPoint : InteractionHandler
    {
        [SerializeField]
        private SpawnTable spawnTable;

        public override bool OnBodyDetected(CharacterBody body, bool interactInputPressed)
        {
            spawnTable.SpawnObject(transform.position);
            Destroy(gameObject);
            return true;
        }
    }
}