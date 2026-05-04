using System.Collections.Generic;
using UnityEngine;
namespace SpellCasting
{
    public class DamagingData
    {
        public float DamageValue;
        public float StunFactor;
        public GameObject AttackerObject;
        public CharacterBody AttackerBody;
        public DamageTypeIndex DamageTypeIndex;
        public Vector3 Knockback;
    }
}