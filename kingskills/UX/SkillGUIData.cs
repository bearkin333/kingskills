using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kingskills.Patches;
using kingskills.UX;
using UnityEngine;
using UnityEngine.UI;

namespace kingskills.UX
{
    public class SkillGUIData
    {
        public virtual bool isOutsideFactors() { return false; }
        public virtual void oPanels() { }

        public virtual void oPerks() { }

        public virtual void oTips() { }

        public void AddTipBreaks()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text += "\n";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text += "\n";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text += "\n";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text += "\n";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text += "\n";
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////
    ///                                    Weapons
    ////////////////////////////////////////////////////////////////////////////////////////
    #region weapons
    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             WEAPONS
    ///              
    ///                             Axes
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class AxeGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Axes);

            float axeDamage = PT.MultToPer(CFG.GetAxeDamageMult(skill));
            float axeStaminaRedux = PT.MultToPer(CFG.GetAxeStaminaRedux(skill), true);
            float axeStaminaGain = CFG.GetAxeStamina(skill);
            float axeChopBonus = PT.MultToPer(1f + CFG.GetAxeChopDamageMod(skill));
            float axeCarryCapacity = CFG.GetAxeCarryCapacity(skill);

            string axeDamageS = PT.Prettify(axeDamage, 1, PT.TType.Percent);
            string axeStaminaReduxS = PT.Prettify(axeStaminaRedux, 1, PT.TType.PercentRedux);
            string axeStaminaGainS = PT.Prettify(axeStaminaGain, 0, PT.TType.Flat);
            string axeChopBonusS = PT.Prettify(axeChopBonus, 1, PT.TType.Percent);
            string axeCarryCapacityS = PT.Prettify(axeCarryCapacity, 0, PT.TType.Flat);


            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                axeDamageS + " damage with axes";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                axeStaminaReduxS + " stamina usage with axes";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                axeStaminaGainS + " higher base stamina";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                axeChopBonusS + " chop damage";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                axeCarryCapacityS + " extra carry capacity";

            //outside factors text
            SkillGUI.LPEffectsTexts["x1"].GetComponent<Text>().text = "";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Axes,
                //perk 1
                Perks.PerkType.Decapitation,
                Perks.PerkType.FourStomachs,
                //perk 2
                Perks.PerkType.Cauterize,
                Perks.PerkType.Highlander,
                //perk 3
                Perks.PerkType.Berserkr,
                Perks.PerkType.Throwback);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Axes are a hardy weapon for a hardy folk, the type who like to make their living " +
                $"in isolation, basking in the biting frost of a mountaintop or the lively loneliness of" +
                $" the forest. Axes also share a deep kinship with woodcutters, and many great axemen were " +
                $"also the most prolific lumberjacks of their time.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"In comparison with other weapons, axes don't have a lot of advantages in combat. They make up " +
                $"for that lack with utility, ferocity, and the convenience of your weapon doubling as a common tool. " +
                $"If you want to make the most of your life as an axemaster, you'd do well to sharpen your weapons " +
                $"on the logs of fallen trees.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"With all weapons in King's Skills, there are 4 ways to earn experience." +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You earn experience over time slowly while holding a weapon" +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You earn a small amount of experience whenever you swing with a weapon" +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a weapon as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every weapon has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"With most weapons, when you are unskilled, combat will be very challenging. It might behoove " +
                $"you to train your skills up against trees or rocks or just swinging at the air before taking on real foes. " +
                $"When you've gotten the hang of a weapon's particular combat style, you'll find that Striking is the quickest " +
                $"way to earn experience. However, a truly skilled viking will keep an eye out for opportunities to earn bonus " +
                $"experience.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Axes gain bonus experience by chopping at logs. It's a simple task with a simple reward, but keep at it " +
                $"for long enough, and you'll grow to be pretty hardy.{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             WEAPONS
    ///              
    ///                             Bows
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class BowGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Bows);

            float bowDamage = PT.MultToPer(CFG.GetBowDamageMult(skill));
            float bowStaminaRedux = PT.MultToPer(CFG.GetBowStaminaRedux(skill), true);
            float bowVelocity = PT.MultToPer(CFG.GetBowVelocityMult(skill));
            float bowDrawSpeed = CFG.GetBowDrawSpeed(skill);
            float bowDropRate = PT.MultToPer(CFG.GetBowDropRateMod(skill) + 1f);

            string bowDamageS = PT.Prettify(bowDamage, 1, PT.TType.Percent);
            string bowStaminaReduxS = PT.Prettify(bowStaminaRedux, 1, PT.TType.PercentRedux);
            string bowVelocityS = PT.Prettify(bowVelocity, 1, PT.TType.Percent);
            string bowDrawSpeedS = PT.Prettify(bowDrawSpeed, 1, PT.TType.Straight);
            string bowDropRateS = PT.Prettify(bowDropRate, 1, PT.TType.Percent);


            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                bowDamageS + " damage with bows";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                bowStaminaReduxS + " stamina usage with bows";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                bowVelocityS + " arrow velocity";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                bowDrawSpeedS + " draw speed";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                bowDropRateS + " loot drops from creatures";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Bows,
                //perk 1
                Perks.PerkType.Frugal,
                Perks.PerkType.PowerDraw,
                //perk 2
                Perks.PerkType.SpiritGuide,
                Perks.PerkType.RunedArrows,
                //perk 3
                Perks.PerkType.Hraesvelg,
                Perks.PerkType.OfferToUllr);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Whether to pick up the bow is not just an easy question in the harsh wilderness of Valheim, " +
                $"but it's often one with only one answer. Relying on a keen eye and ear - and more perhaps importantly," +
                $" the safe distance of a dozen meters - has kept many warriors of Valhalla alive. Being in touch with the beasts " +
                $"and learning to live off the land go hand in hand with the simple, powerful art of archery.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Though bows are one of the safer options in Valheim, their damage output suffers somewhat. It's a " +
                $"good idea to supplement your combat with riskier, more damaging tools, especially early on. Bows are, however, " +
                $"always a practically necessary tool for hunting. Don't fret, though - with a skill like archery, practice certainly pays off." +
                $"Bows get a huge damage increase as their bow draw speed starts to rapidly improve in the later levels.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"With all weapons in King's Skills, there are 4 ways to earn experience." +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You earn experience over time slowly while holding a weapon" +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You earn a small amount of experience whenever you swing with a weapon" +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a weapon as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every weapon has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"With most weapons, when you are unskilled, combat will be very challenging. It might behoove " +
                $"you to train your skills up against trees or rocks or just swinging at the air before taking on real foes. " +
                $"When you've gotten the hang of a weapon's particular combat style, you'll find that Striking is the quickest " +
                $"way to earn experience. However, a truly skilled viking will keep an eye out for opportunities to earn bonus " +
                $"experience.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Bows gain bonus experience based on the distance of every shot that lands. The experience grows " +
                $"exponentially the farther the shot travels, so go for the skies if you want to level your bow quickly!{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             WEAPONS
    ///              
    ///                              Clubs
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class ClubGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Clubs);

            float clubDamage = PT.MultToPer(CFG.GetClubDamageMult(skill));
            float clubStaminaRedux = PT.MultToPer(CFG.GetClubStaminaRedux(skill), true);
            float clubBlunt = PT.MultToPer(CFG.GetClubBluntMult(skill));
            float clubKnockback = PT.MultToPer(CFG.GetClubKnockbackMult(skill));
            float clubStagger = PT.MultToPer(CFG.GetClubStaggerMult(skill));

            string clubDamageS = PT.Prettify(clubDamage, 1, PT.TType.Percent);
            string clubStaminaReduxS = PT.Prettify(clubStaminaRedux, 1, PT.TType.PercentRedux);
            string clubBluntS = PT.Prettify(clubBlunt, 1, PT.TType.Percent);
            string clubKnockbackS = PT.Prettify(clubKnockback, 1, PT.TType.TextlessPercent);
            string clubStaggerS = PT.Prettify(clubStagger, 1, PT.TType.Percent);

            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                clubDamageS + " damage with clubs";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                clubStaminaReduxS + " stamina usage with clubs";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                clubBluntS + " bonus to ALL blunt damage";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                clubKnockbackS + " knockback bonus to ALL weapons";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                clubStaggerS + " stagger damage to ALL weapons";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Clubs,
                //perk 1
                Perks.PerkType.ClosingTheGap,
                Perks.PerkType.BlastWave,
                //perk 2
                Perks.PerkType.Mjolnir,
                Perks.PerkType.GiantSmash,
                //perk 3
                Perks.PerkType.LogHorizon,
                Perks.PerkType.PlusUltra);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"The club is a blunt tool with a very simple application - destroy. The master of the blunt is somewhat of" +
                $" a stubborn, aggressive type - the type who is likely to have an overwhelming preference for the 'fight' " +
                $"instinct over the 'flight'. Anyone who wields a club into battle has only one thing on their mind, and that's " +
                $"the total obliteration of the enemy.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Wielding clubs can be a very powerful but risky choice. Compared to other weapons, they're much slower and often leave" +
                $" much less chance to defend yourself, but they make up for it in high damage output - and more importantly, high " +
                $"stagger damage. The way of the clubs is to knock the enemy silly before they have the chance to deal real damage " +
                $"to you. While they are a more damage focused option, the staggering utility of clubs is their primary focus, " +
                $"making them also a good support in a team scenario.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"With all weapons in King's Skills, there are 4 ways to earn experience." +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You earn experience over time slowly while holding a weapon" +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You earn a small amount of experience whenever you swing with a weapon" +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a weapon as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every weapon has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"With most weapons, when you are unskilled, combat will be very challenging. It might behoove " +
                $"you to train your skills up against trees or rocks or just swinging at the air before taking on real foes. " +
                $"When you've gotten the hang of a weapon's particular combat style, you'll find that Striking is the quickest " +
                $"way to earn experience. However, a truly skilled viking will keep an eye out for opportunities to earn bonus " +
                $"experience.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Clubs gain bonus experience any time you stagger an enemy through raw damage. Enemies also " +
                $"get staggered when you parry, but this doesn't count towards club bonus experience! The amount of " +
                $"bonus experience scales with the level of the enemy, so finding high level foes is worth your time.{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             WEAPONS
    ///              
    ///                             Fists
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class FistGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Unarmed);

            float fistDamage = PT.MultToPer(CFG.GetFistDamageMult(skill));
            float fistStaminaRedux = PT.MultToPer(CFG.GetFistStaminaRedux(skill));
            float fistFlatDamage = CFG.GetFistDamageFlat(skill);
            float fistBlock = CFG.GetFistBlockArmor(skill);
            float fistMovespeed = PT.MultToPer(1f + CFG.GetFistMovespeedMod(skill));

            string fistDamageS = PT.Prettify(fistDamage, 1, PT.TType.Percent);
            string fistStaminaReduxS = PT.Prettify(fistStaminaRedux, 1, PT.TType.PercentRedux);
            string fistFlatDamageS = PT.Prettify(fistFlatDamage, 0, PT.TType.Flat);
            string fistBlockS = PT.Prettify(fistBlock, 0, PT.TType.Flat);
            string fistMovespeedS = PT.Prettify(fistMovespeed, 1, PT.TType.Percent);

            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                fistDamageS + " damage with fists";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                fistStaminaReduxS + " stamina usage with fists";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                fistFlatDamageS + " flat damage";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                fistBlockS + " unarmed block armor";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                fistMovespeedS + " move speed";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Unarmed,
                //perk 1
                Perks.PerkType.IronSkin,
                Perks.PerkType.PressurePoints,
                //perk 2
                Perks.PerkType.LightningReflex,
                Perks.PerkType.Worldly,
                //perk 3
                Perks.PerkType.AttackOfOpportunity,
                Perks.PerkType.FalconKick);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"The unarmed master is never without the capacity of violence. They refuse the call of weapons or armor, not " +
                $"because they can't use them, but because they choose not to - whether it's because they like to feel the raw thrill " +
                $"of throttling an enemy bare handed, or because they choose to obsessively focus on evolving their own body. " +
                $"Either way, there's something ever so slightly unhinged about those who choose the art of the fist.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Unarmed combat, for most, is a last resort when all else fails. Most will only experience the raw, " +
                $"terrifying ordeal of bare-hand blocking a falling troll club because their body is lying just over the ridge. " +
                $"Accordingly, an untrained unarmed skill can be frightfully outmatched by realtively easy opponents. However, " +
                $"this is an instance in which the raw dedication and stubbornness to a cause can provide great rewards, as " +
                $"unarmed combat can be extremely solid all-around when sufficiently trained. Aside from the obvious lack of " +
                $"a need for expensive weaponry, the advanced mobility that comes from unarmed training is almost unparalleled.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"With all weapons in King's Skills, there are 4 ways to earn experience." +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You earn experience over time slowly while holding a weapon" +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You earn a small amount of experience whenever you swing with a weapon" +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a weapon as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every weapon has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"With most weapons, when you are unskilled, combat will be very challenging. It might behoove " +
                $"you to train your skills up against trees or rocks or just swinging at the air before taking on real foes. " +
                $"When you've gotten the hang of a weapon's particular combat style, you'll find that Striking is the quickest " +
                $"way to earn experience. However, a truly skilled viking will keep an eye out for opportunities to earn bonus " +
                $"experience.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Fists skill get bonus experience from performing unarmed blocks. While that's really " +
                $"dangerous in the early levels, later on, you'll find that this can become a very reliable source of " +
                $"experience.{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             WEAPONS
    ///              
    ///                             Knives
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class KnifeGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;
        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Knives);

            float knifeDamage = PT.MultToPer(CFG.GetKnifeDamageMult(skill));
            float knifeStaminaRedux = PT.MultToPer(CFG.GetKnifeStaminaRedux(skill), true);
            float knifeBackstab = PT.MultToPer(CFG.GetKnifeBackstabMult(skill));
            float knifeMovespeed = PT.MultToPer(1f + CFG.GetKnifeMovespeedMod(skill));
            float knifePierce = PT.MultToPer(CFG.GetKnifePierceMult(skill));

            string knifeDamageS = PT.Prettify(knifeDamage, 1, PT.TType.Percent);
            string knifeStaminaReduxS = PT.Prettify(knifeStaminaRedux, 1, PT.TType.PercentRedux);
            string knifeBackstabS = PT.Prettify(knifeBackstab, 0, PT.TType.Percent);
            string knifeMovespeedS = PT.Prettify(knifeMovespeed, 1, PT.TType.Percent);
            string knifePierceS = PT.Prettify(knifePierce, 1, PT.TType.Percent);

            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                knifeDamageS + " damage with knives ";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                knifeStaminaReduxS + " stamina usage with knives ";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                knifeBackstabS + " sneak attack damage ";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                knifeMovespeedS + " move speed ";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                knifePierceS + " to ALL pierce damage";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Knives,
                //perk 1
                Perks.PerkType.Deadeye,
                Perks.PerkType.DisarmingDefense,
                //perk 2
                Perks.PerkType.LokisGift,
                Perks.PerkType.GutAndRun,
                //perk 3
                Perks.PerkType.SleightOfHand,
                Perks.PerkType.Iai);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Of all the weapons to train in, the knife is one of the most frightening. Precisely because the knife master " +
                $"is so seldom seen, that to make enemy of one is to live the rest of your short life in fear of the dark. An assassin " +
                $"trained in this kind of blade only ever fights a battle he has already won. For the foe, the battle is only several " +
                $"terrifying seconds long, but for the truly experienced knife wielder, there are minutes upon hours of prep work and " +
                $"set up.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Knives are known to be quick and highly damaging, but also extraordinarily skill based. Success as a knife" +
                $" user will come down to your ability to pick your battles, find blind spots, and most importantly, perfectly " +
                $"block every attack at just the right time. Daggers have such a small natural block armor that the parry bonus is " +
                $"your only real chance of escaping unscathed from a sizable attack. However, ideal knife play would ideally " +
                $"involve a lot of stealth and very little dodging or blocking. For this reason, knives struggle greatly against " +
                $"bosses, who can't be sneak attacked. However, a dedicated knifemaster who dreams of assassinating the largest " +
                $"enemies of midgard might find a few perks down the line that could help quite a bit.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"With all weapons in King's Skills, there are 4 ways to earn experience." +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You earn experience over time slowly while holding a weapon" +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You earn a small amount of experience whenever you swing with a weapon" +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a weapon as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every weapon has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"With most weapons, when you are unskilled, combat will be very challenging. It might behoove " +
                $"you to train your skills up against trees or rocks or just swinging at the air before taking on real foes. " +
                $"When you've gotten the hang of a weapon's particular combat style, you'll find that Striking is the quickest " +
                $"way to earn experience. However, a truly skilled viking will keep an eye out for opportunities to earn bonus " +
                $"experience.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Knives gain sizable bonus experience from successfully pulled off sneak attacks. Most other" +
                $" weapons don't actually get increased experience relative to backstab damage, but knives do. Thanks to this," +
                $" even though you may spend longer setting up the right situation and the right kill, you won't be losing out " +
                $"on overall experience gain.{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             WEAPONS
    ///              
    ///                             Polearms
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class PolearmGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Polearms);

            float polearmDamage = PT.MultToPer(CFG.GetPolearmDamageMult(skill));
            float polearmStaminaRedux = PT.MultToPer(CFG.GetPolearmStaminaRedux(skill), true);
            float polearmRange = CFG.GetPolearmRange(skill);
            float polearmArmor = CFG.GetPolearmArmor(skill);
            float polearmBlock = CFG.GetPolearmBlock(skill);

            string polearmDamageS = PT.Prettify(polearmDamage, 1, PT.TType.Percent);
            string polearmStaminaReduxS = PT.Prettify(polearmStaminaRedux, 1, PT.TType.PercentRedux);
            string polearmRangeS = PT.Prettify(polearmRange, 0, PT.TType.Flat);
            string polearmArmorS = PT.Prettify(polearmArmor, 0, PT.TType.Flat);
            string polearmBlockS = PT.Prettify(polearmBlock, 0, PT.TType.Flat);


            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                polearmDamageS + " damage with polearms ";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                polearmStaminaReduxS + " stamina usage with polearms";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                polearmRangeS + " units of range with all weapons";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                polearmArmorS + " armor";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                polearmBlockS + " block power with polearms";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Polearms,
                //perk 1
                Perks.PerkType.LivingStone,
                Perks.PerkType.Jotunn,
                //perk 2
                Perks.PerkType.Ymir,
                Perks.PerkType.BigStick,
                //perk 3
                Perks.PerkType.Asguard,
                Perks.PerkType.MassiveStature);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"The sturdy Atgeir wielder is not commonly described as the brightest among Odin's children, but" +
                $" anyone with a bone to pick wouldn't say it to their face. Polearm masters are massive, indestructible forces" +
                $" of methodic destruction. Known for simply walking into the enemy, you can describe them as slow, but " +
                $"it might be easier to describe them as supremely confident and difficult to faze. Characterized by their" +
                $" gruff, silent, and by-the-book approach to combat, polearm users are among the warriors you most want to " +
                $"have on your team, and not on the other.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"The polearm is a unique and exotic weapon, used by the heaviest and most sturdy of Midgard's defenders. " +
                $"The weapon is so large and unwieldy that smaller vikings may have trouble swinging it, but no matter the size, " +
                $"the polearm is a weapon paired best with the heaviest metals known to man adorning your body. Luckily, " +
                $"with the polearm, your ability to successfully block quickly becomes a moot point, as your aoe attack is" +
                $" among the strongest in the game. Polearm masters are most well suited to wading into an army of enemies and" +
                $" sweeping the chaff up in a single strike. Polearms are quite rare to come by in Valheim, so it may be " +
                $"difficult to stick to levelling one up over other, more accessible weapons. However, doing so will be greatly " +
                $"worth your time, as the level-up effects for polearm are quite powerful and unique.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"With all weapons in King's Skills, there are 4 ways to earn experience." +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You earn experience over time slowly while holding a weapon" +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You earn a small amount of experience whenever you swing with a weapon" +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a weapon as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every weapon has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"With most weapons, when you are unskilled, combat will be very challenging. It might behoove " +
                $"you to train your skills up against trees or rocks or just swinging at the air before taking on real foes. " +
                $"When you've gotten the hang of a weapon's particular combat style, you'll find that Striking is the quickest " +
                $"way to earn experience. However, a truly skilled viking will keep an eye out for opportunities to earn bonus " +
                $"experience.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Polearms gain bonus experience whenever you block damage with armor. The amount scales " +
                $"based on the damage you block, so the better your armor and the more challenging your foes, the " +
                $"faster you will find yourself reaching polearm greatness.{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             WEAPONS
    ///              
    ///                             Spears
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class SpearGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Spears);

            float spearDamage = PT.MultToPer(CFG.GetSpearDamageMult(skill));
            float spearStaminaRedux = PT.MultToPer(CFG.GetSpearStaminaRedux(skill), true);
            float spearVelocity = PT.MultToPer(CFG.GetSpearVelocityMult(skill));
            float spearThrowDamage = PT.MultToPer(CFG.GetSpearProjectileDamageMult(skill));
            float spearBlock = CFG.GetSpearBlockArmor(skill);

            string spearDamageS = PT.Prettify(spearDamage, 1, PT.TType.Percent);
            string spearStaminaReduxS = PT.Prettify(spearStaminaRedux, 1, PT.TType.PercentRedux);
            string spearVelocityS = PT.Prettify(spearVelocity, 1, PT.TType.Percent);
            string spearThrowDamageS = PT.Prettify(spearThrowDamage, 1, PT.TType.Percent);
            string spearBlockS = PT.Prettify(spearBlock, 0, PT.TType.Flat);


            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                spearDamageS + " damage with spears ";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                spearStaminaReduxS + " stamina usage with spears";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                spearVelocityS + " velocity with all thrown weapons";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                spearThrowDamageS + " damage with all thrown weapons";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                spearBlockS + " block armor";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Spears,
                //perk 1
                Perks.PerkType.Boomerang,
                Perks.PerkType.FishersBoon,
                //perk 2
                Perks.PerkType.Spearit,
                Perks.PerkType.CouchedLance,
                //perk 3
                Perks.PerkType.Einherjar,
                Perks.PerkType.ValkyrieFlight);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"The long-winged Valkyries of Odin carry thousands of years of combat experience with them, and" +
                $" the exalted few of Midgard's champions who follow their footsteps with the art of spear combat " +
                $"are regaled as among its greatest. Sensible, sharp, well defended, and totally in control of the " +
                $"situation, spear masters are truly some of the most capable combatants there are. They are known by many as" +
                $" perfectionists, elites, or even minor aspects of divinity.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Spears may not have the same raw damage output as some other weapons, but they are without a doubt" +
                $" one of the most well balanced choices on the roster. Designed to be equipped with a shield, spears " +
                $"are quick, good in damage, and very versatile. Most of all, they can be thrown - making for a great " +
                $"way to duel bosses while keeping your distance, or even a bit of hunting. Spears are truly " +
                $"one of the easiest weapons to learn, and hardest to master.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"With all weapons in King's Skills, there are 4 ways to earn experience." +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You earn experience over time slowly while holding a weapon" +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You earn a small amount of experience whenever you swing with a weapon" +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a weapon as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every weapon has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"With most weapons, when you are unskilled, combat will be very challenging. It might behoove " +
                $"you to train your skills up against trees or rocks or just swinging at the air before taking on real foes. " +
                $"When you've gotten the hang of a weapon's particular combat style, you'll find that Striking is the quickest " +
                $"way to earn experience. However, a truly skilled viking will keep an eye out for opportunities to earn bonus " +
                $"experience.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Spears gain bonus experience when you hit with a thrown spear. The experience increases " +
                $"exponentially based on the distance of the shot, so go long if you want to really ascend with the" +
                $" Valkyries!{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             WEAPONS
    ///              
    ///                             Swords
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class SwordGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Swords);
            StatsUpdate.SwordUpdate(Player.m_localPlayer);


            float swordDamage = PT.MultToPer(CFG.GetSwordDamageMult(skill));
            float swordStaminaRedux = PT.MultToPer(CFG.GetSwordStaminaRedux(skill), true);
            float swordParry = PT.MultToPer(CFG.GetSwordParryMult(skill));
            float swordSlash = PT.MultToPer(CFG.GetSwordSlashMult(skill));
            float swordDodgeStaminaRedux = PT.MultToPer(CFG.GetSwordDodgeStaminaRedux(skill), true);

            string swordDamageS = PT.Prettify(swordDamage, 1, PT.TType.Percent);
            string swordStaminaReduxS = PT.Prettify(swordStaminaRedux, 1, PT.TType.PercentRedux);
            string swordParryS = PT.Prettify(swordParry, 0, PT.TType.Percent);
            string swordSlashS = PT.Prettify(swordSlash, 1, PT.TType.Percent);
            string swordDodgeStaminaReduxS = PT.Prettify(swordDodgeStaminaRedux, 1, PT.TType.PercentRedux);

            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                $"{swordDamageS} damage with swords ";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                $"{swordStaminaReduxS} stamina usage with swords";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                $"{swordParryS} parry bonus with ALL weapons ";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                $"{swordSlashS} slash damage with ALL weapons ";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                $"{swordDodgeStaminaReduxS} stamina cost to dodge";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Swords,
                //perk 1
                Perks.PerkType.WarriorOfLight,
                Perks.PerkType.Meditation,
                //perk 2
                Perks.PerkType.Toxic,
                Perks.PerkType.CriticalBlow,
                //perk 3
                Perks.PerkType.PerfectCombo,
                Perks.PerkType.GodSlayingStrike);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Swordmasters are a rare breed. Swords are a weapon characterized by violence - unlike clubs or spears " +
                $"or polearms, there is almost no part of the sword that one can safely touch. Blademasters are a reflection " +
                $"of this fact, lithe but deadly, quick but sturdy, graceful yet powerful. Skill with the blade is thought of " +
                $"as not only the main goal of it's study, but the main goal of life. To those who choose this path, there is" +
                $" no power higher than that of the sword.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Swords are a balanced option that can be used in many ways. Greatswords allow you to go for slower, heavier " +
                $"attacks, where using a shield will allow you to turtle effectively with just as much power. In accordance with" +
                $" the vast array of options, blades are one of the most common weapons. As you get more experienced with them, " +
                $"you'll find yourself being drawn towards the highest skillcap form of play, where your survivability comes " +
                $"largely from parrying and dodge rolling, both areas a sword user excels in.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"With all weapons in King's Skills, there are 4 ways to earn experience." +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You earn experience over time slowly while holding a weapon" +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You earn a small amount of experience whenever you swing with a weapon" +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a weapon as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every weapon has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"With most weapons, when you are unskilled, combat will be very challenging. It might behoove " +
                $"you to train your skills up against trees or rocks or just swinging at the air before taking on real foes. " +
                $"When you've gotten the hang of a weapon's particular combat style, you'll find that Striking is the quickest " +
                $"way to earn experience. However, a truly skilled viking will keep an eye out for opportunities to earn bonus " +
                $"experience.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Swords gain bonus experience when you hit a staggered enemy. For swords, the " +
                $"most effective form of staggering an enemy is to parry and then strike - but combining forces " +
                $"with a club user and their advanced staggering powers may give you ample opportunity to become the" +
                $" world's greatest swordsman.{CFG.ColorEnd}";
        }
    }

    #endregion weapons

    ////////////////////////////////////////////////////////////////////////////////////////
    ///                                    Skills
    ////////////////////////////////////////////////////////////////////////////////////////
    #region skills

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Agriculture
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class AgricultureGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => true;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(SkillMan.Agriculture);

            float plantXPReward = CFG.GetAgriculturePlantReward(Player.m_localPlayer.m_hovering);

            float botYield = PT.MultToPer(1f+CFG.GetAgricultureYieldMod(skill));
            float botGrowRedux = PT.MultToPer(CFG.GetAgricultureGrowTimeRedux(skill), true);
            float botFoodQuality = PT.MultToPer(CFG.GetAgricultureFoodQualityMult(skill));
            float botHealthGain = CFG.GetAgricultureHealthRegain(skill);

            string botYieldS = PT.Prettify(botYield, 2, PT.TType.Percent);
            string botGrowReduxS = PT.Prettify(botGrowRedux, 0, PT.TType.PercentRedux);
            string botFoodQualityS = PT.Prettify(botFoodQuality, 0, PT.TType.Percent);
            string botHealthGainS = PT.Prettify(botHealthGain, 1, PT.TType.Flat);


            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                botYieldS + " yield from plants";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                botGrowReduxS + " time to grow plants";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                botFoodQualityS + " food quality for food you harvest";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                botHealthGainS + " health regained every time you harvest a plant";

            SkillGUI.LPEffectsTexts["x1"].GetComponent<Text>().text =
                "Experience bounty from hovered plant: " + plantXPReward.ToString("F1");
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(SkillMan.Agriculture,
                //perk 1
                Perks.PerkType.GreenThumb,
                Perks.PerkType.SoilMixing,
                //perk 2
                Perks.PerkType.Botany,
                Perks.PerkType.SeedSatchel,
                //perk 3
                Perks.PerkType.Harvester,
                Perks.PerkType.Seeding);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Oft underestimated is the humble botanist, for without the gatherer, no hunter would have " +
                $"ever survived the winter. It was those same gatherers who developed agriculture and farming " +
                $"techniques. It's said that without agriculture, humanity would never have become the top " +
                $"predator on the planet. Woe betide the group that finds itself without agriculture...";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Agriculture is a very important skill. Anything from learning how to replant picked berries " +
                $"to prepping an entire field for a batch of barley are skills that will surely save lives on the " +
                $"battlefield. Experienced botanists will gather larger quantites of materials much faster, and of a " +
                $"higher quality. Using higher quality ingredients affects the quality of cooked food, so this can create " +
                $"a very powerful combination.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"Agriculture experience is gained primarily through harvesting items. Every harvestable item in the game has " +
                $"a different experience bounty associated with it. The Effects screen will show you the value for any harvestable " +
                $"item you're hovering over. You get this bounty once for each item you pick up, so harvesting three Raspberries " +
                $"at once would give you .3xp three times over.";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text = 
                $"As a rule, items that have been grown in a farm will always give much better experience rewards than items you " +
                $"find in the wild. Trees that you plant will grant their requisite experience bounty as soon as they mature.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"[{CFG.ColorPTGreenFF}Food Quality{CFG.ColorEnd}] is a unique mechanic to King's Skills. Each percent of food " +
                $"quality will increase the food's health, stamina, and duration. Items that you harvest or cook will get a " +
                $"random amount that is influenced by your Agriculture or Cooking skill, respectively. Packaging foods together with different " +
                $"qualities will normally flatten both of their quality levels to whichever was the lowest.";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             Skills
    ///              
    ///                             Blocking
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class BlockGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => true;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Blocking);

            float staminaRedux = PT.MultToPer(CFG.GetBlockStaminaRedux(skill), true);
            float baseBlockPower = CFG.GetBlockPowerFlat(skill);
            float blockPerArmor = PT.MultToPer(CFG.GetBlockPowerMult(skill));
            float blockHealth = CFG.GetBlockHealth(skill);
            float parryExpMod = PT.MultToPer(CFG.GetBlockParryExpMult());

            string staminaReduxS = PT.Prettify(staminaRedux, 1, PT.TType.PercentRedux);
            string baseBlockPowerS = PT.Prettify(baseBlockPower, 0, PT.TType.Flat);
            string blockPerArmorS = PT.Prettify(blockPerArmor, 1, PT.TType.Percent);
            string blockHealthS = PT.Prettify(blockHealth, 0, PT.TType.Percent);
            string parryExpModS = PT.Prettify(parryExpMod, 0, PT.TType.ColorlessPercent);


            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                staminaReduxS + " stamina cost for blocks";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                baseBlockPowerS + " flat block armor";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                blockPerArmorS + " block armor";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                blockHealthS + " max health";

            SkillGUI.LPEffectsTexts["x1"].GetComponent<Text>().text =
                $"You get {parryExpModS} experience for parrying an attack.";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Blocking,
                //perk 1
                Perks.PerkType.TitanEndurance,
                Perks.PerkType.SpikedShield,
                //perk 2
                Perks.PerkType.BlockExpert,
                Perks.PerkType.TitanStrength,
                //perk 3
                Perks.PerkType.BlackFlash,
                Perks.PerkType.DidntHurt);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"At the end of the day, there are only really two different ways to survive in Valheim - to dodge, or to block. " +
                $"Either option can be dangerous in it's own way, so it's up to you to decide which risks you're willing to take. " +
                $"Blocking can be dangerous particularly because if an attack puts you over your stagger limit, your block fails and " +
                $"you take the full damage. This is why you gain max health as you level up blocking - in addition to your increased " +
                $"block power, you get a higher stagger limit, allowing your blocks to handle much more.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"You gain experience with the shield based on how much damage you block. You won't learn much from soaking pebbles thrown " +
                $"by greylings in the meadows.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}You gain bonus experience with the shield for correctly parrying an attack. Since this multiplier " +
                $"applies directly to the experience recieved, you can actually get more experience than the damage you blocked this way, so " +
                $"if you're trying to become beefy, do your best to parry! {CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Building
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class BuildGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;
        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(SkillMan.Building);

            float buildHealth = PT.MultToPer(CFG.GetBuildingHealthMult(skill));
            float buildStability = PT.MultToPer(CFG.GetBuildingStabilityMult(skill));
            float buildDamage = PT.MultToPer(CFG.GetBuildingDamageMult(skill));
            float buildWNT = PT.MultToPer(CFG.GetBuildingWNTRedux(skill), true);
            float buildFreeChance = PT.MultToPer(1f + CFG.GetBuildingFreeMod(skill));
            float buildStaminaRedux = PT.MultToPer(CFG.GetBuildingStaminaRedux(skill), true);

            string buildHealthS = PT.Prettify(buildHealth, 1, PT.TType.Percent);
            string buildStabilityS = PT.Prettify(buildStability, 1, PT.TType.Percent);
            string buildDamageS = PT.Prettify(buildDamage, 0, PT.TType.Percent);
            string buildWNTS = PT.Prettify(buildWNT, 0, PT.TType.PercentRedux);
            string buildFreeChanceS = PT.Prettify(buildFreeChance, 0, PT.TType.ColorlessPercent);
            string buildStaminaReduxS = PT.Prettify(buildStaminaRedux, 0, PT.TType.PercentRedux);



            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                buildHealthS + " building health";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                buildStabilityS + " building structural stability ";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                buildDamageS + " damage dealt by buildings";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                buildWNTS + " wear and tear on buildings";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                buildFreeChanceS + " chance to place a free building";

            SkillGUI.LPEffectsTexts["f6"].GetComponent<Text>().text =
                buildStaminaReduxS + " stamina cost for construction and repair";

        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(SkillMan.Building,
                //perk 1
                Perks.PerkType.Engineer,
                Perks.PerkType.Superfuel,
                //perk 2
                Perks.PerkType.Nailgun,
                Perks.PerkType.Warehousing,
                //perk 3
                Perks.PerkType.Trapmaster,
                Perks.PerkType.Efficiency);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Everyone, no matter how strong, ultimately falls prey to Maslow's Hierarchy of Human Needs. Even the most " +
                $"esteemed warrior would crumble over time without food, shelter, a place to rest their head, or a place to sort" +
                $"through their various spoils. No one is quite as aware of this as the architect, master of the towering monuments " +
                $"vikings erect to their own greatness.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Maximizing your building skill makes constructing great buildings easier, cheaper, and faster. The increasing stability you " +
                $"gain from levels in building will allow you to make each new base in a new biome larger and sturdier than the last. Additionally, " +
                $"from level {Mathf.Round(CFG.BuildFreeChanceMinLevel.Value * 100f)} onward, there's an exponentially increasing chance to place any " +
                $"building item for free. You can't get materials back from an item placed for free, however.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"Experience for building can be gained in a number of ways. " +
                $"\n{CFG.ColorWhiteFF}Place:{CFG.ColorEnd} You get a small amount every time you place an item down" +
                $"\n{CFG.ColorWhiteFF}Repair:{CFG.ColorEnd} You get experience when you repair a damage building item, based on the amount of damage repaired" +
                $"\n{CFG.ColorWhiteFF}Defense:{CFG.ColorEnd} You get experience whenever an enemy damages your structures, based on the damage they dealt" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} You get bonus experience when dealing damage with traps";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Bonus experience for building is gained when you deal damage to an enemy with your buildings. In order " +
                $"to maximize your building growth, it's a good idea to take your hammer into highly dangerous territory and build aggressively!{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Cooking
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class CookGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;
        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(SkillMan.Cooking);

            float cookAvgFQ = PT.MultToPer(1f + CFG.GetCookingAverageFoodQualityMod(skill));
            float cookFQRange = PT.MultToPer(1f + CFG.GetCookingFoodQualityRangeMod(skill));
            float cookTimeRedux = PT.MultToPer(CFG.GetCookingTimeRedux(skill), true);
            float cookFermentTimeRedux = PT.MultToPer(CFG.GetCookingFermentTimeRedux(skill), true);

            string cookAvgFQS = PT.Prettify(cookAvgFQ, 1, PT.TType.TextlessPercent);
            string cookFQRangeS = PT.Prettify(cookFQRange, 1, PT.TType.TextlessPercent);
            string cookTimeReduxS = PT.Prettify(cookTimeRedux, 1, PT.TType.PercentRedux);
            string cookFermentTimeReduxS = PT.Prettify(cookFermentTimeRedux, 1, PT.TType.PercentRedux);



            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                cookAvgFQS + " average food quality of cooked items";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                cookFQRangeS + " range in possible food qualities";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                cookTimeReduxS + " cooking time";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                cookFermentTimeReduxS + " fermentation time";

        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(SkillMan.Cooking,
                //perk 1
                Perks.PerkType.SpicySweet,
                Perks.PerkType.TasteTesting,
                //perk 2
                Perks.PerkType.Nutrition,
                Perks.PerkType.KeenNose,
                //perk 3
                Perks.PerkType.SpiceMaster,
                Perks.PerkType.FiveStarChef);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"It is said that for the first several millennia of human history, almost all major conflicts were over either grain or spice. " +
                $"It can't be underestimated what the human being will do for a well cooked dish. Even the halls of Valhalla, overflowing with mead " +
                $"and spiced meats, have tireless chefs working within to produce the true luxuries of the viking's afterlife.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Properly cooking a meal in King's Skills is a different beast. Any time you finish cooking a meal, it gets a random quality. " +
                $"The bulk of the randomness is predicated on your cooking skill, but with each method, you can influence the overall result. " +
                $"\nWith cooking stations, after the meat finishes, in order to get the perfect tenderness, you can leave it on the grill for up " +
                $"to 50% longer. If you wait too long, it will overcook, but the perfect cook will guarantee a positive quality. At higher levels of cooking, " +
                $"you'll be able to get indications of the doneness of the meat just by looking at it." +
                $"\nWith cauldrons and other non-timing based cooking methods, your quality is drastically affected by the quality of the ingredients " +
                $"you use. It's important to make relations with a good gatherer!";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"Every cooking station has a level requirement that must be met with your cooking level in order to use it.";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"Experience for cooking is gained in a number of ways. " +
                $"\n{CFG.ColorWhiteFF}Start:{CFG.ColorEnd} You get a small amount every time you begin a cooking project" +
                $"\n{CFG.ColorWhiteFF}Finish:{CFG.ColorEnd} You get a larger amount for successfully cooking a piece of food" +
                $"\n{CFG.ColorWhiteFF}Tier:{CFG.ColorEnd} Both of the above bonuses get a multiplier for the tier of the station you're using" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} You get bonus experience when someone else eats the food you make";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Bonus experience for cooking is gained when others eat your food. The experience is based on " +
                $"the overall stat bonus that it gives them, so keeping your allies fed using only the highest quality meals will serve " +
                $"you well. You also gain experience from eating the food yourself, but it's a much smaller bonus.{CFG.ColorEnd}";
            SkillGUI.LPTipsTexts[6].GetComponent<Text>().text =
                $"[{CFG.ColorPTGreenFF}Food Quality{CFG.ColorEnd}] is a unique mechanic to King's Skills. Each percent of food " +
                $"quality will increase the food's health, stamina, and duration. Items that you harvest or cook will get a " +
                $"random amount that is influenced by your Agriculture or Cooking skill, respectively. Packaging foods together with different " +
                $"qualities will normally flatten both of their quality levels to whichever was the lowest.";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Jumping
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class JumpGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => false;
        public override void oPanels()
        {
            Player player = Player.m_localPlayer;
            StatsUpdate.JumpForceUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Jump);

            float bonusJumpForce = PT.MultToPer(CFG.GetJumpForceMult(skill));
            float bonusJumpForwardForce = PT.MultToPer(CFG.GetJumpForwardForceMult(skill));
            float staminaRedux = PT.MultToPer(CFG.GetJumpStaminaRedux(skill), true);
            float tired = (CFG.GetJumpTiredMod(skill) + CFG.BaseJumpTiredFactor) * 100f;

            float fallDamageThreshhold = CFG.GetFallDamageThreshold(skill);
            float fallDamageRedux = PT.MultToPer(CFG.GetFallDamageRedux(skill), true);

            string bonusJumpForceS = PT.Prettify(bonusJumpForce, 1, PT.TType.Percent);
            string bonusJumpForwardForceS = PT.Prettify(bonusJumpForwardForce, 1, PT.TType.Percent);
            string staminaReduxS = PT.Prettify(staminaRedux, 1, PT.TType.PercentRedux);
            string tiredS = PT.Prettify(tired, 0, PT.TType.ColorlessPercent);
            string fallDamageThreshholdS = PT.Prettify(fallDamageThreshhold, 0, PT.TType.Straight);
            string fallDamageReduxS = PT.Prettify(fallDamageRedux, 1, PT.TType.PercentRedux);


            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                bonusJumpForceS + " vertical jump force ";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                bonusJumpForwardForceS + " horizontal jump force ";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                staminaReduxS + " stamina cost to jump ";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                tiredS + " jump force modifier when tired";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                fallDamageThreshholdS + "m minimum fall damage height ";

            SkillGUI.LPEffectsTexts["f6"].GetComponent<Text>().text =
                fallDamageReduxS + " fall damage";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Jump,
                //perk 1
                Perks.PerkType.GoombaStomp,
                Perks.PerkType.MeteorDrop,
                //perk 2
                Perks.PerkType.AirStep,
                Perks.PerkType.MarketGardener,
                //perk 3
                Perks.PerkType.HeartOfTheMonkey,
                Perks.PerkType.OdinJump);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Jumping would be a strange skill to focus your energies on, but as with every skill in King's Skills, you can eventually " +
                $"gain very powerful abilities by dedicating your time and training to it.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"You gain a flat amount of experience every time you jump.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"Levelling up jumping both increases the height at which you have to fall before fall damage is calculated and reduces the" +
                $" amount of total fall damage you take. However, those reductions won't affect the experience you get from falling, so the higher " +
                $"your jump gets, the easier time you have levelling it up.";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text = 
                $"{CFG.ColorBonusBlueFF}Bonus experience for jumping is earned by falling from very tall heights. " +
                $"If you really want to maximize your jumping abilities, you'll have to find a <i>very</i> tall cliff. " +
                $"However, you don't gain the experience if you die from the fall, so " +
                $"be careful!{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Mining
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class MineGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => true;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Pickaxes);

            float mineDamage = PT.MultToPer(CFG.GetMiningDamageMult(skill));
            float mineDrop = PT.MultToPer(CFG.GetMiningDropMod(skill) + 1f);
            float mineRebate = CFG.GetMiningStaminaRebate(skill);
            float mineRegen = CFG.GetMiningRegenLessTime(skill);
            float mineCarry = CFG.GetMiningCarryCapacity(skill);

            GameObject rockObj = Player.m_localPlayer.m_hovering;
            ItemDrop.ItemData rock = null;
            if (rockObj != null) rock = rockObj.GetComponent<ItemDrop.ItemData>();
            float mineBXPHoveredRock = CFG.GetMiningXPEatRock(rock);


            string mineDamageS = PT.Prettify(mineDamage, 1, PT.TType.Percent);
            string mineDropS = PT.Prettify(mineDrop, 2, PT.TType.Percent);
            string mineRebateS = PT.Prettify(mineRebate, 0, PT.TType.Flat);
            string mineRegenS = PT.Prettify(mineRegen, 1, PT.TType.Straight);
            string mineCarryS = PT.Prettify(mineCarry, 0, PT.TType.Flat);
            string mineBXPHoveredRockS = PT.Prettify(mineBXPHoveredRock, 0, PT.TType.Straight);

            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                mineDamageS + " mining damage";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                mineDropS + " ore drop rates";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                mineRebateS + " stamina rebate on mining swings";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                mineRegenS + " fewer seconds between health regeneration ticks";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                mineCarryS + " carrying capacity";


            SkillGUI.LPEffectsTexts["x1"].GetComponent<Text>().text =
                $"Bonus Exp from currently hovered rock: {CFG.ColorBonusBlueFF}{mineBXPHoveredRockS}{CFG.ColorEnd}";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Pickaxes,
                //perk 1
                Perks.PerkType.TrenchDigger,
                Perks.PerkType.Magnetic,
                //perk 2
                Perks.PerkType.Fragmentation,
                Perks.PerkType.Stretch,
                //perk 3
                Perks.PerkType.LodeBearingStone,
                Perks.PerkType.RockHauler);
        }

        public override void oTips()
        {
            //EAT ROCKS EAT ROCKS EAT ROCKS
            if (UnityEngine.Random.Range(0f, 1f) < 0.03f) 
            {
                SkillGUI.LPTipsTexts[1].GetComponent<Text>().text = "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS ";
                SkillGUI.LPTipsTexts[2].GetComponent<Text>().text = "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS ";
                SkillGUI.LPTipsTexts[3].GetComponent<Text>().text = "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS ";
                SkillGUI.LPTipsTexts[4].GetComponent<Text>().text = "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS ";
                SkillGUI.LPTipsTexts[5].GetComponent<Text>().text = "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS " +
                    "EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS ";
                return;
            }

            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"The hardness of rocks and metal are some of the most self-evident things about them. The kind of person " +
                $"who makes their living from shattering and harvesting them can only be described as hard themselves. A " +
                $"sturdy person with a rock-solid understanding of their own place in the world.  ";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Mining is arguably the most important skill to level up to progress through the game. The higher level metals create " +
                $"some of the most valuable and powerful equipment that you can make. A true miner is in touch with the ground and the earth.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"Experience for any tool-based skill in King's skills can be gained in several ways. " +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You get a experience very slowly over time while holding the tool." +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You gain a small amount of experience every time you swing." +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a tool as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every tool has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS EAT ROCKS";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Bonus experience with mining is earned by eating rocks! Rocks will stay in your body " +
                $"for {CFG.GetMiningXPRockTimerInSeconds().ToString("F0")} minutes, so you'll have to trade off the value of your food slots and the value " +
                $"of the rock in question for sizable experience bonuses. Any kind of metal, ore, or rock can be eaten. The " +
                $"effects screen will show you the experience value for any rock you're currently hovering over. {CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Running
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class RunGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => true;
        public override void oPanels()
        {
            Player player = Player.m_localPlayer;
            StatsUpdate.RunSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Run);

            float runSpeedBonus = PT.MultToPer(CFG.GetRunSpeedMult(skill));
            float equipmentMalusRedux = PT.MultToPer(CFG.GetEquipmentRedux(skill), true);
            float encumberanceRedux = PT.MultToPer(CFG.GetEncumberanceRedux(skill), true);
            float staminaDrainRedux = PT.MultToPer(CFG.GetRunStaminaRedux(skill), true);
            float baseStaminaGain = CFG.GetRunStamina(skill);


            float encumberanceFactor = -1f * PT.MultToPer(MovePatch.GetEncumberanceRedux(player), true);
            float equipmentFactor = -1f * PT.MultToPer(MovePatch.GetEquipmentMult(player), true);

            //Jotunn.Logger.LogMessage($"encumberance factor comes out as {MovePatch.GetEncumberanceRedux(player)}");
            //Jotunn.Logger.LogMessage($"equipment factor comes out as {MovePatch.GetEquipmentMult(player)}");
            //Jotunn.Logger.LogMessage($"after mult to per, encumb is {encumberanceFactor}");
            //Jotunn.Logger.LogMessage($"after mult to per, equip is {equipmentFactor}");

            float absWeightExp = PT.MultToPer(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = PT.MultToPer(MovePatch.relativeWeightBonus(player));
            float runSpeedExp = PT.MultToPer(MovePatch.runSpeedExpBonus(player));


            string runSpeedBonusS = PT.Prettify(runSpeedBonus, 1, PT.TType.Percent);
            string equipmentMalusReduxS = PT.Prettify(equipmentMalusRedux, 1, PT.TType.PercentRedux);
            string encumberanceReduxS = PT.Prettify(encumberanceRedux, 1, PT.TType.PercentRedux);
            string staminaDrainReduxS = PT.Prettify(staminaDrainRedux, 1, PT.TType.PercentRedux);
            string baseStaminaGainS = PT.Prettify(baseStaminaGain, 0, PT.TType.Flat);

            string encumberanceFactorS = PT.Prettify(encumberanceFactor, 1, PT.TType.Percent);
            string equipmentFactorS = PT.Prettify(equipmentFactor, 1, PT.TType.Percent);

            string absWeightExpS = PT.Prettify(absWeightExp, 1, PT.TType.ColorlessPercent);
            string relWeightExpS = PT.Prettify(relWeightExp, 1, PT.TType.ColorlessPercent);
            string runSpeedExpS = PT.Prettify(runSpeedExp, 1, PT.TType.ColorlessPercent);


            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                runSpeedBonusS + " run speed";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                equipmentMalusReduxS + " penalty from equipment";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                encumberanceReduxS + " penalty from encumberance";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                staminaDrainReduxS + " run stamina cost";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                baseStaminaGainS + " base stamina";


            SkillGUI.LPEffectsTexts["x1"].GetComponent<Text>().text =
                encumberanceFactorS + " speed from encumberance\n " +
                equipmentFactorS + " speed from equipment\n";

            SkillGUI.LPEffectsTexts["x2"].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}{runSpeedExpS} experience{CFG.ColorEnd} from current run speed \n" +
                $"{CFG.ColorBonusBlueFF}{absWeightExpS} experience{CFG.ColorEnd} from absolute weight carried \n" +
                $"{CFG.ColorBonusBlueFF}{relWeightExpS} experience{CFG.ColorEnd} from fullness of inventory";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Run,
                //perk 1
                Perks.PerkType.HermesBoots,
                Perks.PerkType.Tackle,
                //perk 2
                Perks.PerkType.BreakMyStride,
                Perks.PerkType.MountainGoat,
                //perk 3
                Perks.PerkType.Juggernaut,
                Perks.PerkType.WaterRunning);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Running is the one skill that every viking must learn, no matter the situation. There are vast expanses of " +
                $"land in Valheim, and the most reliable source of transportation is one's own feet. To this end, it's also " +
                $"one of the skills with the most factors.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"When your inventory fills up, you gradually slow down as you approach your carrying capacity. The slope is " +
                $"exponential, so you won't <i>really</i> start noticing until you're about half full. You can see the current effect " +
                $"of encumberance on the effects screen. The best way to reduce this is to level up your running skill, or increase " +
                $"your carrying capacity.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"Your move speed is also affected by the weight of the equipment you're currently equipping. This is displayed in each " +
                $"item you equip, but you can see the total effect on the effects screen. Luckily, running skill will also reduce the amount " +
                $"that each heavy equpment item weighs against you, so heavily armored folks stand to gain a lot from upping their run skill.";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"You gain experience based on how fast you're running. Move speed buffs will really help you level up much faster.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}You gain bonus experience for running with weight training. You get a bonus based on the " +
                $"relative fullness of your inventory, but ALSO your absolute " +
                $"weight value. That means that the more you're capable of carrying, the more experience you can get for your running " +
                $"skill - but you can also gain a good bonus just by filling up, even with very little carry capacity. {CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Sailing
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class SailGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => true;
        public override void oPanels()
        {
            Player player = Player.m_localPlayer;
            float skill = player.GetSkillFactor(SkillMan.Sailing);
            Ship ship = player.GetStandingOnShip() ?? player.GetControlledShip();

            float sailXPRate = 0f;
            float sailCaptainBonus = 0f;
            float sailVesselBonus = 0f;
            float sailCrewBonus = 0f;

            if (ship != null)
            {
                if (player.GetControlledShip() == ship)
                {
                    sailXPRate = CFG.SailXPCaptainBase.Value;
                    sailCaptainBonus = Mathf.Abs(ship.GetSpeed()) * CFG.SailXPSpeedMod.Value;
                    sailCaptainBonus *= CFG.GetSailXPWindMult(ship.GetWindAngleFactor());
                }
                else
                {
                    sailXPRate = CFG.SailXPCrewBase.Value;
                }
                sailVesselBonus = PT.MultToPer(CFG.GetSailXPTierMult(ship));
                sailCrewBonus = PT.MultToPer(CFG.GetSailXPCrewMult(ship.m_players.Count));
            }

            float sailSpeed = PT.MultToPer(CFG.GetSailSpeedMult(skill));
            float sailWindNudge = PT.MultToPer(1f + CFG.GetSailWindNudgeMod(skill));
            float sailExploreRange = CFG.GetSailExploreRange(skill);
            float sailPaddleSpeed = PT.MultToPer(CFG.GetSailPaddleSpeedMult(skill));
            float sailShipDamageRedux = PT.MultToPer(CFG.GetSailDamageRedux(skill), true);


            string sailSpeedS = PT.Prettify(sailSpeed, 1, PT.TType.Percent);
            string sailWindNudgeS = PT.Prettify(sailWindNudge, 1, PT.TType.Percent);
            string sailExploreRangeS = PT.Prettify(sailExploreRange, 0, PT.TType.Flat);
            string sailPaddleSpeedS = PT.Prettify(sailPaddleSpeed, 1, PT.TType.Percent);
            string sailShipDamageReduxS = PT.Prettify(sailShipDamageRedux, 0, PT.TType.PercentRedux);


            string sailXPRateS = PT.Prettify(sailXPRate, 2, PT.TType.Straight);
            string sailCaptainBonusS = PT.Prettify(sailCaptainBonus, 2, PT.TType.Flat);
            string sailVesselBonusS = PT.Prettify(sailVesselBonus, 0, PT.TType.Percent);
            string sailCrewBonusS = PT.Prettify(sailCrewBonus, 0, PT.TType.Percent);




            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                sailSpeedS + " ship sailing speed";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                sailWindNudgeS + " nudge towards favorable winds";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                sailExploreRangeS + "m exploration range while on board ";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                sailPaddleSpeedS + " paddle speed";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                sailShipDamageReduxS + " ship damage taken";

            SkillGUI.LPEffectsTexts["x1"].GetComponent<Text>().text =
                sailXPRateS + " per second base rate\n" +
                sailCaptainBonusS + " from Captain bonuses\n" +
                sailVesselBonusS + " from vessel tier\n" +
                sailCrewBonusS + " from number of crewmates";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(SkillMan.Sailing,
                //perk 1
                Perks.PerkType.RockDodger,
                Perks.PerkType.FirstMate,
                //perk 2
                Perks.PerkType.SeaShanty,
                Perks.PerkType.ManOverboard,
                //perk 3
                Perks.PerkType.RammingSpeed,
                Perks.PerkType.CoupDeBurst);
        }

        public override void oTips()
        {

            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"If there's one taste a viking knows well, it's the bitter, salty spray of the sea air betwixt the chopping " +
                $"waves. Many have spent their entire lives at sea, learning the secrets of the ocean and passing them down to " +
                $"generations of younger raiders. This old, venerable art has much to discover yet.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Sailing is a unique skill. Both to be a crew member and to be a captain is to participate in the art of seamanship, " +
                $"and both are responsible for keeping the vessel afloat in their own unique ways. Though the captain may plot the course, " +
                $"without the watchful eyes of their crew, the many dangerous of Valheim's seas could prove overwhelming.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"You gain a flat amount of experience while swimming or standing on board a vessel, and an increased rate while helming " +
                $"it. In either case, there are many ways to modify this experience rate. You can find your current bonuses on the " +
                $"effects page." +
                $"\n{CFG.ColorWhiteFF}Captain Bonus:{CFG.ColorEnd} When you're the captain, you gain a extra flat rate based on the speed " +
                $"of the vessel, which is multiplied by how closely the angle of the wind matches your sails." +
                $"\n{CFG.ColorWhiteFF}Vessel Tier:{CFG.ColorEnd} Every member of the crew gets an experience multiplier based on the vessel's tier" +
                $"\n{CFG.ColorBonusBlueFF}Bonus:{CFG.ColorEnd} Every member of the crew gains a bonus experience multiplier based on how many " +
                $"vikings are on board.";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"Additionally, in order to captain a particular vessel, your sailing skill must surpass a minimum level, based on the tier " +
                $"of the vessel.";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Sneaking
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class SneakGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => true;

        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.Sneak);
            StatsUpdate.SneakUpdate(Player.m_localPlayer);

            float sneakSpeed = PT.MultToPer(CFG.GetSneakSpeedMult(skill));
            float sneakStaminaCost = CFG.GetSneakStaminaDrain(skill);
            float sneakLightFactor = PT.MultToPer(CFG.GetSneakFactor(skill, 2f));
            float sneakDarkFactor = PT.MultToPer(CFG.GetSneakFactor(skill, 0f));

            float sneakDangerXPMod = PT.MultToPer(KSSneak.GetDangerEXPMult(Player.m_localPlayer));

            string sneakSpeedS = PT.Prettify(sneakSpeed, 1, PT.TType.Percent);
            string sneakStaminaCostS = PT.Prettify(sneakStaminaCost, 1, PT.TType.Straight);
            string sneakLightFactorS = PT.Prettify(sneakLightFactor, 0, PT.TType.Percent);
            string sneakDarkFactorS = PT.Prettify(sneakDarkFactor, 0, PT.TType.Percent);

            string sneakDangerXPModS = PT.Prettify(sneakDangerXPMod, 1, PT.TType.ColorlessPercent);

            /*
            SkillGUI.LPEffectsTexts["x1"].GetComponent<Text>().text =
                "While you are actively avoiding detection of a nearby enemy, " +
                "you gain experience every second.";
            SkillGUI.LPEffectsTexts["x2"].GetComponent<Text>().text =
                "If you aren't nearby an enemy while sneaking, you gain 10% experience.";
            */

            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                sneakSpeedS + " sneak speed";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                sneakStaminaCostS + " stamina per second cost while sneaking";



            SkillGUI.LPEffectsTexts["x1"].GetComponent<Text>().text =
                "Current bonus from spotted enemy: " + sneakDangerXPModS;
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Sneak,
                //perk 1
                Perks.PerkType.SilentSprinter,
                Perks.PerkType.VitalStudy,
                //perk 2
                Perks.PerkType.CloakOfShadows,
                Perks.PerkType.ESP,
                //perk 3
                Perks.PerkType.HideInPlainSight,
                Perks.PerkType.SmokeBomb);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Sneak is one of the most interesting, but difficult to pull off gameplay styles in Valheim. King's Skills " +
                $"strives to patch some of the holes a regular sneak player might experience when trying to play through the " +
                $"game with sneak. ";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"You gain experience very slowly while sneaking, but the rate increases by 10x while you're sneaking within the " +
                $"sight line of an enemy.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"Stealth in Valheim is not random. Based on your skill and your level, all enemies have their sight lines decrease " +
                $"by a deterministic amount. If you touch one of their sight cones, you will be noticed. At the same thing, you are " +
                $"totally silent while sneaking, allowing you to perfectly sneak up behind someone as long as they don't turn around.";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Bonus experience with sneak is gained based on the difficulty of the enemy you're sneaking " +
                $"around. The current bonus from the strongest nearby enemy is displayed on the effects screen. And remember - " +
                $"the thrill is part of the fun!{CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Swimming
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class SwimGUI : SkillGUIData
    {
        public override bool isOutsideFactors() => true;
        public override void oPanels()
        {
            Player player = Player.m_localPlayer;
            StatsUpdate.SwimSpeedUpdate(player);
            float skill = player.GetSkillFactor(Skills.SkillType.Swim);

            float swimSpeed = PT.MultToPer(CFG.GetSwimSpeedMult(skill));
            float swimAccel = PT.MultToPer(CFG.GetSwimAccelMult(skill));
            float swimTurn = PT.MultToPer(CFG.GetSwimTurnMult(skill));
            float swimStaminaCost = CFG.GetSwimStaminaPerSec(skill);

            float absWeightExp = PT.MultToPer(MovePatch.absoluteWeightBonus(player));
            float relWeightExp = PT.MultToPer(MovePatch.relativeWeightBonus(player));
            float swimSpeedExp = PT.MultToPer(MovePatch.swimSpeedExpBonus(player));

            string swimSpeedS = PT.Prettify(swimSpeed, 1, PT.TType.Percent);
            string swimAccelS = PT.Prettify(swimAccel, 1, PT.TType.Percent);
            string swimTurnS = PT.Prettify(swimTurn, 1, PT.TType.Percent);
            string swimStaminaCostS = PT.Prettify(swimStaminaCost, 2, PT.TType.Straight);

            string absWeightExpS = PT.Prettify(absWeightExp, 1, PT.TType.ColorlessPercent);
            string relWeightExpS = PT.Prettify(relWeightExp, 1, PT.TType.ColorlessPercent);
            string swimSpeedExpS = PT.Prettify(swimSpeedExp, 1, PT.TType.ColorlessPercent);


            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                swimSpeedS + " swim speed";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                swimAccelS + " acceleration in water";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                swimTurnS + " turn speed in water";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                swimStaminaCostS + " stamina per second swim cost";

            SkillGUI.LPEffectsTexts["x2"].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}{swimSpeedExpS} experience{CFG.ColorEnd} from current swim speed \n" +
                $"{CFG.ColorBonusBlueFF}{absWeightExpS} experience{CFG.ColorEnd} from absolute weight carried \n" +
                $"{CFG.ColorBonusBlueFF}{relWeightExpS} experience{CFG.ColorEnd} from fullness of inventory";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.Swim,
                //perk 1
                Perks.PerkType.JoJoPose,
                Perks.PerkType.Butterfly,
                //perk 2
                Perks.PerkType.Hydrodynamic,
                Perks.PerkType.MarathonSwimmer,
                //perk 3
                Perks.PerkType.Aquaman,
                Perks.PerkType.AlwaysPrepared);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Swimming is a skill most will not have the need or reason to appreciate, but will become increasingly necessary " +
                $"for seabound vikings to master. Vanilla swimming was greatly underutilized, but the effects have vastly expanded. " +
                $"Now, a master swimmer will likely be quicker in the water than a master runner would be on land. However, swimming is " +
                $"beholden to many of the same rules as running.";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"When your inventory fills up, you gradually slow down as you approach your carrying capacity. The slope is " +
                $"exponential, so you won't <i>really</i> start noticing until you're about half full. You can see the current effect " +
                $"of encumberance on the effects screen. The best way to reduce this is to level up your running skill, or increase " +
                $"your carrying capacity.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"Your move speed is also affected by the weight of the equipment you're currently equipping. This is displayed in each " +
                $"item you equip, but you can see the total effect on the effects screen. Luckily, running skill will also reduce the amount " +
                $"that each heavy equpment item weighs against you, so heavily armored folks stand to gain a lot from upping their run skill.";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"You gain experience based on how fast you're swimming. Move speed buffs will really help you level up much faster.";
            SkillGUI.LPTipsTexts[5].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}You gain bonus experience for swimming with weight training. You get a bonus based on the " +
                $"relative fullness of your inventory, but ALSO your absolute " +
                $"weight value. That means that the more you're capable of carrying, the more experience you can get for your running " +
                $"skill - but you can also gain a good bonus just by filling up, even with very little carry capacity. {CFG.ColorEnd}";
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    /// 
    ///                             SKILLS
    ///              
    ///                             Woodcutting
    /// 
    ////////////////////////////////////////////////////////////////////////////////////////
    public class WoodGUI : SkillGUIData
    {
        public override void oPanels()
        {
            float skill = Player.m_localPlayer.GetSkillFactor(Skills.SkillType.WoodCutting);

            float woodDamage = PT.MultToPer(1 + CFG.GetWoodcuttingDamageMod(skill));
            float woodDrop = PT.MultToPer(CFG.GetWoodDropMod(skill) + 1f);
            float woodRebate = CFG.GetWoodcuttingStaminaRebate(skill);
            float woodRegen = CFG.GetWoodcuttingRegenLessTime(skill);
            float woodCarry = CFG.GetWoodcuttingCarryCapacity(skill);

            string woodDamageS = PT.Prettify(woodDamage, 1, PT.TType.Percent);
            string woodDropS = PT.Prettify(woodDrop, 2, PT.TType.Percent);
            string woodRebateS = PT.Prettify(woodRebate, 0, PT.TType.Flat);
            string woodRegenS = PT.Prettify(woodRegen, 0, PT.TType.Straight);
            string woodCarryS = PT.Prettify(woodCarry, 0, PT.TType.Flat);

            SkillGUI.LPEffectsTexts["f1"].GetComponent<Text>().text =
                woodDamageS + " woodcutting damage";

            SkillGUI.LPEffectsTexts["f2"].GetComponent<Text>().text =
                woodDropS + " drop rates from trees";

            SkillGUI.LPEffectsTexts["f3"].GetComponent<Text>().text =
                woodRebateS + " stamina rebate on woodcutting swings";

            SkillGUI.LPEffectsTexts["f4"].GetComponent<Text>().text =
                woodRegenS + " fewer seconds before stamina regeneration";

            SkillGUI.LPEffectsTexts["f5"].GetComponent<Text>().text =
                woodCarryS + " carrying capacity";
        }

        public override void oPerks()
        {
            OpenPerks.OpenPerksByType(Skills.SkillType.WoodCutting,
                //perk 1
                Perks.PerkType.HeartOfTheForest,
                Perks.PerkType.ControlledDemo,
                //perk 2
                Perks.PerkType.ResponsibleLumberjack,
                Perks.PerkType.MasterOfTheLog,
                //perk 3
                Perks.PerkType.ShatterStrike,
                Perks.PerkType.PandemoniumPoint);
        }

        public override void oTips()
        {
            SkillGUI.LPTipsTexts[1].GetComponent<Text>().text =
                $"Trees truly are massive, heavy, unbelievable living objects. Any viking who chooses to make their living from " +
                $"the powerful trunks of these old behemoths understands this, and is filled with respect for these venerated pillars " +
                $"from the bottom of their soul. ";
            SkillGUI.LPTipsTexts[2].GetComponent<Text>().text =
                $"Woodcutting is one of the oldest and most vital skills in Valheim. No matter what stage of the game you're in, you'll " +
                $"never run out of the desire for wood. Luckily, levelling this skill up will give you a plethora of ways to compensate " +
                $"for that ever-growing need.";
            SkillGUI.LPTipsTexts[3].GetComponent<Text>().text =
                $"Experience for any tool-based skill in King's skills can be gained in several ways. " +
                $"\n{CFG.ColorWhiteFF}Hold:{CFG.ColorEnd} You get a experience very slowly over time while holding the tool." +
                $"\n{CFG.ColorWhiteFF}Swing:{CFG.ColorEnd} You gain a small amount of experience every time you swing." +
                $"\n{CFG.ColorWhiteFF}Strike:{CFG.ColorEnd} You earn a percentage of the damage you deal with a tool as experience" +
                $"\n{CFG.ColorWhiteFF}Bonus:{CFG.ColorEnd} Every tool has a different bonus task that can earn you extra XP";
            SkillGUI.LPTipsTexts[4].GetComponent<Text>().text =
                $"{CFG.ColorBonusBlueFF}Bonus experience with woodcutting is earned by clearing out stumps. It's hard work, but " +
                $"unless you clear out those stumps, your forests will never regrow! Every true lumberjack has the health of the " +
                $"forest deep in their heart.{CFG.ColorEnd}";
        }
    }

    #endregion skills

}
