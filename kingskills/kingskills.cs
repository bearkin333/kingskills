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
using kingskills.Perks;
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
        public const string PluginVersion = "1.0.1";


        Harmony harmony = new Harmony(PluginGUID);
        public ButtonConfig OpenSkillWindowBtn;
        public ButtonConfig CollapseFoodBtn;
        public ButtonConfig ConfirmShortcutBtn;
        public ButtonConfig ExitSkillGUIBtn;
        public ButtonConfig DetailedPerkBtn;

        // Use this class to add your own localization to the game
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            Jotunn.Logger.LogInfo("King's skills HAS COME!");
            InitConfig();
            InitExtendedItem.Init();
            InitCommands();
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
                    SkillGUIUpdate.ToggleSkillGUI();

                if (ZInput.GetButtonDown(CollapseFoodBtn.Name))
                    QuickCombineGUI.CollapseHoveredFood();

                if (ZInput.GetButtonDown(ExitSkillGUIBtn.Name))
                    SkillGUIUpdate.CloseSkillGUI();

                CombineGUI.CombineUpdate(ConfirmShortcutBtn.Name);
                OpenPerks.UpdateOpenPerks(DetailedPerkBtn.Name);
            }

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
                Config = CFG.KeyBindingSkillGUI,
                ActiveInCustomGUI = true,
                ActiveInGUI = true
            };
            InputManager.Instance.AddButton(PluginGUID, OpenSkillWindowBtn);
            CollapseFoodBtn = new ButtonConfig
            {
                Name = "CollapseFood",
                Config = CFG.KeyBindingCollapseFood,
                ActiveInCustomGUI = true,
                ActiveInGUI = true
            };
            InputManager.Instance.AddButton(PluginGUID, CollapseFoodBtn);
            ConfirmShortcutBtn = new ButtonConfig
            {
                Name = "ConfirmShortcut",
                Config = CFG.KeyBindingConfirmShortcut,
                ActiveInCustomGUI = true,
                ActiveInGUI = true
            };
            InputManager.Instance.AddButton(PluginGUID, ConfirmShortcutBtn);
            ExitSkillGUIBtn = new ButtonConfig
            {
                Name = "ExitShortcut",
                Config = CFG.KeyBindingSkillGUIExit,
                ActiveInCustomGUI = true,
                ActiveInGUI = true
            };
            InputManager.Instance.AddButton(PluginGUID, ExitSkillGUIBtn);
            DetailedPerkBtn = new ButtonConfig
            {
                Name = "DetailedPerkHover",
                Config = CFG.KeyBindingDetailedPerkTooltip,
                ActiveInCustomGUI = true,
                ActiveInGUI = true
            };
            InputManager.Instance.AddButton(PluginGUID, DetailedPerkBtn);
        }

        private void InitCommands()
        {
            CommandManager.Instance.AddConsoleCommand(new TestSkillCommand());
            CommandManager.Instance.AddConsoleCommand(new SkillUpdateCommand());
            CommandManager.Instance.AddConsoleCommand(new TestSkillsCommand());
            CommandManager.Instance.AddConsoleCommand(new ResetPerksCommand());
            CommandManager.Instance.AddConsoleCommand(new ResetAscensionsCommand());
            CommandManager.Instance.AddConsoleCommand(new TestExperienceCommand());
        }
    }
}

