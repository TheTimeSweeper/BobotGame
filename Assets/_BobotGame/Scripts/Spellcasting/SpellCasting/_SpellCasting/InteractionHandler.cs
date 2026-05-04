using UnityEngine;

namespace SpellCasting
{
    public abstract class InteractionHandler : MonoBehaviour
    {
        public abstract bool OnBodyDetected(CharacterBody body, bool pressed);
    }
}
