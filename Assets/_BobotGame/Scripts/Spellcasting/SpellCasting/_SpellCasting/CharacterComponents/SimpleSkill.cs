using ActiveStates;
using UnityEngine;

namespace SpellCasting
{
    [CreateAssetMenu(menuName = "SpellCasting/SimpleSkill/Default", fileName = "Skill")]
    public class SimpleSkill : ScriptableObject
    {

        [SerializeField]
        public SerializableActiveState state;

        [SerializeField]
        public string stateMachine = "weapon";

        [SerializeField]
        public InterruptPriority priority = InterruptPriority.STATE_LOW;
    }
}