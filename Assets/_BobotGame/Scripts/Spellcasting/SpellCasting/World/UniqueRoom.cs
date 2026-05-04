using UnityEngine;
namespace SpellCasting.World
{
    [CreateAssetMenu(menuName = "SpellCasting/UniqueRoom/Unspecified", fileName = "UniqueRoom")]
    public class UniqueRoom : ScriptableObject
    {
        [SerializeField]
        public Room room;

        [SerializeField]
        public int maximumInstancesAllowed = 1;

        [SerializeField]
        public int minimumInstancesRequired = 1;

        [SerializeField]
        private int minimumDifficultyRequirement = 0;

        [SerializeField]
        private ElementType[] requiredElements;

        //jam generalize this to a room scriptablobject and make uniqueroom use this function to check instances n stuff n things
        public virtual bool CanSpawn() {

            for (int i = 0; i < requiredElements.Length; i++)
            {
                if (!MainGame.Instance.SavedData.UnlockedElements.Contains(requiredElements[i]))
                {
                    return false;
                }
            }

            return LevelProgressionManager.DifficultyProgression >= minimumDifficultyRequirement;
        }

        void OnValidate()
        {
            minimumInstancesRequired = Mathf.Min(minimumInstancesRequired, maximumInstancesAllowed);
            maximumInstancesAllowed = Mathf.Max(maximumInstancesAllowed, minimumInstancesRequired);
        }
    }
}