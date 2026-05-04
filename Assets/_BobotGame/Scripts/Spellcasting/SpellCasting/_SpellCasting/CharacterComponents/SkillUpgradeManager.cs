using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting.UI
{
    public class SkillUpgradeManager : Singleton<SkillUpgradeManager>
    {
        [SerializeField]
        private SkillIcon baseSkillIcon;

        [SerializeField]
        private GameObject menu;

        [SerializeField]
        private List<SkillIcon> upgradeSkillIcons;
        public void ShowUpgrades(SkillController skillController, SkillSlot skillToUpgrade)
        {
            menu.SetActive(true);

            baseSkillIcon.Init(skillToUpgrade.skillInfo, () => { menu.SetActive(false); });

            List<SkillInfo> upgradedSkills = new List<SkillInfo>();
            for (int i = 0; i < skillController.upgradePath.branches.Length; i++)
            {
                var branch = skillController.upgradePath.branches[i];
                if (branch.baseSKill == skillToUpgrade.skillInfo)
                {
                    upgradedSkills.AddRange(branch.upgrades);
                }
            }
            int iter = 0;
            for (; iter < upgradedSkills.Count; iter++)
            {
                while (upgradeSkillIcons.Count < iter)
                {
                    upgradeSkillIcons.Add(Instantiate(upgradeSkillIcons[0], upgradeSkillIcons[0].transform.parent));
                }
                upgradeSkillIcons[iter].gameObject.SetActive(true);
                int itere = iter;
                upgradeSkillIcons[iter].Init(upgradedSkills[iter], () => {
                    skillController.OverrideSkill(skillToUpgrade, upgradedSkills[itere]);
                    menu.SetActive(false);
                });
            }
            for (; iter < upgradeSkillIcons.Count; iter++)
            {
                upgradeSkillIcons[iter].gameObject.SetActive(false);
            }
        }
    }
}
