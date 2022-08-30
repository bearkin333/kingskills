// kingskills
// a Valheim mod skeleton using Jötunn
// 
// File:    kingskills.cs
// Project: kingskills

using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using kingskills.Commands;
using UnityEngine;

namespace kingskills
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class KingSkills : BaseUnityPlugin
    {
        public const string PluginGUID = "bearking.kingskills";
        public const string PluginName = "King's Skills";
        public const string PluginVersion = "1.0.0";

        public static Skills.SkillType TestSkillType = 0;

        Harmony harmony = new Harmony(PluginGUID);
        private ButtonConfig OpenSkillWindowBtn;
        
        // Use this class to add your own localization to the game
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            Jotunn.Logger.LogInfo("King's skills has awakened!");
            InitConfig();
            CommandManager.Instance.AddConsoleCommand(new BearSkillCommand());
            CommandManager.Instance.AddConsoleCommand(new SkillUpdateCommand());
            CommandManager.Instance.AddConsoleCommand(new TestSkillsCommand());
            AddSkills();
            AddInputs();
            harmony.PatchAll();
        }

        private void Update()
        {
            if (ZInput.instance != null)
            {
                if (ZInput.GetButtonDown(OpenSkillWindowBtn.Name))
                {
                    SkillGUI.ToggleSkillGUI();
                }
            }
        }

        private void InitConfig()
        {
            ConfigManager.Init(Config);
        }

        private void AddSkills()
        {
            Jotunn.Configs.SkillConfig skill = new Jotunn.Configs.SkillConfig();
            skill.Identifier = "bearking.kingskills.bearskill";
            skill.Name = "Bear";
            skill.Description = "Become good at bearing";
            skill.IncreaseStep = 1f;

            TestSkillType = SkillManager.Instance.AddSkill(skill);

            Jotunn.Logger.LogMessage(TestSkillType);
        }

        private void AddInputs()
        {
            OpenSkillWindowBtn = new ButtonConfig
            {
                Name = "OpenSkillWindow",
                Key = KeyCode.I,
                ActiveInCustomGUI = true
            };
            InputManager.Instance.AddButton(PluginGUID, OpenSkillWindowBtn);
        }
    }
}

