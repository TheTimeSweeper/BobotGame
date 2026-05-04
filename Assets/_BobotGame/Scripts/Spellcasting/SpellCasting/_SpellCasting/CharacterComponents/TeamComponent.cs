using System;
using UnityEngine;

namespace SpellCasting
{
    [Flags]
    public enum TeamTargetType : uint
    {
        NONE = 0u,
        SELF = 1u,
        OTHER= 2u,
        ALLY= 4u,
    }
    [Flags]
    public enum TeamIndex : uint
    {
        None = 0u,
        NEUTRAL = 1u,
        PLAYER = 2u,
        MONSTER = 4u,
    }

    //jam would like a TeamInfo scriptableobject that has information about what teams it can and cannot hit but this will do for now
    public class TeamComponent : MonoBehaviour
    {
        [SerializeField]
        private TeamIndex teamIndex;
        public TeamIndex TeamIndex { get => teamIndex; set => teamIndex = value; }
    }
}