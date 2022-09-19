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
            perk = new Perk("Soil Mixing",
                "New techniques allow you to plant any crop anywhere, regardless of biome limitations.",
                "Why go to the biome when you can bring the biome to you?",
                PerkType.SoilMixing, "Icons/soilworking.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Botany",
                "Intensive study of the workings of plants has increased your ability read basic information about them, such as time until " +
                "completion or yield.",
                "Turns out, when you study plants, you learn things.",
                PerkType.Botany, "Icons/botany.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Seed Satchel",
                "You now carry around an extra pouch, accessible from the inventory, which can store an unlimited number of seeds.",
                "What is a botanist without seeds?",
                PerkType.SeedSatchel, "Icons/seedsatchel.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Green Thumb",
                "Your natural talent for growing plants has manifested, giving you an extra insight into information such as time until " +
                "completion or yield.",
                "Much better than a yellow foot, which you ought to bring up with your doctor.",
                PerkType.GreenThumb, "Icons/greenthumb.png");
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


            /////////////////////////////////////////////////////////////////////////////////////
            //Axes
            perk = new Perk("Decapitate", 
                "Trophies can be eaten to gain axe experience.",
                "My ancestors are smiling at me, imperial. Can you say the same?",
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

            perk = new Perk("Cauterize",
                "You've had the incredible idea of setting all your weapons on fire. You now do 20% extra damage as fire damage.",
                "Anything is a torch, if you're brave enough.",
                PerkType.Cauterize, "Icons/cauterize.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Four Stomachs",
                "You can now throw your axe like a spear. This attack one-shots all trees it hits.",
                "For when you can't quite reach.",
                PerkType.FourStomachs, "Icons/fourstomachs.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Blocking Expertise",
                "All forms of damage are now blockable. Yes, even that.",
                "If it breathes, you can block it.",
                PerkType.BlockExpert, "Icons/blockexpert.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("That Didn't Even Hurt",
                "Any creature that deals less than 5 damage to you, while you're not blocking, now gets staggered out of fear.",
                "Is that all you've got? Pathetic.",
                PerkType.DidntHurt, "Icons/didnthurt.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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
                "It takes pretty long to rune each arrowhead, but luckily, that part happens off-screen.",
                PerkType.RunedArrows, "Icons/runedarrows.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Offering to Ullr",
                "Regular offerings to Ullr have blessed your luck, causing monsters to drop better items.",
                "The norse god of the hunt recognizes your skill.",
                PerkType.OfferToUllr, "Icons/ullr.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Spirit Guide",
                "The ghosts of slain woodland creatures now haunt you, providing an excellent light source while hunting down their children.",
                "They say you should use every part of the animal, why not the soul?",
                PerkType.SpiritGuide, "Icons/spiritguide.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Wings of Hraesvelg",
                "Shots fired will now be accompanied by a pillar of wind, knocking back enemies that have gotten too close as well as yourself.",
                "The great eagle guides you. Fall damage reduction not included.",
                PerkType.Hraesvelg, "Icons/hraesvelg.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Efficiency",
                "New building techniques allow you to build new constructions at half the cost.",
                "It's not 'cutting corners'... If those corners were completely unnecessary!",
                PerkType.Efficiency, "Icons/efficiency.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Superfuel",
                "Constructions you build, such as torches or campfires, no longer require additional fuel to keep running.",
                "It's even renewable!",
                PerkType.Superfuel, "Icons/superfuel.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Clubs
            perk = new Perk("Closing The Gap",
                "Every time you stagger an enemy, you regain 5 stamina and skip towards them.",
                "Perfect for when your flight reflex is broken.",
                PerkType.ClosingTheGap, "Icons/closingthegap.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Blast Wave",
                "All attacks you inflict that hit an area will have their size doubled.",
                "It was all reduced to rubble... and then, again, to ash.",
                PerkType.BlastWave, "Icons/thunderhammer.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Plus Ultra",
                "Enemies can no longer be resistant to blunt damage.",
                "What do you do when punching something isn't working? Just punch it harder, obviously.",
                PerkType.PlusUltra, "Icons/plusultra.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Giant Smash",
                "A special attack that ragdolls enemies into the air.",
                "A power even the dragonborn never mastered.",
                PerkType.GiantSmash, "Icons/giantsmash.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Mjolnir",
                "You deal an additional 20% of damage as lightning damage.",
                "Whosoever holds this hammer, if they be worthy, shall possess the power of... you?",
                PerkType.Mjolnir, "Icons/mjolnir.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Log Horizon",
                "Fallen logs can be picked up to be used as a single use attack, dealing incredible damage.",
                "You swing the log. The enemy sees the horizon... Now <i>that's</i> living in the database.",
                PerkType.LogHorizon, "Icons/loghorizon.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Taste Testing",
                "The simple act of taste testing the food has led you to be able to garner information about a piece of food, " +
                "such as time until completion.",
                "Hmmm. Needs more salt.",
                PerkType.TasteTesting, "Icons/tastetesting.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Keen Nose",
                "Your nose is so powerful that you can detect what's in a dish without even seeing it. This lets you garner addition information " +
                "about cooking food, such as time until completion.",
                "Smells like... Cardamum and basil. And the lightest hint of lime...",
                PerkType.KeenNose, "Icons/keennose.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Worldly Existence",
                "Hours of meditation upon your material lifestyle has lead to an epiphany. You gain timeless wisdom of the ages..." +
                " and 50% increased experience gain.",
                "Material gwurl!",
                PerkType.Worldly, "Icons/worldly.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Attack of Opportunity",
                "While running and unarmed, you perform a free high-speed kick against any enemy that gets close to you.",
                "You're already in my range...",
                PerkType.AttackOfOpportunity, "Icons/attackofopportunity.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Air Step",
                "Allows you to jump one additional time while in the air.",
                "The laws of physics are nothing to a viking!",
                PerkType.AirStep, "Icons/airstep.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Heart of the Monkey",
                "By holding space while in the air, you can now cling to walls such as trees and cliffs. Drains stamina as though you were running.",
                "The will of D now lives inside you.",
                PerkType.HeartOfTheMonkey, "Icons/heartofthemonkey.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Knives
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

            perk = new Perk("Gut and Run",
                "Each attack against an enemy with full health will now cause them to start bleeding.",
                "Sorry to drop in out of nowhere, but...",
                PerkType.GutAndRun, "Icons/gutandrun.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Sleight of Hand",
                "You can now bring certain items through portals.",
                "With a quick enough hand, you can even trick game devs.",
                PerkType.SleightOfHand, "Icons/sleightofhand.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Frag Mine",
                "If there are any enemies nearby when you break a rock, the stone and ore will first pelt that enemy for great damage before " +
                "returning to your feet.",
                "It's not the explosion that gets you, it's the fragmentation.",
                PerkType.Fragmentation, "Icons/fragmentation.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Magnetic Personality",
                "You are now magnetic, dramatically increasing your auto-pickup range.",
                "All that time rubbing metals together has finally affected you. Sure, that's how magnets work, why not?",
                PerkType.Magnetic, "Icons/magnetic.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Mountain Goat",
                "You can now run up nearly sheer angled surfaces, dramatically increasing your mountain mobility.",
                "The undisputed GOAT.",
                PerkType.MountainGoat, "Icons/mountaingoat.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Break My Stride",
                "Carts now only affect your movespeed 30%.",
                "Ain't nothing gonna...",
                PerkType.BreakMyStride, "Icons/breakmystride.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Rock Dodger",
                "Significantly improves several ship manuverability values.",
                "I am a leaf on the wind.",
                PerkType.RockDodger, "Icons/rockdodger.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Ramming Speed",
                "For every 3 seconds that you sail without changing directon, you slowly gain a stacking movespeed increase. The bonus increases " +
                "multiplicatively and does not have a limit.",
                "Brace for impact! Does not come with damage reduction.",
                PerkType.RammingSpeed, "Icons/rammingspeed.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Man Overboard",
                "While captaining a ship, you can now press B to automatically suck in all nearby players.",
                "Much better option than everyone jumping ship.",
                PerkType.ManOverboard, "Icons/manoverboard.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Massive Stature",
                "Increases your overall size even more. Your skin toughens like hide, causing you to be resistant to slashing, lightning, " +
                "and frost damage.",
                "You're gonna need to build a bigger house.",
                PerkType.MassiveStature, "Icons/massivestature.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Memories of Ymir",
                "Tapping into the land's ancient heritage, you gain a 20% extra frost damage with each attack.",
                "Let's hope Odin is not a jealous god.",
                PerkType.Ymir, "Icons/ymir.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Vital Study",
                "Any sneak attack now gains you bonus sneak experience.",
                "They say the appendix serves no function, but when you rip it out, people die? Think for yourself, sheeple.",
                PerkType.VitalStudy, "Icons/vitalstudy.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Extrasensory perception",
                "You can now see lines highlighting enemy sight cones while you're sneaking.",
                "That, or you're just starting to see things now.",
                PerkType.ESP, "Icons/esp.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Spear
            perk = new Perk("Spearfisher's Boon",
                "Whenever you throw a spear, a second spear will be thrown automatically towards the nearest enemy.",
                "Where does the second spear come from? A fisherman never tells.",
                PerkType.FishersBoon, "Icons/fishersboon.png");
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
                PerkType.Einherjar, "Icons/einherjar.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Flight of the Valkyries",
                "When you hit with a thrown spear, a mark is formed. Pressing B will teleport you to that mark, consuming it.",
                "To valhalla leads the way",
                PerkType.ValkyrieFlight, "Icons/valkyrieflight.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("In Spearit",
                "Your spear now deals 30% additional spirit damage.",
                "I'm not sorry.",
                PerkType.Spearit, "Icons/spearit.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
            //Swim
            perk = new Perk("JoJo Pose",
                "When treading water, you now regenerate stamina.",
                "Become the kind of man Jotaro Kujo expects you to be",
                PerkType.JoJoPose, "Icons/jojopose.png");
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

            perk = new Perk("Hydrodynamic Form",
                "You are no longer affected by the 'Wet' Debuff.",
                "It just... slides right off.",
                PerkType.Hydrodynamic, "Icons/hydrodynamic.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Aquaman",
                "As you remain in water, an increasingly larger school of fish will circle you and protect you from any " +
                "aggressors.",
                "It may not be laser eyes, but it's an objectively cool power.",
                PerkType.Aquaman, "Icons/aquaman.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Marathon Swimmer",
                "While you're swimming in the ocean, you gain a slowly stacking bonus to move speed, damage reduction, and stamina " +
                "regeneration.",
                "You're just on a roll.",
                PerkType.MarathonSwimmer, "Icons/marathonswimmer.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Critical Blow",
                "You have a 10% chance to deal double damage on each hit.",
                "The kind of hit your DM would be embarassed to narrate.",
                PerkType.CriticalBlow, "Icons/criticalblow.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Poisoned Blade",
                "Your attacks deal an additional 40% damage as poison damage.",
                "The hardest part is remembering to wear gloves when you apply the poison.",
                PerkType.Toxic, "Icons/toxic.png");
            perkList.Add(perk.type, perk);


            /////////////////////////////////////////////////////////////////////////////////////
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

            perk = new Perk("Pandemonium Point",
                "Standing in place for 20 seconds near a tree will cause a special target to appear. Striking it will cause a " +
                "shockwave that fells an entire forest.",
                "You might want to take cover first.",
                PerkType.PandemoniumPoint, "Icons/pandemoniumpoint.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Responsible Lumberjacking",
                "Stumps now get destroyed in one hit, and if possible, will automatically get replanted as saplings.",
                "Sustainable farming is the work of the finest lumberjacks!",
                PerkType.ResponsibleLumberjack, "Icons/responsiblelumberjack.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Controlled Demolition",
                "Trees will always fall away from you.",
                "Instructions: Put tree between you and enemy",
                PerkType.ControlledDemo, "Icons/controlleddemo.png");
            perkList.Add(perk.type, perk);

            perk = new Perk("Shatterstrike",
                "You build up stacks every second over time while holding an axe. Each stack increases the damage you deal to the first tree " +
                "you hit. If you have maximum stacks, 60, hitting a tree will cause it to immediately get turned into wood. There's a 10 second " +
                "cooldown before the stacks begin building again.",
                "Be the kind of man Captain America believes you can be.",
                PerkType.ShatterStrike, "Icons/shatterstrike.png");
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
