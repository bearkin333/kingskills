using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Jotunn.Managers;

namespace kingskills
{
    public static class SkillMan
    {
        //public static Skills.SkillType TestSkillType = 0;

        public static Skills.SkillType Cooking = 0;
        public static Skills.SkillType Sailing = 0;
        public static Skills.SkillType Building = 0;
        public static Skills.SkillType Agriculture = 0;

        public static void AddSkills()
        {
            /* Goodbye, bear skill. Your service is no longer needed
            Jotunn.Configs.SkillConfig skill = new Jotunn.Configs.SkillConfig();
            skill.Identifier = "bearking.kingskills.bearskill";
            skill.Name = "Bear";
            skill.Description = "Become good at bearing";
            skill.IncreaseStep = 1f;
            TestSkillType = SkillManager.Instance.AddSkill(skill);
            */

            SkillManager.Instance.AddSkillsFromJson("kingskills/Assets/skills.json");
            Cooking = SkillManager.Instance.GetSkill("bearking.kingskills.cooking").m_skill;
            Sailing = SkillManager.Instance.GetSkill("bearking.kingskills.sailing").m_skill;
            Building = SkillManager.Instance.GetSkill("bearking.kingskills.building").m_skill;
            Agriculture = SkillManager.Instance.GetSkill("bearking.kingskills.agriculture").m_skill;

            ConfigManager.InitSkillActiveDict();
        }

        public static void UnDeepFryIcons(Skills skills)
        {
            foreach (Skills.SkillDef skillDef in skills.m_skills)
            {
                switch (skillDef.m_skill)
                {
                    case Skills.SkillType.Axes:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Axes"];
                        break;
                    case Skills.SkillType.Blocking:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Blocking"];
                        break;
                    case Skills.SkillType.Bows:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Bows"];
                        break;
                    case Skills.SkillType.Clubs:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Clubs"];
                        break;
                    case Skills.SkillType.Jump:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Jump"];
                        break;
                    case Skills.SkillType.Knives:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Knives"];
                        break;
                    case Skills.SkillType.Pickaxes:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Mining"];
                        break;
                    case Skills.SkillType.Polearms:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Polearms"];
                        break;
                    case Skills.SkillType.Run:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Run"];
                        break;
                    case Skills.SkillType.Sneak:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Sneak"];
                        break;
                    case Skills.SkillType.Spears:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Spears"];
                        break;
                    case Skills.SkillType.Swim:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Swim"];
                        break;
                    case Skills.SkillType.Swords:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Swords"];
                        break;
                    case Skills.SkillType.Unarmed:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Fists"];
                        break;
                    case Skills.SkillType.WoodCutting:
                        skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Woodcutting"];
                        break;
                }

                if (skillDef.m_skill == Agriculture)
                    skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Agriculture"];

                else if (skillDef.m_skill == Building)
                    skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Building"];

                else if (skillDef.m_skill == Cooking)
                    skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Cooking"];

                else if (skillDef.m_skill == Sailing)
                    skillDef.m_icon = Assets.AssetLoader.skillIconSprites["Sailing"];
            }
        }
    }

    [HarmonyPatch(typeof(Skills))]
    public static class SkillLoadHook
    {
        [HarmonyPatch(nameof(Skills.Load))]
        [HarmonyPostfix]
        public static void LoadUnDeepFry(Skills __instance)
        {
            //Whenever skills are loaded, this will load in the new icons for them
            SkillMan.UnDeepFryIcons(__instance);
        }
    }
}
