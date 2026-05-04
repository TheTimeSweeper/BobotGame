using System;
using UnityEngine;

namespace SpellCasting
{ 
    [CreateAssetMenu(menuName = "SpellCasting/UpgradePath", fileName = "UpgradePath")]
    public class UpgradePathInfo : ScriptableObject
    {
        [Serializable]
        public class UpgradeRelations
        {
            public SkillInfo baseSKill;
            public SkillInfo[] upgrades;
        }

        public UpgradeRelations[] branches;
    }
}