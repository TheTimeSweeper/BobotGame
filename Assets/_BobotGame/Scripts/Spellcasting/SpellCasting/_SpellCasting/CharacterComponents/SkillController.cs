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

        private void Awake()
        {
        }
        private void Start()
        {
            for (int i = 0; i < skillSlots.Count; i++)
            {
                skillSlots[i].Init(commonComponents);
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < skillSlots.Count; i++)
            {
                skillSlots[i].Recharge();
            }
        }

        internal void OverrideSkill(SkillSlot skillSlot, SkillInfo skillInfo)
        {
            skillSlot.skillInfo = skillInfo;
            skillSlot.Init(commonComponents);
        }
    }
}