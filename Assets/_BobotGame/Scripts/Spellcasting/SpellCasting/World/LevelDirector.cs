using UnityEngine;
namespace SpellCasting.World
{
    public class LevelDirector : MonoBehaviour
    {
        [SerializeField]
        private float BaseCredits;
        private VariableNumberStat Credits; //jam if we have multiple levels an item or some kind of modifier that makes the world bigger would be cool

        void Awake()
        {
            
        }
    }
}