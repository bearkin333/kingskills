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
using kingskills.KSkills;
using kingskills.UX;
using System.IO;
using UnityEngine;

namespace kingskills
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("randyknapp.mods.extendeditemdataframework")]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class KingSkills : BaseUnityPlugin
    {
        public const string PluginGUID = "bearking.kingskills";
        public const string PluginName = "King's Skills";
        public const string PluginVersion = "1.0.0";


        Harmony harmony = new Harmony(PluginGUID);
        private ButtonConfig OpenSkillWindowBtn;
        
        // Use this class to add your own localization to the game
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            Jotunn.Logger.LogInfo("King's skills HAS COME!");
            InitConfig();
            InitExtendedItem.Init();
            CommandManager.Instance.AddConsoleCommand(new TestSkillCommand());
            CommandManager.Instance.AddConsoleCommand(new SkillUpdateCommand());
            CommandManager.Instance.AddConsoleCommand(new TestSkillsCommand());
            CommandManager.Instance.AddConsoleCommand(new ResetPerksCommand());
            CommandManager.Instance.AddConsoleCommand(new ResetAscensionsCommand());
            CommandManager.Instance.AddConsoleCommand(new TestExperienceCommand());
            SkillMan.AddSkills();
            Assets.AssetLoader.InitAssets();
            AddInputs();
            harmony.PatchAll();
        }

        private void Update()
        {
            if (ZInput.instance != null)
            {
                if (ZInput.GetButtonDown(OpenSkillWindowBtn.Name))
                {
                    SkillGUIUpdate.ToggleSkillGUI();
                }
            }

            OpenPerks.UpdateOpenPerks();
        }

        private void InitConfig()
        {
            CFG.Init(Config);
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

