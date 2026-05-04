using System;
namespace SpellCasting
{
    [Flags]
    public enum DamageTypeIndex : uint
    {
        NONE = 0u,
        FREEZE = 1u,
        STUN = 2u,
        FIRE = 4u,
        EARTH = 8u,
        AIR = 16u,
        WATER = 32u,
        BIGSTUN = 64u,
    }
}