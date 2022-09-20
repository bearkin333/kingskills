using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills
{
    public static class Perks
    {
        public static Dictionary<PerkType, bool> perkFlags;
        public static Dictionary<Skills.SkillType, bool> skillAscendedFlags;
        public static Dictionary<PerkType, Perk> perkList;
        public static bool loaded = false;

        public static void Awake()
        {
            perkFlags = new Dictionary<PerkType, bool>();
            skillAscendedFlags = new Dictionary<Skills.SkillType, bool>();
            InitSkillAscensions();
            InitPerkList();

            loaded = true;
        }
        public static void InitSkillAscensions()
        {
            skillAscendedFlags.Add(SkillMan.Agriculture, false);
            skillAscendedFlags.Add(Skills.SkillType.Axes, false);
            skillAscendedFlags.Add(Skills.SkillType.Blocking, false);
            skillAscendedFlags.Add(Skills.SkillType.Bows, false);
            skillAscendedFlags.Add(SkillMan.Building, false);
            skillAscendedFlags.Add(Skills.SkillType.Clubs, false);
            skillAscendedFlags.Add(SkillMan.Cooking, false);
            skillAscendedFlags.Add(Skills.SkillType.Jump, false);
            skillAscendedFlags.Add(Skills.SkillType.Knives, false);
            skillAscendedFlags.Add(Skills.SkillType.Pickaxes, false);
            skillAscendedFlags.Add(Skills.SkillType.Polearms, false);
            skillAscendedFlags.Add(Skills.SkillType.Run, false);
            skillAscendedFlags.Add(SkillMan.Sailing, false);
            skillAscendedFlags.Add(Skills.SkillType.Sneak, false);
            skillAscendedFlags.Add(Skills.SkillType.Spears, false);
            skillAscendedFlags.Add(Skills.SkillType.Swim, false);
            skillAscendedFlags.Add(Skills.SkillType.Swords, false);
            skillAscendedFlags.Add(Skills.SkillType.Unarmed, false);
            skillAscendedFlags.Add(Skills.SkillType.WoodCutting, false);
        }

        public static void InitPerkList()
        {
            perkList = new Dictionary<PerkType, Perk>();
            Perk perk;


            /////////////////////////////////////////////////////////////////////////////////////
            //Agriculture
            perk = CFG.GetPerkSoilMixing();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkBotany();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkSeedSatchel();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkGreenThumb();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkHarvester();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkSeeding();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Axes
            perk = CFG.GetPerkDecapitation();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkBerserkr();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkHighlander();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkThrowback();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkCauterize();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkFourStomachs();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Blocking
            perk = CFG.GetPerkTitanEndurance();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkSpikedShield();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkTitanStrength();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkBlackFlash();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkBlockExpert();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkDidntHurt();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Bows
            perk = CFG.GetPerkPowerDraw();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkFrugal();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkRunedArrows();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkOfferToUllr();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkSpiritGuide();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkHraesvelg();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Building
            perk = CFG.GetPerkEngineer();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkNailgun();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkTrapmaster();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkWarehousing();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkEfficiency();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkSuperfuel();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Clubs
            perk = CFG.GetPerkClosingTheGap();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkBlastWave();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkPlusUltra();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkGiantSmash();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkMjolnir();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkLogHorizon();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Cooking
            perk = CFG.GetPerkSpicySweet();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkNutrition();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkFiveStarChef();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkSpiceMaster();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkTasteTesting();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkKeenNose();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Fists
            perk = CFG.GetPerkIronSkin();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkLightningReflex();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkFalconKick();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkPressurePoints();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkWorldly();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkAttackOfOpportunity();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Jump
            perk = CFG.GetPerkGoombaStomp();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkMarketGardener();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkMeteorDrop();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkOdinJump();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkAirStep();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkHeartOfTheMonkey();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Knives
            perk = CFG.GetPerkDeadeye();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkIai();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkLokisGift();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkDisarmingDefense();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkGutAndRun();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkSleightOfHand();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Mining
            perk = CFG.GetPerkTrenchDigger();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkStretch();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkRockHauler();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkLodeBearingStone();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkFragmentation();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkMagnetic();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Run
            perk = CFG.GetPerkTackle();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkHermesBoots();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkWaterRunning();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkJuggernaut();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkMountainGoat();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkBreakMyStride();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Sailing
            perk = CFG.GetPerkSeaShanty();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkFirstMate();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkCoupDeBurst();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkRockDodger();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkRammingSpeed();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkManOverboard();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Polearm
            perk = CFG.GetPerkJotunn();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkLivingStone();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkBigStick();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkAsguard();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkMassiveStature();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkYmir();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Sneak
            perk = CFG.GetPerkSilentSprinter();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkSmokeBomb();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkHideInPlainSight();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkCloakOfShadows();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkVitalStudy();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkESP();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Spear
            perk = CFG.GetPerkFishersBoon();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkBoomerang();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkCouchedLance();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkEinherjar();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkValkyrieFlight();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkSpearit();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Swim
            perk = CFG.GetPerkJoJoPose();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkButterfly();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkAlwaysPrepared();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkHydrodynamic();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkAquaman();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkMarathonSwimmer();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Swords
            perk = CFG.GetPerkPerfectCombo();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkMeditation();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkGodSlayingStrike();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkWarriorOfLight();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkCriticalBlow();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkToxic();
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Woodcutting
            perk = CFG.GetPerkHeartOfTheForest();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkMasterOfTheLog();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkPandemoniumPoint();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkResponsibleLumberjack();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkControlledDemo();
            perkList.Add(perk.type, perk);

            perk = CFG.GetPerkShatterStrike();
            perkList.Add(perk.type, perk);

        }

        public static void UpdatePerkList()
        {
            foreach (KeyValuePair<PerkType, bool> flaggedPerk in perkFlags)
            {
                if (flaggedPerk.Value)
                {
                    perkList[flaggedPerk.Key].learned = true;
                    perkList[flaggedPerk.Key].learnable = false;
                    //Jotunn.Logger.LogMessage($"Freshly loaded, just set perk {flaggedPerk.Key} to learned and unlearnable because it was in the data");
                }
            }
        }


        public static bool IsSkillAscended(Skills.SkillType skill)
        {
            if (loaded)
            {
                bool contains = skillAscendedFlags.TryGetValue(skill, out bool value);
                if (contains) return value;
                else return contains;
            }
            else
            {
                Jotunn.Logger.LogWarning("skill ascensions haven't been loaded!");
                return false;
            }
        }

        public enum PerkType
        {
            //Agriculture
            SoilMixing, Botany, GreenThumb, SeedSatchel, Harvester, Seeding,

            //Axe
            Decapitation, Berserkr, Highlander, Throwback, Cauterize, FourStomachs,

            //Blocking
            TitanEndurance, SpikedShield, TitanStrength, BlackFlash, BlockExpert, DidntHurt,

            //Bow
            PowerDraw, Frugal, RunedArrows, OfferToUllr, SpiritGuide, Hraesvelg,

            //Building
            Engineer, Nailgun, Trapmaster, Warehousing, Superfuel, Efficiency,

            //Club
            ClosingTheGap, BlastWave, GiantSmash, PlusUltra, Mjolnir, LogHorizon,

            //Cooking
            SpicySweet, Nutrition, FiveStarChef, SpiceMaster, TasteTesting, KeenNose,

            //Unarmed
            IronSkin, LightningReflex, FalconKick, PressurePoints, AttackOfOpportunity, Worldly,

            //Jump
            GoombaStomp, MarketGardener, MeteorDrop, OdinJump, AirStep, HeartOfTheMonkey,

            //Knife
            Deadeye, Iai, LokisGift, DisarmingDefense, GutAndRun, SleightOfHand,

            //Mining
            TrenchDigger, Stretch, RockHauler, LodeBearingStone, Fragmentation, Magnetic,

            //Run
            Tackle, HermesBoots, WaterRunning, Juggernaut, MountainGoat, BreakMyStride,

            //Sailing
            SeaShanty, FirstMate,  CoupDeBurst, RockDodger, RammingSpeed, ManOverboard,

            //Polearm
            Jotunn, LivingStone, BigStick, Asguard, MassiveStature, Ymir,

            //Sneak
            SmokeBomb, SilentSprinter, HideInPlainSight, CloakOfShadows, ESP, VitalStudy,

            //Spear
            FishersBoon, Boomerang, CouchedLance, Einherjar, ValkyrieFlight, Spearit,

            //Swim
            JoJoPose, Butterfly, AlwaysPrepared, Hydrodynamic, Aquaman, MarathonSwimmer,

            //Sword
            PerfectCombo, Meditation, GodSlayingStrike, WarriorOfLight, CriticalBlow, Toxic,

            //Woodcutting
            HeartOfTheForest, MasterOfTheLog, ResponsibleLumberjack, PandemoniumPoint, ControlledDemo, ShatterStrike
        }

        public static void ResetAllPerks()
        {
            //Jotunn.Logger.LogMessage($"Every perk in the perklist is now learnable and not learned");
            foreach (KeyValuePair<Perks.PerkType, Perk> perk in perkList)
            {
                perk.Value.learnable = true;
                perk.Value.learned = false;
            }
        }

        public static void ResetAscensions()
        {
            skillAscendedFlags = new Dictionary<Skills.SkillType, bool>();
            InitSkillAscensions();
        }
    }

    public class Perk
    {
        public string name;
        public string description;
        public string tooltip;
        public bool learned;
        public bool learnable;
        public Perks.PerkType type;
        public Sprite icon;

        public Perk(string nName, string nDescription, string nTooltip, Perks.PerkType nType, string nIcon)
        {
            name = nName;
            description = nDescription;
            tooltip = nTooltip;
            learned = false;
            learnable = true;
            type = nType;
            icon = Assets.AssetLoader.LoadSpriteFromFilename(nIcon);
        }

    }

    public class IsPerkBox : MonoBehaviour
    {

    }
}
