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

        public List<SkillSlot> skillSlots = new List<SkillSlot>();
                                          //todo bobot ew;
        public SkillSlot PrimarySkill => skillSlots.Find((slot) => slot.skillButton == SkillButton.PRIMARY);
        public SkillSlot BlockSkill => skillSlots.Find((slot) => slot.skillButton == SkillButton.BLOCK);
        public SkillSlot CrouchSkill => skillSlots.Find((slot) => slot.skillButton == SkillButton.CROUCH);
        public SkillSlot DashSkill => skillSlots.Find((slot) => slot.skillButton == SkillButton.DASH);
        public SkillSlot HeavySkill => skillSlots.Find((slot) => slot.skillButton == SkillButton.HEAVY);

        public UpgradePathInfo upgradePath;

        //moofa proto soc ree
        private float exp;
        private float nextLevel = 40;
        public int level { get; set; } = 1;

        private List<SkillSlot> allSkills;
        private void Awake()
        {
            allSkills = new List<SkillSlot> { /*m1Skill, m2Skill, spaceSkill*/ };
            allSkills.AddRange(skillSlots);
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
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < allSkills.Count; i++)
            {
                allSkills[i].Recharge();
            }
        }

        internal void OverrideSkill(SkillSlot skillSlot, SkillInfo skillInfo)
        {
            skillSlot.skillInfo = skillInfo;
            skillSlot.Init(commonComponents);
        }
    }
}