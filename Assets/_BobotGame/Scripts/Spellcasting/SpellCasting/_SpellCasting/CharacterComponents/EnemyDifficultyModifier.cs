using UnityEngine;
namespace SpellCasting
{
    public class EnemyDifficultyModifier : MonoBehaviour {

        [SerializeField]
        private CharacterBody charBod;
        [SerializeField]
        private BuffInfo enemyDifficultyBuff;

        void Start()
        {
            for (int i = 0; i < LevelProgressionManager.DifficultyProgression; i++)
            {
                charBod.AddBuff(enemyDifficultyBuff);
            }
        }
    }
}