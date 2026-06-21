using ActiveStates.Bobots;
using UnityEngine;
namespace SpellCasting
{
    [RequireComponent(typeof(TEMPBobotCrouchController))]
    public class TEMPPartsHealthComponent : HealthComponent
    {
        public float legHealth;
        public float armHealth;

        public Transform chestPosition;
        public Transform legsPosition;
        public Transform armsPosition;

        protected override void JudgeTheDamage(GetDamagedData info)
        {
            bool? attackerCrouched = false;
            if (info.DamagingInfo.AttackerObject.TryGetComponent<TEMPBobotCrouchController>(out var attackerCrouchcontroller))
            {
                attackerCrouched = attackerCrouchcontroller.isCrouched;
            }

            //always true
            if (TryGetComponent<TEMPBobotCrouchController>(out var victimCrouchController) && victimCrouchController.isBlocking)
            {
                bool shouldBlock = victimCrouchController.isBlocking;
                //if incoming attack has no elevation specified, you can block from either position
                if (!attackerCrouched.HasValue)
                {
                    shouldBlock = true;
                }
                //if it does elevation specified
                //todo bobot: rename crouched to upper/lower, also probably have an enum, yeah this sucks ass but it gonna  just wo-ork
                else
                {
                    //both bots are at the same elevation, block
                    if (attackerCrouched.Value == victimCrouchController.isCrouched)
                    {
                        shouldBlock = true;
                    }
                    //we blocked the wrong elevation, take damage
                    else
                    {
                        shouldBlock = false;
                    }
                }

                if (shouldBlock && armHealth > 0)//todo bobot: can early out of all this logic if we didn't have arm health anyways. // well maybe not because we can have blocking still be detected for something
                {
                    info.DamagingInfo.DamageValue /= 2;
                    info.DamagingInfo.wasBlocked = true;
                    TakeTheDamage(ref armHealth, info);
                    SpawnEffect(info, 2, armHealth < 0);
                    if (armHealth < 0)
                    {
                        Debug.LogWarning("BROKE ARMS!");
                    }
                    return;
                }
            }
            //no blocking. if damage is unspecified, or enemy is at same level as us, we take "core" health damage
            if (!attackerCrouched.HasValue || attackerCrouched.Value == victimCrouchController.isCrouched)
            {
                TakeTheDamage(ref health, info);
                SpawnEffect(info, 0, health < 0);
                if (health < 0)
                {
                    Debug.LogWarning("BROKE HEALTH!");
                }
            }
            //no blocking. enemy at different vertical level
            else
            {
                //we were not crouched, they were attacking crouched, take leg damage
                if (!victimCrouchController.isCrouched)
                {
                    TakeTheDamage(ref legHealth, info);
                    SpawnEffect(info, 1, legHealth < 0);
                    if (legHealth < 0)
                    {
                        Debug.LogWarning("BROKE LEGS!");
                    }
                }
                //we were crouched, they were attacking normal height, still hit our core
                else
                {
                    TakeTheDamage(ref health, info);
                    SpawnEffect(info, 0, health < 0);
                    if (health < 0)
                    {
                        Debug.LogWarning("BROKE HEALTH!");
                    }
                }
            }
        }

        private void SpawnEffect(GetDamagedData info, int place, bool dunGotBroked)
        {
            EffectIndex effect = info.DamagingInfo.AttackerBody.teamIndex == TeamIndex.MONSTER ? EffectIndex.DAMAGENUMBER_FROMENEMY : EffectIndex.DAMAGENUMBER;
            if (dunGotBroked)
            {
                effect = EffectIndex.BREAK;

            }
            Transform positionTransform;
            switch (place)
            {
                default://health
                case 0:
                    positionTransform = chestPosition;
                    break;
                case 1://legs
                    positionTransform = legsPosition;
                    break;
                case 2://arms
                    positionTransform = armsPosition;
                    effect = EffectIndex.DAMAGENUMBER_KILL;
                    break;
            }

            EffectManager.SpawnEffect(effect, positionTransform.position, null, (int)info.DamagingInfo.DamageValue);
        }
    }
}