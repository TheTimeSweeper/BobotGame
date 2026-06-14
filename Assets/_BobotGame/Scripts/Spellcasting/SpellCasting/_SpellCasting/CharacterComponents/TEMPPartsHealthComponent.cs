using ActiveStates.Bobots;
using UnityEngine;
namespace SpellCasting
{
    [RequireComponent(typeof(TEMPBobotCrouchController))]
    public class TEMPPartsHealthComponent : HealthComponent
    {
        public float legHealth;
        public float armHealth;

        protected override void TakeTheDamage(ref float healthToHit, GetDamagedData info)
        {
            bool? attackerCrouched = false;
            if (info.DamagingInfo.AttackerObject.TryGetComponent<TEMPBobotCrouchController>(out var attackerCrouchcontroller))
            {
                attackerCrouched = attackerCrouchcontroller.isCrouched;
            }

            //always true
            if (TryGetComponent<TEMPBobotCrouchController>(out var victimCrouchController))
            {
                bool shouldBlock = victimCrouchController.isBlocking;
                //if incoming attack has no upper/lower specified, you can block from either position
                if (!attackerCrouched.HasValue)
                {
                    shouldBlock = true;
                }
                //if it does, block it if you both have the same upper/lower value
                //todo bobot: rename crouched to upper/lower, also probably have an enum, yeah this sucks ass but it gonna  just wo-ork
                else
                {
                    shouldBlock |= attackerCrouched.Value == victimCrouchController.isCrouched;
                }

                if (shouldBlock && armHealth > 0)//todo bobot: can early out of all this logic if we didn't have arm health anyways. // well maybe not because we can have blocking still be detected for something
                {
                    TakeTheDamage(ref armHealth, info);
                    return;
                }
            }
            //no blocking. if damage is unspecified, or enemy is not crouched, we take "core" health damage
            if (!attackerCrouched.HasValue || attackerCrouched.Value == false)
            {
                base.TakeTheDamage(ref health, info);
            }
            //no blocking. if the enemy was crouched, we take leg damage
            else
            {
                base.TakeTheDamage(ref legHealth, info);
            }
        }
    }
}