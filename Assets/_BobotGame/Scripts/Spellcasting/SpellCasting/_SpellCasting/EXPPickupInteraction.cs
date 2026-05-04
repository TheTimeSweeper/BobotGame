using UnityEngine;

namespace SpellCasting
{
    public class EXPPickupInteraction : InteractionHandler
    {
        [SerializeField]
        private float expAmount = 1;

        [SerializeField]
        private float speed;
        [SerializeField]
        private float contactRange;

        private CharacterBody _foundBody;

        public override bool OnBodyDetected(CharacterBody body, bool pressed)
        {
            _foundBody = body;
            return true;
        }

        private void FixedUpdate()
        {
            if (!_foundBody)
                return;

            Vector3 difference = (_foundBody.transform.position - transform.position);
            if(difference.sqrMagnitude <= contactRange * contactRange)
            {
               GrantExp();
                return;
            }

            transform.Translate(speed * Time.fixedDeltaTime * difference.normalized);
            speed += Time.fixedDeltaTime; //yes I know this isn't properly frame independent

        }

        private void GrantExp()
        {
            _foundBody.CommonComponents.SkillController.GainExp(expAmount);
            EffectManager.SpawnEffect(EffectIndex.DAMAGENUMBER, _foundBody.transform.position, null, (int)expAmount);
            Destroy(gameObject);
        }
    }
}
