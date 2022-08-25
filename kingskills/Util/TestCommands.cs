﻿using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;

namespace kingskills.Commands
{
    public class BearSkillCommand : ConsoleCommand
    {
        public override string Name => "increment_bear_skill";

        public override string Help => "levels up the test ability for bear's skills";

        public override void Run(string[] args)
        {
            if (args.Length != 0)
            {
                return;
            }
            //increment test skill
            Jotunn.Logger.LogMessage("Bear skill incrementing!");
            Player.m_localPlayer.RaiseSkill(KingSkills.TestSkillType, 10);
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

            LevelPatch.SwimSpeedUpdate(Player.m_localPlayer);
            LevelPatch.RunSpeedUpdate(Player.m_localPlayer);
            LevelPatch.JumpForceUpdate(Player.m_localPlayer);
        }
    }
}
