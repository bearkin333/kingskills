using HarmonyLib;
using Jotunn.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace kingskills.UX
{
    [HarmonyPatch]
    class UpdateGUI
    {
        //how many seconds between GUI update
        public const float updateGUITimer = 2f;

        public static float timeSinceUpdate = 0f;
        public static bool GUIOpen = false;

        [HarmonyPatch(typeof(Player), nameof(Player.FixedUpdate))]
        [HarmonyPrefix]
        public static void FixedUpdateGUI(Player __instance)
        {
            try
            {
                if (!__instance.m_nview) return;
                if (!__instance.m_nview.IsOwner()) return;
                if (!SkillGUI.SkillGUIWindow) return;
                if (!SkillGUI.SkillGUIWindow.activeSelf) return;
            }
            catch
            {
                Jotunn.Logger.LogWarning("Didn't check for GUI Update");
                return;
            }

            timeSinceUpdate += Time.fixedDeltaTime;
            if (timeSinceUpdate >= updateGUITimer)
            {
                timeSinceUpdate -= updateGUITimer;
                //Jotunn.Logger.LogMessage("Updating the values on the GUI");
                GUICheck();
            }
        }

        public static void ToggleSkillGUI()
        {
            if (Player.m_localPlayer == null)
            {
                return;
            }
            if (!SkillGUI.SkillGUIWindow)
            {
                SkillGUI.InitSkillWindow();
            }

            bool state = !SkillGUI.SkillGUIWindow.activeSelf;

            SkillGUI.SkillGUIWindow.SetActive(state);

            GUIManager.BlockInput(state);

            if (state)
            {
                GUICheck();
            }
        }

        public static void StickGUI()
        {
            GUIManager.BlockInput(false);
        }

        public static void OnDropdownValueChange()
        {
            SkillGUI.scroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            GUICheck();
        }

        public static void GUICheck()
        {
            //Jotunn.Logger.LogMessage($"Detected a dropdown value change.");

            Player player = Player.m_localPlayer;
            Skills.SkillType skill = Skills.SkillType.None;

            string skillName = SkillGUI.dd.options[SkillGUI.dd.value].text;
            StatsPatch.UpdateStats(player);

            switch (skillName)
            {
                case "Axes":
                    skill = Skills.SkillType.Axes;
                    OpenAxePanels();
                    OpenPerks.OpenAxePerkBoxes();
                    break;
                case "Blocking":
                    skill = Skills.SkillType.Blocking;
                    OpenBlockPanels();
                    OpenPerks.OpenBlockPerkBoxes();
                    break;
                case "Bows":
                    skill = Skills.SkillType.Bows;
                    OpenBowPanels();
                    OpenPerks.OpenBowPerkBoxes();
                    break;
                case "Clubs":
                    skill = Skills.SkillType.Clubs;
                    OpenClubPanels();
                    OpenPerks.OpenClubPerkBoxes();
                    break;
                case "Fists":
                    skill = Skills.SkillType.Unarmed;
                    OpenFistPanels();
                    OpenPerks.OpenFistPerkBoxes();
                    break;
                case "Jump":
                    skill = Skills.SkillType.Jump;
                    OpenJumpPanels();
                    OpenPerks.OpenJumpPerkBoxes();
                    break;
                case "Knives":
                    skill = Skills.SkillType.Knives;
                    OpenKnifePanels();
                    OpenPerks.OpenKnifePerkBoxes();
                    break;
                case "Mining":
                    skill = Skills.SkillType.Pickaxes;
                    OpenMiningPanels();
                    OpenPerks.OpenMiningPerkBoxes();
                    break;
                case "Polearms":
                    skill = Skills.SkillType.Polearms;
                    OpenPolearmPanels();
                    OpenPerks.OpenPolearmPerkBoxes();
                    break;
                case "Run":
                    skill = Skills.SkillType.Run;
                    OpenRunPanels();
                    OpenPerks.OpenRunPerkBoxes();
                    break;
                case "Spears":
                    skill = Skills.SkillType.Spears;
                    OpenSpearPanels();
                    OpenPerks.OpenSpearPerkBoxes();
                    break;
                case "Sneak":
                    skill = Skills.SkillType.Sneak;
                    OpenSneakPanels();
                    OpenPerks.OpenSneakPerkBoxes();
                    break;
                case "Swim":
                    skill = Skills.SkillType.Swim;
                    OpenSwimPanels();
                    OpenPerks.OpenSwimPerkBoxes();
                    break;
                case "Swords":
                    skill = Skills.SkillType.Swords;
                    OpenSwordPanels();
                    OpenPerks.OpenSwordPerkBoxes();
                    break;
                case "Woodcutting":
                    skill = Skills.SkillType.WoodCutting;
                    OpenWoodcuttingPanels();
                    OpenPerks.OpenWoodcuttingPerkBoxes();
                    break;
            }
            if (!ConfigManager.IsSkillActive(skill))
            {
                SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                    "Not currently active";
                SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                    "Not currently active";
            }

            Skills.Skill skillRef = player.GetSkills().GetSkill(skill);
            //Jotunn.Logger.LogMessage($"The skill the player seems to want is {skillRef.m_info}");

            SkillGUI.SSIcon.GetComponent<Image>().sprite = player.m_skills.GetSkillDef(skill).m_icon;
            SkillGUI.SSkillName.GetComponent<Text>().text = skillName;
            SkillGUI.SSkillLevel.GetComponent<Text>().text = "Level: " + skillRef.m_level.ToString("F0") + " / 100";
            SkillGUI.SSkillExp.GetComponent<Text>().text = "Experience: " + skillRef.m_accumulator.ToString("F2") + " / " + skillRef.GetNextLevelRequirement().ToString("F2");
            //scroll.GetComponent<ScrollRect>().Rebuild(UnityEngine.UI.CanvasUpdate.PreRender);


            if (!ConfigManager.IsSkillActive(skill))
            {
                SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                    "Not currently active";
                SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                    "Not currently active";
                return;
            }

            if (Perks.IsSkillAscended(skill))
            {
                SkillGUI.RightPanelAscendedText.GetComponent<Text>().text = "Ascended";
            }
            else
            {
                SkillGUI.RightPanelAscendedText.GetComponent<Text>().text = "";
            }
            //Jotunn.Logger.LogMessage($"{Perks.IsSkillAscended(skill).ToString()}");
        }


        public static void OpenAxePanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Axes);
            bool ascended = Perks.IsSkillAscended(Skills.SkillType.Axes);

            float axeDamage = ToPercent(ConfigManager.GetAxeDamageMult(skill));
            float axeStaminaRedux = ToPercent(ConfigManager.GetAxeStaminaRedux(skill));
            float axeStaminaGain = ConfigManager.GetAxeStamina(skill);
            float axeChopBonus = ToPercent(1f + ConfigManager.GetAxeChopDamageMod(skill));
            float axeCarryCapacity = ConfigManager.GetAxeCarryCapacity(skill);

            //Jotunn.Logger.LogMessage($"I'm changing the axe values in now");
            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all axe damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding an axe gains you experience at a very slow rate. \n" +
                "Bonus experience for the axe is gained every time you break a log. ";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                axeDamage.ToString("F1") + "% extra damage with axes\n" +
                axeStaminaRedux.ToString("F1") + "% stamina usage with axes\n" +
                axeStaminaGain.ToString("F0") + " higher base stamina \n" +
                axeChopBonus.ToString("F1") + "% extra chop damage \n" +
                axeCarryCapacity.ToString("F0") + " extra carry capacity";
        }
        public static void OpenBlockPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Blocking);

            float staminaRedux = ToPercent(ConfigManager.GetBlockStaminaRedux(skill));
            float baseBlockPower = ConfigManager.GetBlockPowerFlat(skill);
            float blockPerArmor = ToPercent(ConfigManager.GetBlockPowerMult(skill));
            float blockHealth = ConfigManager.GetBlockHealth(skill);
            float parryExpMod = ToPercent(ConfigManager.GetBlockParryExpMult());

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all damage you block is turned into experience. \n" +
                "This number is unaffected by resistances. \n" +
                "You get " + parryExpMod.ToString("F0") + "% experience for parrying an attack.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                staminaRedux.ToString("F1") + "% stamina cost for blocks\n" +
                baseBlockPower.ToString("F0") + " extra flat block armor\n" +
                blockPerArmor.ToString("F1") + "% extra block armor\n" +
                blockHealth.ToString("F0") + " extra max health";
        }
        public static void OpenBowPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Bows);

            float bowDamage = ToPercent(ConfigManager.GetBowDamageMult(skill));
            float bowStaminaRedux = ToPercent(ConfigManager.GetBowStaminaRedux(skill));
            float bowVelocity = ToPercent(ConfigManager.GetBowVelocityMult(skill));
            float bowDrawSpeed = ConfigManager.GetBowDrawSpeed(skill);
            float bowDropRate = ToPercent(ConfigManager.GetBowDropRateMult(skill));

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all bow damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you shoot, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a bow gains you experience at a very slow rate. \n" +
                "Bonus experience is gained based on the length and difficulty of successful shots with a bow.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                bowDamage.ToString("F1") + "% extra damage with bows\n" +
                bowStaminaRedux.ToString("F1") + "% stamina usage with bows\n" +
                bowVelocity.ToString("F1") + "% extra arrow velocity\n" +
                bowDrawSpeed.ToString("F1") + "% draw speed\n" +
                bowDropRate.ToString("F1") + "% extra loot drops from creatures";
        }
        public static void OpenClubPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Clubs);

            float clubDamage = ToPercent(ConfigManager.GetClubDamageMult(skill));
            float clubStaminaRedux = ToPercent(ConfigManager.GetClubStaminaRedux(skill));
            float clubBlunt = ToPercent(ConfigManager.GetClubBluntMult(skill));
            float clubKnockback = ToPercent(ConfigManager.GetClubKnockbackMult(skill));
            float clubStagger = ToPercent(ConfigManager.GetClubStaggerMult(skill));

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all club damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a club gains you experience at a very slow rate. \n" +
                "Bonus experience every time you stagger an enemy with damage with a club.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                clubDamage.ToString("F1") + "% extra damage with clubs\n" +
                clubStaminaRedux.ToString("F1") + "% stamina usage with clubs\n" +
                clubBlunt.ToString("F1") + "% extra bonus to ALL blunt damage\n" +
                clubKnockback.ToString("F1") + "% knockback bonus to ALL weapons\n" +
                clubStagger.ToString("F1") + "% extra stagger damage to ALL weapons";
        }
        public static void OpenFistPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Unarmed);

            float fistDamage = ToPercent(ConfigManager.GetFistDamageMult(skill));
            float fistStaminaRedux = ToPercent(ConfigManager.GetFistStaminaRedux(skill));
            float fistFlatDamage = ConfigManager.GetFistDamageFlat(skill);
            float fistBlock = ConfigManager.GetFistBlockArmor(skill);
            float fistMovespeed = ToPercent(1f + ConfigManager.GetFistMovespeedMod(skill));

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all unarmed damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding no weapon gains you experience at a very slow rate. \n" +
                "Bonus experence for fists is gained every time you perform an unarmed block.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                fistDamage.ToString("F1") + "% extra damage with fists\n" +
                fistStaminaRedux.ToString("F1") + "% stamina usage with fists\n" +
                fistFlatDamage.ToString("F0") + " extra flat damage\n" +
                fistBlock.ToString("F0") + " extra unarmed block armor\n" +
                fistMovespeed.ToString("F1") + "% extra move speed";
        }
        public static void OpenJumpPanels()
        {
            Player player = Player.m_localPlayer;
            StatsPatch.JumpForceUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Jump);

            float bonusJumpForce = ToPercent(ConfigManager.GetJumpForceMult(skill));
            float bonusJumpForwardForce = ToPercent(ConfigManager.GetJumpForwardForceMult(skill));
            float staminaRedux = ToPercent(ConfigManager.GetJumpStaminaRedux(skill));
            float tired = ToPercent(ConfigManager.GetJumpTiredMod(skill));

            float fallDamageThreshhold = ConfigManager.GetFallDamageThreshold(skill);
            float fallDamageRedux = ToPercent(ConfigManager.GetFallDamageRedux(skill));

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "Every time you jump, you gain a small amount of experience. \n" +
                "Every time you land, you gain an amount of experience based on " +
                "how far you fell and how much damage you'd take.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                bonusJumpForce.ToString("F1") + "% extra vertical jump force \n" +
                bonusJumpForwardForce.ToString("F1") + "% extra horizontal jump force \n" +
                staminaRedux.ToString("F1") + "% less stamina cost to jump \n" +
                tired.ToString("F0") + "% jump force modifier when tired \n" +
                fallDamageThreshhold.ToString("F1") + "m minimum fall damage height \n" +
                fallDamageRedux.ToString("F1") + "% less fall damage";
        }
        public static void OpenKnifePanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Knives);

            float knifeDamage = ToPercent(ConfigManager.GetKnifeDamageMult(skill));
            float knifeStaminaRedux = ToPercent(ConfigManager.GetKnifeStaminaRedux(skill));
            float knifeBackstab = ToPercent(ConfigManager.GetKnifeBackstabMult(skill));
            float knifeMovespeed = ToPercent(1f + ConfigManager.GetKnifeMovespeedMod(skill));
            float knifePierce = ToPercent(ConfigManager.GetKnifePierceMult(skill));

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all knife damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a knife gains you experience at a very slow rate. \n" +
                "Bonus experience is gained every time you perform a sneak attack with a knife.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                knifeDamage.ToString("F1") + "% extra damage with knives \n" +
                knifeStaminaRedux.ToString("F1") + "% stamina usage with knives \n" +
                knifeBackstab.ToString("F0") + "% sneak attack bonus damage \n" +
                knifeMovespeed.ToString("F1") + "% extra move speed \n" +
                knifePierce.ToString("F1") + "% extra to ALL pierce damage";
        }
        public static void OpenPolearmPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Polearms);

            float polearmDamage = ToPercent(ConfigManager.GetPolearmDamageMult(skill));
            float polearmStaminaRedux = ToPercent(ConfigManager.GetPolearmStaminaRedux(skill));
            float polearmRange = ConfigManager.GetPolearmRange(skill);
            float polearmArmor = ConfigManager.GetPolearmArmor(skill);
            float polearmBlock = ConfigManager.GetPolearmBlock(skill);

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all polearm damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a polearm gains you experience at a very slow rate. \n" +
                "Bonus experience for the polearm is gained everytime you hit multiple targets in a single swing.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                polearmDamage.ToString("F1") + "% extra damage with polearms \n" +
                polearmStaminaRedux.ToString("F1") + "% stamina usage with polearms\n" +
                polearmRange.ToString("F0") + " increased units of range with all weapons\n" +
                polearmArmor.ToString("F0") + " increased armor\n" +
                polearmBlock.ToString("F0") + " extra block power with polearms";
        }
        public static void OpenRunPanels()
        {
            Player player = Player.m_localPlayer;
            StatsPatch.RunSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Run);

            float runSpeedBonus = ToPercent(ConfigManager.GetRunSpeedMult(skill));
            float equipmentMalusRedux = ToPercent(ConfigManager.GetEquipmentRedux(skill));
            float encumberanceRedux = ToPercent(ConfigManager.GetEncumberanceRedux(skill));
            float staminaDrainRedux = ToPercent(ConfigManager.GetRunStaminaRedux(skill));
            float baseStaminaGain = ConfigManager.GetRunStamina(skill);


            float encumberanceFactor = ToPercent(MovePatch.GetEncumberanceMult(player));
            float equipmentFactor = ToPercent(MovePatch.GetEquipmentMult(player));

            float absWeightExp = ToPercent(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = ToPercent(MovePatch.relativeWeightBonus(player));
            float runSpeedExp = ToPercent(MovePatch.runSpeedExpBonus(player));

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.\n" +
                "The closer you are to fully encumbered, the less movespeed you have.\n" +
                "You also gain experience based on how encumbered you are. \n" +
                runSpeedExp.ToString("F1") + "% experience bonus from current run speed \n" +
                absWeightExp.ToString("F1") + "% experience bonus from absolute weight carried \n" +
                relWeightExp.ToString("F1") + "% experience bonus from fullness of inventory";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                runSpeedBonus.ToString("F1") + "% extra run speed\n" +
                equipmentMalusRedux.ToString("F1") + "% to equipment penalty\n" +
                encumberanceRedux.ToString("F1") + "% to encumberance penalty\n" +
                staminaDrainRedux.ToString("F1") + "% less stamina cost to run\n" +
                baseStaminaGain.ToString("F1") + " extra base stamina\n" +
                "\nCurrent effects from outside factors: \n" +
                encumberanceFactor.ToString("F1") + "% speed from encumberance\n" +
                equipmentFactor.ToString("F1") + "% speed from equipment";
        }
        public static void OpenSpearPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Spears);

            float spearDamage = ToPercent(ConfigManager.GetSpearDamageMult(skill));
            float spearStaminaRedux = ToPercent(ConfigManager.GetSpearStaminaRedux(skill));
            float spearVelocity = ToPercent(ConfigManager.GetSpearVelocityMult(skill));
            float spearThrowDamage = ToPercent(ConfigManager.GetSpearProjectileDamageMult(skill));
            float spearBlock = ConfigManager.GetSpearBlockArmor(skill);

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all spear damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a spear gains you experience at a very slow rate. \n" +
                "Bonus experience anytime you hit an enemy with a thrown weapon.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                spearDamage.ToString("F1") + "% extra damage with spears \n" +
                spearStaminaRedux.ToString("F1") + "% stamina usage with spears\n" +
                spearVelocity.ToString("F1") + "% increased velocity with all thrown weapons\n" +
                spearThrowDamage.ToString("F1") + "% increased damage with all thrown weapons\n" +
                spearBlock.ToString("F0") + " higher flat block armor";
        }
        public static void OpenSneakPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Sneak);
            StatsPatch.SneakUpdate(Player.m_localPlayer);

            float sneakSpeed = ToPercent(ConfigManager.GetSneakSpeedMult(skill));
            float sneakStaminaCost = ConfigManager.GetSneakStaminaDrain(skill);
            float sneakLightFactor = ToPercent(ConfigManager.GetSneakFactor(skill, 2f));
            float sneakDarkFactor = ToPercent(ConfigManager.GetSneakFactor(skill, 0f));

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "While you are inside the vision angle of an enemy " +
                "but outside its vision range, you gain a flat amount of sneak " +
                "experience every second. \n This value is reduced to 10% while" +
                "you aren't being observed. \n" +
                "You get an additional experience bonus based on how dangerous the " +
                "creature you're sneaking around is.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                sneakSpeed.ToString("F1") + "% increased speed while crouching\n" +
                sneakStaminaCost.ToString("F1") + " stamina per second while crouching\n" +
                sneakLightFactor.ToString("F0") + "% increased sneakiness in the light \n" +
                sneakDarkFactor.ToString("F0") + "% increased sneakiness in the dark";
        }
        public static void OpenSwimPanels()
        {
            Player player = Player.m_localPlayer;
            StatsPatch.SwimSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Run);

            float swimSpeed = ToPercent(ConfigManager.GetSwimSpeedMult(skill));
            float swimAccel = ToPercent(ConfigManager.GetSwimAccelMult(skill));
            float swimTurn = ToPercent(ConfigManager.GetSwimTurnMult(skill));
            float swimStaminaCost = ConfigManager.GetSwimStaminaPerSec(skill);

            float absWeightExp = ToPercent(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = ToPercent(MovePatch.relativeWeightBonus(player));
            float swimSpeedExp = ToPercent(MovePatch.swimSpeedExpBonus(player));


            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "The faster you are moving, the more experience you get.\n" +
                "You also gain experience based on how encumbered you are. \n" +
                swimSpeedExp.ToString("F1") + "% experience bonus from current swimming speed \n" +
                absWeightExp.ToString("F1") + "% experience bonus from absolute weight carried \n" +
                relWeightExp.ToString("F1") + "% experience bonus from fullness of inventory";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                swimSpeed.ToString("F1") + "% extra swim speed\n" +
                swimAccel.ToString("F1") + "% extra acceleration in water\n" +
                swimTurn.ToString("F1") + "% extra turn speed in water\n" +
                swimStaminaCost.ToString("F2") + " stamina per second while swimming";
        }
        public static void OpenSwordPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Swords);
            StatsPatch.SwordUpdate(Player.m_localPlayer);


            float swordDamage = ToPercent(ConfigManager.GetSwordDamageMult(skill));
            float swordStaminaRedux = ToPercent(ConfigManager.GetSwordStaminaRedux(skill));
            float swordParry = ToPercent(ConfigManager.GetSwordParryMult(skill));
            float swordSlash = ToPercent(ConfigManager.GetSwordSlashMult(skill));
            float swordDodgeStaminaRedux = ToPercent(ConfigManager.GetSwordDodgeStaminaRedux(skill));

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all sword damage dealt is turned into experience. \n" +
                "The exp gain rate is much higher versus living targets. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Holding a sword gains you experience at a very slow rate. \n" +
                "Bonus experience is gained every time you deal damage to a staggered enemy with a sword.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                swordDamage.ToString("F1") + "% extra damage with swords \n" +
                swordStaminaRedux.ToString("F1") + "% stamina usage with swords\n" +
                swordParry.ToString("F0") + "% higher parry bonus with ALL weapons \n" +
                swordSlash.ToString("F1") + "% increased slash damage with ALL weapons \n" +
                swordDodgeStaminaRedux.ToString("F1") + "% stamina cost to dodge";
        }
        public static void OpenWoodcuttingPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.WoodCutting);

            float woodDamage = ToPercent(1 + ConfigManager.GetWoodcuttingDamageMod(skill));
            float woodDrop = ToPercent(ConfigManager.GetWoodDropMult(skill));
            float woodRebate = ConfigManager.GetWoodcuttingStaminaRebate(skill);
            float woodSomething = ToPercent(0f);

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all chop damage dealt is turned into experience. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Bonus experience for the axe is gained based on the tier of the tool used.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                woodDamage.ToString("F1") + "% extra damage to trees\n" +
                woodDrop.ToString("F2") + "% increased wood drop rates from trees\n" +
                woodRebate.ToString("F0") + " stamina rebate on axe swings that hit a tree\n" +
                woodSomething.ToString("F0") + " something else!";
        }
        public static void OpenMiningPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Pickaxes);

            float mineDamage = ToPercent(ConfigManager.GetMiningDamageMult(skill));
            float mineDrop = ToPercent(ConfigManager.GetMiningDropMult(skill));
            float mineRebate = ConfigManager.GetMiningStaminaRebate(skill);
            float mineSomething = ToPercent(0f);

            SkillGUI.LeftPanelExperienceText.GetComponent<Text>().text =
                "A percentage of all pick damage dealt to ground turned into experience. \n" +
                "Every time you swing, a small amount of experience is gained, whether you hit anything or not. \n" +
                "Bonus experience for the pick is gained based on the tier of the tool used.\n" +
                "You also gain experience, if reduced, for mining ground without ore.";
            SkillGUI.LeftPanelEffectsText.GetComponent<Text>().text =
                mineDamage.ToString("F1") + "% extra damage to rocks\n" +
                mineDrop.ToString("F2") + "% increased ore drop rates\n" +
                mineRebate.ToString("F0") + " stamina rebate on pick swings that hit a rock\n" +
                mineSomething.ToString("F0") + " something else!";
        }

        public static float ToPercent(float number)
        {
            return (number - 1) * 100;
        }

        public static void PerkLevelCheck(Skills.SkillType skill, float skillFactor)
        {

        }
    }
}
