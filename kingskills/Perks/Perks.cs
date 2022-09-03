using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills
{
    public static class Perks
    {
        public static Dictionary<Perk, bool> perkFlags;
        public static Dictionary<Skills.SkillType, bool> skillAscendedFlags;
        public static bool loaded = false;

        public static void Awake()
        {
            perkFlags = new Dictionary<Perk, bool>();
            skillAscendedFlags = new Dictionary<Skills.SkillType, bool>();
            InitSkillAcensions();

            loaded = true;
        }

        public static void InitSkillAcensions()
        {
            skillAscendedFlags.Add(Skills.SkillType.Axes, false);
            skillAscendedFlags.Add(Skills.SkillType.Blocking, false);
            skillAscendedFlags.Add(Skills.SkillType.Bows, false);
            skillAscendedFlags.Add(Skills.SkillType.Clubs, false);
            skillAscendedFlags.Add(Skills.SkillType.Jump, false);
            skillAscendedFlags.Add(Skills.SkillType.Knives, false);
            skillAscendedFlags.Add(Skills.SkillType.Pickaxes, false);
            skillAscendedFlags.Add(Skills.SkillType.Polearms, false);
            skillAscendedFlags.Add(Skills.SkillType.Run, false);
            skillAscendedFlags.Add(Skills.SkillType.Sneak, false);
            skillAscendedFlags.Add(Skills.SkillType.Spears, false);
            skillAscendedFlags.Add(Skills.SkillType.Swim, false);
            skillAscendedFlags.Add(Skills.SkillType.Swords, false);
            skillAscendedFlags.Add(Skills.SkillType.Unarmed, false);
            skillAscendedFlags.Add(Skills.SkillType.WoodCutting, false);
        }

        public static bool IsSkillAscended(Skills.SkillType skill)
        {
            if (loaded)
            {
                return skillAscendedFlags[skill];
            }
            else
            {
                Jotunn.Logger.LogWarning("skill acensions haven't been loaded!");
                return false;
            }
        }

        public enum Perk
        {
            //Axe
            Decapitation, Berserkr,
            Highlander, Throwback,

            //Blocking
            TitanEndurance, SpikedShield,
            TitanStrength, BlackFlash,

            //Bow
            PowerDraw, Frugal,
            RunedArrows, OfferToUllr,

            //Club
            ClosingTheGap, ThunderHammer,
            TrollSmash, PlusUltra,

            //Unarmed
            IronSkin, LightningReflex,
            FalconKick, PressurePoints,

            //Jump
            GoombaStomp, MarketGardener,
            MeteorDrop, OdinJump,

            //Knife
            Deadeye, Iai,
            LokisGift, DisarmingDefense,

            //Mining
            TrenchDigger, Stretch,
            RockHauler, LodeBearingStone,

            //Run
            Tackle, HermesBoots,
            WaterRunning,Juggernaut,

            //Polearm
            Jotunn, LivingStone,
            BigStick, Asguard,

            //Sneak
            SmokeBomb, SilentSprinter,
            HideInPlainSight, CloakOfShadows,

            //Spear
            ValkyriesBoon, Boomerang,
            CouchedLance, EinherjarsBlessing,

            //Swim
            SeaLegs, Butterfly,
            AlwaysPrepared, Aerodynamic,

            //Sword
            PerfectCombo, Meditation,
            GodSlayingStrike, WarriorOfLight,

            //Woodcutting
            HeartOfTheForest, MasterOfTheLog,
            HeartOfTheMonkey, PandemoniumSwing
        }

    }
}
