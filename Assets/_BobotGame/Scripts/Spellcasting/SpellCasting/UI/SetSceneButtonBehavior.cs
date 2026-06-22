using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpellCasting.UI
{
    [CreateAssetMenu(menuName = "BobotGame/ButtonBehavior/SetScene", fileName = "butSetScene_")]
    public class SetSceneButtonBehavior : ButtonBehavior
    {
        [SerializeField]
        int sceneIndex;

        public override void OnButtonClick()
        {
            SceneManager.LoadScene(sceneIndex); 
        }
    }
}