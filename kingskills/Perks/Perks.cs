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
            InitSkillAcensions();
            InitPerkList();

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

        public static void InitPerkList()
        {
            perkList = new Dictionary<PerkType, Perk>();
            Perk perk;

            perk = new Perk("Decapitation Strike", 
                "A special strike that is guaranteed to drop a trophy.",
                "Yikes!",
                PerkType.Decapitation, "Icons/decapitation.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Berserkr",
                "When you take damage, there's a small chance to enter a berserker rage, which increases" +
                " your damage, reduces stamina costs, and increases your movespeed.",
                "",
                PerkType.Berserkr, "Icons/berserkr.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Highlander",
                "Increases your max health by 100.",
                "",
                PerkType.Highlander, "Icons/highlander.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Throwback",
                "You can now throw your axe like a spear. This attack one-shots all trees it hits.",
                "",
                PerkType.Throwback, "Icons/throwback.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Titan Endurance",
                "Increases your stagger limit by an additional 20% of your max health.",
                "",
                PerkType.TitanEndurance, "Icons/titanendurance.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Spiked Shield",
                "Whenever you block an attack, you reflect 50% of the blocked damage to the attacker.",
                "",
                PerkType.SpikedShield, "Icons/spikedshield.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Titan Strength",
                "You are no longer encumbered by shields.",
                "",
                PerkType.TitanStrength, "Icons/titanstrength.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Black Flash",
                "Perfect blocks will now cause explosions, dealing damage in an area based on your block armor" +
                " and parry bonus.",
                "",
                PerkType.BlackFlash, "Icons/blackflash.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Power Draw",
                "A special show that takes much more energy to draw, but fires with incredible speed and damage.",
                "",
                PerkType.PowerDraw, "Icons/powerdraw.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Frugal",
                "You have a 50% chance not to expend ammo.",
                "",
                PerkType.Frugal, "Icons/frugal.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Runed Arrows",
                "There's a 75% chance for your arrows to automatically return to you - the other 25% disappear.",
                "",
                PerkType.RunedArrows, "Icons/runedarrows.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Offering to Ullr",
                "Regular offerings to Ullr have blessed your luck, causing monsters to drop better items.",
                "",
                PerkType.OfferToUllr, "Icons/ullr.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Closing The Gap",
                "Every time you stagger an enemy, you regain 5 stamina and skip towards them.",
                "",
                PerkType.ClosingTheGap, "Icons/closingthegap.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Thunder Hammer",
                "All AOEs that you create will have double size.",
                "",
                PerkType.ThunderHammer, "Icons/thunderhammer.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Plus Ultra",
                "Enemies can no longer be resistant to blunt damage.",
                "",
                PerkType.PlusUltra, "Icons/plusultra.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Troll Smash",
                "A special attack that ragdolls enemies into the air.",
                "",
                PerkType.TrollSmash, "Icons/trollsmash.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Iron Skin",
                "While you're not wearing armor, 50% of your damage with fists becomes armor.",
                "",
                PerkType.IronSkin, "Icons/ironskin.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Lightning Reflexes",
                "Automatically catch arrows and rocks that enter your range of attack.",
                "",
                PerkType.LightningReflex, "Icons/lightningreflex.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Falcon Kick",
                "Your kick now causes you to backflip into the air, and sends enemies flying.",
                "",
                PerkType.FalconKick, "Icons/falconkick.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Pressure Points",
                "Each attack against an enemy causes a stacking debuff, slowing them and decreasing" +
                " their damage dealt.",
                "",
                PerkType.PressurePoints, "Icons/pressurepoints.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Goomba Stomp",
                "You now deal damage when landing on an enemy's head.",
                "",
                PerkType.GoombaStomp, "Icons/goombastomp.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("The Market Gardener",
                "You now do 50% extra damage to enemies while in the air.",
                "",
                PerkType.MarketGardener, "Icons/marketgardener.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Meteor Drop",
                "When you fall far enough, you make a huge crater and send a shockwave that deals damage " +
                "based on how far you fell.",
                "",
                PerkType.MeteorDrop, "Icons/meteordrop.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Odin's Jump",
                "After concentration, you safely leap thousands of meters forwards.",
                "",
                PerkType.OdinJump, "Icons/odinjump.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Deadeye",
                "You can now throw your knives, dealing half backstab damage.",
                "",
                PerkType.Deadeye, "Icons/deadeye.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Iai",
                "When you dodgeroll through an enemy, they take damage as though you backstabbed them. Has a " +
                "20 second cooldown.",
                "",
                PerkType.Iai, "Icons/iai.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Loki's Gift",
                "Teleport to a target's back, backstabbing them immediately. Distance scales on sneak skill.",
                "",
                PerkType.LokisGift, "Icons/lokisgift.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Disarming Smile",
                "Knives now use their backstab bonus instead of their parry bonus for parrying.",
                "",
                PerkType.DisarmingDefense, "Icons/disarmingsmile.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Trench Digger",
                "Increases the depth and width of your pickaxe strikes on the ground.",
                "",
                PerkType.TrenchDigger, "Icons/trenchdigger.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Stretch",
                "Increases the range of your pickaxe swings by 50%.",
                "",
                PerkType.Stretch, "Icons/stretch.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Rock Hauler",
                "Rocks and metals now weigh 80% less.",
                "",
                PerkType.RockHauler, "Icons/rockhauler.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Lode (Bearing) Stone",
                "When you start hitting an ore vein, a point of light appears. Hitting this point of light will " +
                "cause the entire vein to take full damage.",
                "",
                PerkType.LodeBearingStone, "Icons/lodebearingstone.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Tackle",
                "Running into an enemy causes a huge knockback effect and causes you to gain a quickly fading " +
                "burst of movespeed.",
                "",
                PerkType.Tackle, "Icons/tackle.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Hermes' Boots",
                "Running for an extended period of time without interruption will cause you to slowly accelerate " +
                "up to a point.",
                "",
                PerkType.HermesBoots, "Icons/hermesboots.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Water Running",
                "You can now run on water.",
                "",
                PerkType.WaterRunning, "Icons/waterrunning.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("I'm the Juggernaut",
                "Running into obstacles like trees and rocks will simply cause them to get obliterated " +
                "rather than slowing you down.",
                "",
                PerkType.Juggernaut, "Icons/juggernaut.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Jotunn",
                "Your overall size increases. You'll have to duck through short doorways now, but your carrying " +
                "capacity and health will increase.",
                "",
                PerkType.Jotunn, "Icons/jotunn.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Living Stone",
                "All knockback effects on you are greatly reduced, and you gain 10 flat armor.",
                "",
                PerkType.LivingStone, "Icons/livingstone.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Big Stick",
                "Your equipped weapon increases in size, costing more stamina but also dealing more damage.",
                "",
                PerkType.BigStick, "Icons/bigstick.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Asguard",
                "You now block in all directions while blocking - but you can only get a perfect block when " +
                "properly facing the attack.",
                "",
                PerkType.Asguard, "Icons/asguard.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Silent Sprinter",
                "Adds 30% of your run speed to your sneak speed.",
                "",
                PerkType.SilentSprinter, "Icons/silentsprinter.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Smoke Bomb",
                "When you enter stealth, all enemies will lose track of you momentarily. Has a 20 second cooldown.",
                "",
                PerkType.SmokeBomb, "Icons/smokebomb.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Hide in Plain Sight",
                "No matter the light conditions, you are always considered to be in pitch blackness.",
                "",
                PerkType.HideInPlainSight, "Icons/hideinplainsight.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Cloak of Shadows",
                "While sneaking, you now passively regenerate a percentage of your max health per second.",
                "",
                PerkType.CloakOfShadows, "Icons/cloakofshadows.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Valkyrie's Boon",
                "Whenever you throw a spear, a second spear will be thrown automatically towards the nearest enemy.",
                "",
                PerkType.ValkyriesBoon, "Icons/valkyriesboon.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Just a Guy with a Boomerang",
                "Spears now automatically return to you after they're thrown.",
                "",
                PerkType.Boomerang, "Icons/boomerang.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Couched Lance",
                "After standing still for several seconds, you now gain a large boost to damage.",
                "",
                PerkType.CouchedLance, "Icons/couchedlance.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Blessing of the Einherjar",
                "All of your projectiles now home towards the nearest target.",
                "",
                PerkType.EinherjarsBlessing, "Icons/einherjarsblessing.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Sea Legs",
                "You now take half damage while in water.",
                "",
                PerkType.SeaLegs, "Icons/sealegs.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Butterfly",
                "You can now jump while in water.",
                "",
                PerkType.Butterfly, "Icons/butterfly.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Always Prepared",
                "Entering water no longer causes you to put your weapons or tools away.",
                "",
                PerkType.AlwaysPrepared, "Icons/alwaysprepared.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Aerodynamic Form",
                "You are no longer affected by the 'Wet' Debuff.",
                "",
                PerkType.Aerodynamic, "Icons/aerodynamic.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Perfect Combo",
                "Each uninterrupted hit now stacks a damage buff. Combo is lost on taking damage.",
                "",
                PerkType.PerfectCombo, "Icons/perfectcombo.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("I Studied the Blade",
                "Meditate by using the /sit emote shortly after battle to massively increase your " +
                "experience gain.",
                "",
                PerkType.Meditation, "Icons/meditation.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("God Slayer",
                "Damage dealt to bosses is now increased by 50%.",
                "",
                PerkType.GodSlayingStrike, "Icons/godslayer.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Warrior of Light",
                "You take less damage based on how much light you are in.",
                "",
                PerkType.WarriorOfLight, "Icons/warrioroflight.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Heart of the Forest",
                "Every hit on a tree stacks up a buff that reduces the stagger damage you take.",
                "",
                PerkType.HeartOfTheForest, "Icons/heartoftheforest.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Master of the Log",
                "You are now immune to falling logs.",
                "",
                PerkType.MasterOfTheLog, "Icons/masterofthelog.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Heart of the Monkey",
                "You can now climb trees.",
                "",
                PerkType.HeartOfTheMonkey, "Icons/heartofthemonkey.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Pandemonium Swing",
                "A special swing that fells an entire forest.",
                "",
                PerkType.PandemoniumSwing, "Icons/pandemoniumswing.png");
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
                }
            }
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

        public enum PerkType
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

        public static void ResetAllPerks()
        {
            foreach (KeyValuePair<Perks.PerkType, Perk> perk in perkList)
            {
                perk.Value.learnable = true;
                perk.Value.learned = false;
            }
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
