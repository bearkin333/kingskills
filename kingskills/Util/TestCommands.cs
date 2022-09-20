using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using kingskills;
using kingskills.Perks;
using kingskills.Patches;

namespace kingskills
{
    public class TestExperienceCommand : ConsoleCommand
    {
        public override string Name => "expks";

        public override string Help => "applies a certain experience for testing";

        public override void Run(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }
            string skill = args[0];
            float.TryParse(args[1], out float factor);

            Player.m_localPlayer.RaiseSkill(CFG.GetSkillFromName(skill), factor);
        }
    }
    public class TestSkillCommand : ConsoleCommand
    {
        public override string Name => "raiseks";

        public override string Help => "levels up the new skills for kingskills";

        public override void Run(string[] args)
        {
            if (args.Length != 0)
            {
                return;
            }
            //increment test skill
            Jotunn.Logger.LogMessage("Raising sailing, cooking, building, and agriculture");
            Player.m_localPlayer.RaiseSkill(SkillMan.Sailing, 10);
            Player.m_localPlayer.RaiseSkill(SkillMan.Cooking, 10);
            Player.m_localPlayer.RaiseSkill(SkillMan.Building, 10);
            Player.m_localPlayer.RaiseSkill(SkillMan.Agriculture, 10);
        }
    }
    public class ResetPerksCommand : ConsoleCommand
    {
        public override string Name => "resetperks";

        public override string Help => "resets all perk selections";

        public override void Run(string[] args)
        {
            if (args.Length != 0)
            {
                return;
            }

            PerkMan.ResetAllPerks();
        }
    }

    public class ResetAscensionsCommand : ConsoleCommand
    {
        public override string Name => "resetascend";

        public override string Help => "resets all ascensions";

        public override void Run(string[] args)
        {
            if (args.Length != 0)
            {
                return;
            }

            PerkMan.ResetAscensions();
        }
    }

    public class SkillUpdateCommand : ConsoleCommand
    {
        public override string Name => "updateks";

        public override string Help => "Runs the update functions for king's skills";

        public override void Run(string[] args)
        {
            if (args.Length != 0)
            {
                return;
            }

            StatsUpdate.UpdateStats(Player.m_localPlayer);
        }
    }

    public class TestSkillsCommand : ConsoleCommand
    {
        public override string Name => "setallskills";

        public override string Help => "Sets all skills to 50 for testing";

        public override void Run(string[] args)
        {
            if (args.Length != 0)
            {
                return;
            }

            Console.instance.TryRunCommand("devcommands");
            Console.instance.TryRunCommand("god");

            Console.instance.TryRunCommand("resetskill Axes");
            Console.instance.TryRunCommand("resetskill Blocking");
            Console.instance.TryRunCommand("resetskill Bows");
            Console.instance.TryRunCommand("resetskill Clubs");
            Console.instance.TryRunCommand("resetskill Unarmed");
            Console.instance.TryRunCommand("resetskill Jump");
            Console.instance.TryRunCommand("resetskill Knives");
            Console.instance.TryRunCommand("resetskill Pickaxes");
            Console.instance.TryRunCommand("resetskill Polearms");
            Console.instance.TryRunCommand("resetskill Run");
            Console.instance.TryRunCommand("resetskill Spears");
            Console.instance.TryRunCommand("resetskill Sneak");
            Console.instance.TryRunCommand("resetskill Swim");
            Console.instance.TryRunCommand("resetskill Swords");
            Console.instance.TryRunCommand("resetskill Woodcutting");

            Console.instance.TryRunCommand("raiseskill Axes 50");
            Console.instance.TryRunCommand("raiseskill Blocking 50");
            Console.instance.TryRunCommand("raiseskill Bows 50");
            Console.instance.TryRunCommand("raiseskill Clubs 50");
            Console.instance.TryRunCommand("raiseskill Unarmed 50");
            Console.instance.TryRunCommand("raiseskill Jump 50");
            Console.instance.TryRunCommand("raiseskill Knives 50");
            Console.instance.TryRunCommand("raiseskill Pickaxes 50");
            Console.instance.TryRunCommand("raiseskill Polearms 50");
            Console.instance.TryRunCommand("raiseskill Run 50");
            Console.instance.TryRunCommand("raiseskill Spears 50");
            Console.instance.TryRunCommand("raiseskill Sneak 50");
            Console.instance.TryRunCommand("raiseskill Swim 50");
            Console.instance.TryRunCommand("raiseskill Swords 50");
            Console.instance.TryRunCommand("raiseskill Woodcutting 50");

        }
    }
}
