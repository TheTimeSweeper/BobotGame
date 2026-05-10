using System;
using System.Collections.Generic;
using SpellCasting.UI;
using UnityEngine;
using UnityEngine.Events;

namespace SpellCasting
{
    public class SkillController : MonoBehaviour
    {
        [SerializeField]
        private CommonComponentsHolder commonComponents;

        public SkillSlot primarySkill;//not named for ror2, named for moonfall (which took from ror2 but shut up about that)
        public SkillSlot secondarySkill;
        public SkillSlot utilitySkill;
        public List<SkillSlot> extraSkills = new List<SkillSlot>();

        public UpgradePathInfo upgradePath;

        //moofa proto soc ree
        private float exp;
        private float nextLevel = 40;
        public int level { get; set; } = 1;

        private List<SkillSlot> allSkills;
        private void Awake()
        {
            allSkills = new List<SkillSlot> { primarySkill, secondarySkill, utilitySkill };
            allSkills.AddRange(extraSkills);
        }
        private void Start()
        {
            for (int i = 0; i < allSkills.Count; i++)
            {
                allSkills[i].Init(commonComponents);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SkillUpgradeManager.Instance.ShowUpgrades(this, primarySkill);
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < allSkills.Count; i++)
            {
                allSkills[i].Recharge();
            }
        }

        internal void OverrideSkill(SkillSlot primarySkill, SkillInfo skillInfo)
        {
            primarySkill.skillInfo = skillInfo;
            primarySkill.Init(commonComponents);
        }

        internal void GainExp(float amount)
        {
            exp += amount;
            if (exp >= nextLevel)
            {
                exp -= nextLevel;
                nextLevel *= 4f;
                level++;
                if (level >= 4)
                    return;
                SkillUpgradeManager.Instance.ShowUpgrades(this, primarySkill);
            }
        }
    }
}