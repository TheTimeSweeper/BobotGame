using UnityEngine;

namespace SpellCasting
{
    public class PowerItemPickupInteraction : InteractionHandler
    {
        [SerializeField]
        private PowerItem item;

        [SerializeField]
        private int soundIndex = 24;

        public override bool OnBodyDetected(CharacterBody body, bool interactInputPressed)
        {
            EffectManager.SpawnEffect(EffectIndex.SOUND_FAST, transform.position, null, soundIndex);

            body.GiveItem(item);
            Destroy(gameObject);

            return true;
        }
    }
}
