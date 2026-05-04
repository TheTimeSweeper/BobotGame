using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace SpellCasting
{
    public class LevelProgressionManager : Singleton<LevelProgressionManager>
    {
        public static int DifficultyProgression;

        public static bool resetting;

        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private CharacterBody Joanna;

        protected override void InitOnce()
        {
            base.InitOnce();
            resetting = false;
            DontDestroyOnLoad(gameObject);
            Instantiate(Joanna, spawnPoint.transform.position, Quaternion.identity);
        }

        public override void ReInit()
        {
            base.ReInit();
            CharacterBodyTracker.FindPrimaryPlayer().CommonComponents.FixedMotorDriver.engine.Teleport(spawnPoint.transform.position);
            DifficultyProgression++;
            resetting = false;
        }

        public void NextLevel()
        {

            SceneManager.LoadScene(0);
        }

        public IEnumerator DelayReset()
        {
            yield return new WaitForSeconds(1);
            TrueReset();
        }

        public static void TrueReset()
        {
            //man the end of jam spaghetti
            //DifficultyProgression = 0;
            //resetting = true;
            //SceneManager.MoveGameObjectToScene(Instance.gameObject, SceneManager.GetActiveScene());
            //Instance = null;

            //var player = CharacterBodyTracker.FindPrimaryPlayer();
            //if (player != null)
            //{
            //    SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetActiveScene());
            //}

            //Destroy(Camera.main.gameObject);
            SceneManager.LoadScene(0);
        }

        public static void DelayedTrueReset()
        {
            if (resetting)
                return;

            resetting = true;
            TrueReset();
            //Instance.StartCoroutine(Instance.DelayReset());
        }
    }
}