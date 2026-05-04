using SpellCasting.UI;

namespace SpellCasting
{
    public class Portal : InteractionHandler
    {
        public override bool OnBodyDetected(CharacterBody body, bool pressed)
        {
            if (pressed)
            {
                ConfirmPopup.Open("Enter the next realm (difficulty increases)", "LFG", "no", LevelProgressionManager.Instance.NextLevel);
                return true;
            }
            return false;
        }
    }
}