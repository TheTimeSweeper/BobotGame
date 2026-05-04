using UnityEngine;

namespace SpellCasting
{
    public class RandomSpawner : MonoBehaviour
    {
        public SpawnTable enemySpawns;

        public float range = 10;
        public float offset = 10;

        public float timeMin = 1;
        public float timeMax = 0.2f;

        float timspeed = 1;
        float nextTime;
        float tim;
        private void FixedUpdate()
        {
            tim -= Time.deltaTime * timspeed;
            if(tim <= 0)
            {
                Spawn();
                tim = Random.Range(timeMin, timeMax);
                timspeed += 0.01f;
            }
        }

        private void Spawn()
        {
            Vector3 pos = Random.onUnitSphere * range;
            pos.y = 0;
            pos += pos.normalized * offset;

            enemySpawns.SpawnObject(transform.position + pos);
        }
    }
}