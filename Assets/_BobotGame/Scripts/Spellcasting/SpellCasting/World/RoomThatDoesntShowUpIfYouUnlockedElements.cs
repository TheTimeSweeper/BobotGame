using UnityEngine;
namespace SpellCasting.World
{
    [CreateAssetMenu(menuName = "SpellCasting/UniqueRoom/RoomThatDoesntShowUpIfYouUnlockedElements", fileName = "UniqueRoom")]
    public class RoomThatDoesntShowUpIfYouUnlockedElements : UniqueRoom
    {
        [SerializeField]
        private ElementType elementThatTheRoomDoesntShowUpIfYouUnlock;

        public override bool CanSpawn()
        {
            return base.CanSpawn() && !MainGame.Instance.SavedData.UnlockedElements.Contains(elementThatTheRoomDoesntShowUpIfYouUnlock);
        }
    }
}