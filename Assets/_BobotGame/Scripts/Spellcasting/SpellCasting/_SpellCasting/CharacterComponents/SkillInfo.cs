using ActiveStates;
using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/SkillInfo/Default", fileName = "skNewSkill")]
    public class SkillInfo : ScriptableObject
    {
        public string displayName;
        public float baseCooldown;
        public SerializableActiveState state;
        public string stateMachineName;
        public InterruptPriority interruptingPriority;
        public bool autoCast;

    }
}