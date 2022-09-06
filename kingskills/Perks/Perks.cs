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

            //Agriculture
            perk = new Perk("Soil Working",
                "New soil mixing techniques allow you to plant any crop anywhere, regardless of biome limitations.",
                "Why go to the biome when you can bring the biome to you?",
                PerkType.SoilWorking, "Icons/soilworking.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Botany",
                "Your experience with plants gives you the ability to guesstimate time until completion and total yield.",
                "Turns out, when you've seen enough plants, you start to understand them.",
                PerkType.Botany, "Icons/botany.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Combine Harvester",
                "You have gotten so experienced that entire fields will seem to pick themselves before your very eyes. " +
                "You now harvest in a large radius.",
                "The botanical version of strip mining.",
                PerkType.Harvester, "Icons/harvester.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Mass Seeding",
                "Pre-dug holes allow you to sprinkle seeds and vastly improve your planting rate. You now plant" +
                " in a large radius.",
                "Gaia ain't got nothing on you.",
                PerkType.Seeding, "Icons/seeding.png");
            perkList.Add(perk.type, perk);

            //Axes
            perk = new Perk("Decapitation Strike", 
                "Kills with the axe's secondary attack will always drop a trophy.",
                "For when you don't have enough heads on your wall.",
                PerkType.Decapitation, "Icons/decapitation.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Berserkr",
                "When you take damage, there's a small chance to enter a berserker rage, which increases" +
                " your damage, reduces stamina costs, and increases your movespeed.",
                "That's my secret, Cap. I'm always angry.",
                PerkType.Berserkr, "Icons/berserkr.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Highlander",
                "Increases your max health by 100.",
                "There can only be one - and it will be you.",
                PerkType.Highlander, "Icons/highlander.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Throwback",
                "You can now throw your axe like a spear. This attack one-shots all trees it hits.",
                "For when you can't quite reach.",
                PerkType.Throwback, "Icons/throwback.png");
            perkList.Add(perk.type, perk);

            //Blocking
            perk = new Perk("Titan Endurance",
                "Increases your stagger limit by an additional 20% of your max health.",
                "On that day, mankind receieved a grim reminder...",
                PerkType.TitanEndurance, "Icons/titanendurance.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Spiked Shield",
                "Whenever you block an attack, you reflect 50% of the blocked damage to the attacker.",
                "Who needs a sword?",
                PerkType.SpikedShield, "Icons/spikedshield.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Titan Strength",
                "You are no longer encumbered by shields.",
                "If you win, you live. If you lose, you die. If you don't fight, you can't win!",
                PerkType.TitanStrength, "Icons/titanstrength.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Black Flash",
                "Perfect blocks will now cause explosions, dealing damage in an area based on your block armor" +
                " and parry bonus.",
                "'There's a sense of omnipotence, like everything else revolves around them.'",
                PerkType.BlackFlash, "Icons/blackflash.png");
            perkList.Add(perk.type, perk);

            //Bows
            perk = new Perk("Power Draw",
                "A special shot that takes much more energy to draw, but fires with incredible speed and damage.",
                "For when you absolutely need that deer gone.",
                PerkType.PowerDraw, "Icons/powerdraw.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Frugal",
                "You have a 50% chance not to expend ammo.",
                "For the eco-friendly viking warrior.",
                PerkType.Frugal, "Icons/frugal.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Runed Arrows",
                "There's a 75% chance for your arrows to automatically return to you - the other 25% disappear.",
                "It takes pretty long to rune each arrowhead, but luckily, you do that part while you're logged off.",
                PerkType.RunedArrows, "Icons/runedarrows.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Offering to Ullr",
                "Regular offerings to Ullr have blessed your luck, causing monsters to drop better items.",
                "The norse god of the hunt recognizes your skill.",
                PerkType.OfferToUllr, "Icons/ullr.png");
            perkList.Add(perk.type, perk);

            //Building
            perk = new Perk("Structural Engineer",
                "Advanced knowledge of civil engineering secrets have caused your support pillars" +
                " and beams to become almost three times as sturdy as usual.",
                "It's triangles all the way down.",
                PerkType.Engineer, "Icons/engineer.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Ye Olde Nailgune",
                "Your strength and precision have caused your repair efforts to accelerate rapidly. You now " +
                "repair in a large radius with every swing of the hammer.",
                "When all you have is a hammer...",
                PerkType.Nailgun, "Icons/nailgun.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Trapmaster",
                "Your cunning has resulted in several new inventive blueprints of death. Adds several " +
                "new traps to your arsenal.",
                "Some people think it's either building or combat. Why not both?",
                PerkType.Trapmaster, "Icons/trapmaster.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Warehousing Techniques",
                "Your constructions are so sturdy and precise that you've managed to squeeze an extra few" +
                " inventory spaces into almost all forms of storage.",
                "Master of time and space. Well, space, at least.",
                PerkType.Warehousing, "Icons/warehousing.png");
            perkList.Add(perk.type, perk);


            //Clubs
            perk = new Perk("Closing The Gap",
                "Every time you stagger an enemy, you regain 5 stamina and skip towards them.",
                "Perfect for when your flight reflex is broken.",
                PerkType.ClosingTheGap, "Icons/closingthegap.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Thunder Hammer",
                "All AOEs that you create will have double size.",
                "Whosoever holds this hammer, if they be worthy, shall possess the power of... you?",
                PerkType.ThunderHammer, "Icons/thunderhammer.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Plus Ultra",
                "Enemies can no longer be resistant to blunt damage.",
                "What do you do when punching something isn't working? Just punch it harder, obviously.",
                PerkType.PlusUltra, "Icons/plusultra.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Troll Smash",
                "A special attack that ragdolls enemies into the air.",
                "A power even the dragonborn never mastered.",
                PerkType.TrollSmash, "Icons/trollsmash.png");
            perkList.Add(perk.type, perk);

            //Cooking
            perk = new Perk("Spicy and Sweet",
                "The discovery of several new spices has revolutionized your cooking. You can now" +
                " imbue your dishes with resistances to fire, cold, lightning, or poison.",
                "Cornerstones of any refined chef's palate.",
                PerkType.SpicySweet, "Icons/spicysweet.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("High Nutritional Content",
                "You've created a new form of cooking that packs tons of healthy nutrients into the " +
                "dish. You can now imbue your dishes with resistances to blunt, slashing, and piercing, " +
                "or move speed.",
                "The taste, however...",
                PerkType.Nutrition, "Icons/nutrition.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Five Star Chef",
                "Hours of experimentation have led you to understanding what works and what doesn't. " +
                "You have come up with several new recipes, both tasty and healthy.",
                "Now you can call your compatriots idiot sandwiches and they can't complain.",
                PerkType.FiveStarChef, "Icons/fivestarchef.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Spice Master",
                "Your mastery of spices has entered a legendarily gourmet realm. You can now " +
                "imbue your food with extra damage, experience gain, and damage reduction.",
                "A power even the dragonborn never mastered.",
                PerkType.SpiceMaster, "Icons/spicemaster.png");
            perkList.Add(perk.type, perk);

            //Fists
            perk = new Perk("Iron Skin",
                "While you're not wearing armor, you get armor based on half of your unarmed damage.",
                "Breaking all those boards finally paid off.",
                PerkType.IronSkin, "Icons/ironskin.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Lightning Reflexes",
                "Automatically catch arrows and rocks that enter your range of attack.",
                "Nothing goes over my head. My reflexes are too fast. I would catch it.",
                PerkType.LightningReflex, "Icons/lightningreflex.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Falcon Kick",
                "Your kick now causes you to backflip into the air, and sends enemies flying.",
                "K.O.!",
                PerkType.FalconKick, "Icons/falconkick.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Pressure Points",
                "Each attack against an enemy causes a stacking debuff, slowing them and decreasing" +
                " their damage dealt.",
                "Like acupuncture, with significantly less medical benefit.",
                PerkType.PressurePoints, "Icons/pressurepoints.png");
            perkList.Add(perk.type, perk);

            //Jump
            perk = new Perk("Goomba Stomp",
                "You now deal damage when landing on an enemy's head.",
                "Yahoo!",
                PerkType.GoombaStomp, "Icons/goombastomp.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("The Market Gardener",
                "You now do 50% extra damage to enemies while in the air.",
                "Screamin' Eagles!",
                PerkType.MarketGardener, "Icons/marketgardener.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Meteor Drop",
                "When you fall far enough, you make a huge crater and send a shockwave that deals damage " +
                "based on how far you fell.",
                "No promises that you'll survive it.",
                PerkType.MeteorDrop, "Icons/meteordrop.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Odin's Jump",
                "After concentration, you safely leap thousands of meters forwards.",
                "Now possible without getting hit by a giant first!",
                PerkType.OdinJump, "Icons/odinjump.png");
            perkList.Add(perk.type, perk);

            //Jump
            perk = new Perk("Deadeye",
                "You can now throw your knives, dealing half backstab damage.",
                "They don't come back to you, though.",
                PerkType.Deadeye, "Icons/deadeye.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Iai",
                "When you dodgeroll through an enemy, they take damage as though you backstabbed them. Has a " +
                "20 second cooldown.",
                "Lion song.",
                PerkType.Iai, "Icons/iai.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Loki's Gift",
                "Teleport to a target's back, backstabbing them immediately. Distance scales on sneak skill.",
                "Burdened with glorious purpose.",
                PerkType.LokisGift, "Icons/lokisgift.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Disarming Smile",
                "Knives now use their backstab bonus instead of their parry bonus for parrying.",
                "No better defense than an alarmingly quick offense.",
                PerkType.DisarmingDefense, "Icons/disarmingsmile.png");
            perkList.Add(perk.type, perk);

            //Mining
            perk = new Perk("Trench Digger",
                "Increases the depth and width of your pickaxe strikes on the ground.",
                "We both know why you're here.",
                PerkType.TrenchDigger, "Icons/trenchdigger.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Stretch",
                "Increases the range of your pickaxe swings by 50%.",
                "When you just barely can't reach that last rock.",
                PerkType.Stretch, "Icons/stretch.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Rock Hauler",
                "Rocks and metals now weigh 80% less.",
                "When you've handled so many, they all just sorta phase together.",
                PerkType.RockHauler, "Icons/rockhauler.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Lode (Bearing) Stone",
                "When you start hitting an ore vein, a point of light appears. Hitting this point of light will " +
                "cause the entire vein to take full damage.",
                "Copper veins, watch out. I'm coming.",
                PerkType.LodeBearingStone, "Icons/lodebearingstone.png");
            perkList.Add(perk.type, perk);

            //Run
            perk = new Perk("Tackle",
                "Running into an enemy causes a huge knockback effect and causes you to gain a quickly fading " +
                "burst of movespeed.",
                "Outta the way, I'm walkin' here!",
                PerkType.Tackle, "Icons/tackle.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Hermes' Boots",
                "Running for an extended period of time without interruption will cause you to slowly accelerate " +
                "up to a point.",
                "Your world has been blessed with Speed!",
                PerkType.HermesBoots, "Icons/hermesboots.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Water Running",
                "You can now run on water.",
                "When you don't want to wait for the boat.",
                PerkType.WaterRunning, "Icons/waterrunning.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("I'm the Juggernaut",
                "Running into obstacles like trees and rocks will simply cause them to get obliterated " +
                "rather than slowing you down.",
                "Now you just need to find an immovable object.",
                PerkType.Juggernaut, "Icons/juggernaut.png");
            perkList.Add(perk.type, perk);

            //Sailing
            perk = new Perk("Sea Shanty",
                "All crew members aboard your boat gain increased move speed, damage reduction, and damage.",
                "Anyone who boards your vessel will have a belly full of mead and a throat full of song.",
                PerkType.SeaShanty, "Icons/seashanty.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("First Mate",
                "Whenever you are on a ship, but not the captain, a portion of your sailing level is added to" +
                " the captain's when determining sailing buffs.",
                "Every man does their part.",
                PerkType.FirstMate, "Icons/firstmate.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Coup De Burst",
                "You can now activate a huge explosion, sending your ship flying through the air. Better" +
                " hold on!",
                "And so men set sights on the Grand Line, in pursuit of their dreams.",
                PerkType.CoupDeBurst, "Icons/coupdeburst.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Sailing 4th perk",
                "",
                "",
                PerkType.Sailing4, "Icons/seashanty.png");
            perkList.Add(perk.type, perk);

            //Polearm
            perk = new Perk("Jotunn",
                "Your overall size increases. You'll have to duck through short doorways now, but your carrying " +
                "capacity and health will increase.",
                "What's the weather like up there?",
                PerkType.Jotunn, "Icons/jotunn.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Living Stone",
                "All knockback effects on you are greatly reduced, and you gain 10 flat armor.",
                "Now you just need to find an unstoppable force.",
                PerkType.LivingStone, "Icons/livingstone.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Big Stick",
                "Your equipped weapon increases in size, costing more stamina but also dealing more damage.",
                "Unfortunately, does nothing for the softness of your voice.",
                PerkType.BigStick, "Icons/bigstick.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Asguard",
                "You now block in all directions while blocking - but you can only get a perfect block when " +
                "properly facing the attack.",
                "Are all the stars in the sky my enemy?",
                PerkType.Asguard, "Icons/asguard.png");
            perkList.Add(perk.type, perk);

            //Sneak
            perk = new Perk("Silent Sprinter",
                "Adds 30% of your run speed to your sneak speed.",
                "Get back here, you goddamn boar -",
                PerkType.SilentSprinter, "Icons/silentsprinter.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Smoke Bomb",
                "When you enter stealth, all enemies will lose track of you momentarily. Has a 20 second cooldown.",
                "Where'd he go? Must have been the wind.",
                PerkType.SmokeBomb, "Icons/smokebomb.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Hide in Plain Sight",
                "No matter the light conditions, you are always considered to be in pitch blackness.",
                "Much more effective than burying your head.",
                PerkType.HideInPlainSight, "Icons/hideinplainsight.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Cloak of Shadows",
                "While sneaking, you now passively regenerate a percentage of your max health per second.",
                "You have no ally but the darkness.",
                PerkType.CloakOfShadows, "Icons/cloakofshadows.png");
            perkList.Add(perk.type, perk);

            //Spear
            perk = new Perk("Valkyrie's Boon",
                "Whenever you throw a spear, a second spear will be thrown automatically towards the nearest enemy.",
                "Where does the second spear come from? A Valkyrie never tells.",
                PerkType.ValkyriesBoon, "Icons/valkyriesboon.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Just a Guy with a Boomerang",
                "Spears now automatically return to you after they're thrown.",
                "I didn't ask for all this flying and magic!",
                PerkType.Boomerang, "Icons/boomerang.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Couched Lance",
                "After standing still for several seconds, you now gain a large boost to damage.",
                "Usually for charging cavalry, now for stampeding Loxes.",
                PerkType.CouchedLance, "Icons/couchedlance.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Blessing of the Einherjar",
                "All of your projectiles now home towards the nearest target.",
                "Alone no longer.",
                PerkType.EinherjarsBlessing, "Icons/einherjarsblessing.png");
            perkList.Add(perk.type, perk);

            //Swim
            perk = new Perk("Effortless Treading",
                "When treading water, you now regenerate stamina.",
                "Become the kind of man Jotaro Kujo expects you to be",
                PerkType.Treading, "Icons/sealegs.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Butterfly",
                "You can now jump while in water.",
                "Beautiful form!",
                PerkType.Butterfly, "Icons/butterfly.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Always Prepared",
                "Entering water no longer causes you to put your weapons or tools away.",
                "Breaking news: man literally too angry to swim",
                PerkType.AlwaysPrepared, "Icons/alwaysprepared.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Aerodynamic Form",
                "You are no longer affected by the 'Wet' Debuff.",
                "It just... slides right off.",
                PerkType.Aerodynamic, "Icons/aerodynamic.png");
            perkList.Add(perk.type, perk);

            //Swords
            perk = new Perk("Perfect Combo",
                "Each uninterrupted hit now stacks a damage buff. Combo is lost on taking damage.",
                "C-C-COMBO BREAKER!",
                PerkType.PerfectCombo, "Icons/perfectcombo.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("I Studied the Blade",
                "Meditate by using the /sit emote shortly after battle to massively increase your " +
                "experience gain.",
                "Perfect for use while all your peers are partying.",
                PerkType.Meditation, "Icons/meditation.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("God Slayer",
                "Damage dealt to bosses is now increased by 50%.",
                "A god, are you? Good. I was just thinking there was an empty wall above my hearth.",
                PerkType.GodSlayingStrike, "Icons/godslayer.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Warrior of Light",
                "You take less damage based on how much light you are in.",
                "Now you truly do have the power of god and anime on your side.",
                PerkType.WarriorOfLight, "Icons/warrioroflight.png");
            perkList.Add(perk.type, perk);

            //Woodcutting
            perk = new Perk("Heart of the Forest",
                "Every hit on a tree stacks up a buff that reduces the stagger damage you take.",
                "For when you literally can't be bothered to entertain the greydwarves swarming you.",
                PerkType.HeartOfTheForest, "Icons/heartoftheforest.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Master of the Log",
                "You are now immune to falling logs.",
                "You will be the last figure standing when the smoke clears.",
                PerkType.MasterOfTheLog, "Icons/masterofthelog.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Heart of the Monkey",
                "You can now climb trees.",
                "The will of D now lives inside you.",
                PerkType.HeartOfTheMonkey, "Icons/heartofthemonkey.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Pandemonium Swing",
                "Every minute, your next hit on an undamaged tree will fell an entire forest.",
                "You might want to take cover first.",
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
                    //Jotunn.Logger.LogMessage($"Freshly loaded, just set perk {flaggedPerk.Key} to learned and unlearnable because it was in the data");
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
                Jotunn.Logger.LogWarning("skill ascensions haven't been loaded!");
                return false;
            }
        }

        public enum PerkType
        {
            //Agriculture
            SoilWorking, Botany,
            Harvester, Seeding,

            //Axe
            Decapitation, Berserkr,
            Highlander, Throwback,

            //Blocking
            TitanEndurance, SpikedShield,
            TitanStrength, BlackFlash,

            //Bow
            PowerDraw, Frugal,
            RunedArrows, OfferToUllr,

            //Building
            Engineer, Nailgun,
            Trapmaster, Warehousing,

            //Club
            ClosingTheGap, ThunderHammer,
            TrollSmash, PlusUltra,

            //Cooking
            SpicySweet, Nutrition,
            FiveStarChef, SpiceMaster,

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

            //Sailing
            SeaShanty, FirstMate,
            CoupDeBurst, Sailing4,

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
            Treading, Butterfly,
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
