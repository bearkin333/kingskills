using HarmonyLib;
using kingskills.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.RPC
{
    [HarmonyPatch]
    class RPCMan
    {
        [HarmonyPatch(typeof(Player), nameof(Player.Awake))]
        [HarmonyPostfix]
        public static void RegisterRPCs(Player __instance)
        {
            if (__instance.m_nview.GetZDO() is null) return;

            __instance.m_nview.Register<ZPackage>("RPC_RaiseSkill", RPC_RaiseSkill);
        }

        public static void SendXP_RPC(ZNetView player, float factor, Skills.SkillType skill, bool BXP = false, bool weaponRequired = false)
        {
            //Jotunn.Logger.LogMessage($"sending an rpc to {player}");
            if (player.IsValid())
            {
                ZPackage data = new ZPackage();
                data.Write((double)factor);
                data.Write((int)skill);
                data.Write(BXP);
                data.Write(weaponRequired);

                player.InvokeRPC("RPC_RaiseSkill", data);
            }
            else
            {
                Jotunn.Logger.LogWarning("that nview was invalid!");
            }
        }

        public static void RPC_RaiseSkill(long sender, ZPackage data)
        {
            float factor = (float)data.ReadDouble();
            Skills.SkillType skill = (Skills.SkillType)data.ReadInt();
            bool BXP = data.ReadBool();
            bool WeaponRequired = data.ReadBool();

            Player player = Player.m_localPlayer;

            if (player is null) return;
            if (!player.m_nview.IsOwner()) player.m_nview.ClaimOwnership();
            if (WeaponRequired && Util.GetPlayerWeapon(player).m_shared.m_skillType != skill) return;

            Jotunn.Logger.LogMessage($"Just got a successfull call from the server to raise my skill {skill}. If you say so, buddy!");

            if (BXP)
            {
                LevelUp.BXP(player, skill, factor);
            }
            else
            {
                player.RaiseSkill(skill, factor);
            }
        }
    }
}
