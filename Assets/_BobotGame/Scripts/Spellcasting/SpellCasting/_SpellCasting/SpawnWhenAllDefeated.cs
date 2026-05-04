using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class SpawnWhenAllDefeated : InteractionHandler
    {
        [SerializeField]
        private SpawnTable enemySpawnTable;

        [SerializeField]
        private Transform[] spawnPoints;

        [SerializeField]
        private SpawnTable ResultTable;

        [SerializeField]
        public List<GameObject> Friends;

        [SerializeField]
        private bool requireInput;

        private bool _spawned = false;

        public override bool OnBodyDetected(CharacterBody body, bool pressed)
        {
            if (_spawned)
                return true;

            if (pressed || !requireInput)
            {
                Commence();
                return true;
            }

            return false;
        }

        private void Commence()
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Friends.Add(enemySpawnTable.SpawnObject(spawnPoints[i].position));
            }

            _spawned = true;
        }

        void Update()
        {
            if (!_spawned)
                return;

            bool anyAlive = false;
            for (int i = 0; i < Friends.Count; i++)
            {
                if (Friends[i] != null)
                {
                    anyAlive = true;
                    break;
                }
            }
            if (!anyAlive)
            {
                ResultTable.SpawnObject(transform.position);
                Destroy(gameObject);
            }
        }
    }
}