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
            if (WeaponRequired)
            {
                ItemDrop.ItemData weapon = CFG.GetPlayerWeapon(player);
                if (weapon is null) return;
                if (weapon.m_shared.m_skillType != skill) return;
            }

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


        [HarmonyPatch(typeof(MineRock), nameof(MineRock.Start)), HarmonyPostfix]
        static void MineRockRegister(MineRock __instance) =>
            RegisterSetKiller(__instance.m_nview);

        [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.Start)), HarmonyPostfix]
        static void MineRock5Register(MineRock5 __instance) =>
            RegisterSetKiller(__instance.m_nview);

        [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.Awake)), HarmonyPostfix]
        static void TreeBaseRegister(TreeBase __instance) =>
            RegisterSetKiller(__instance.m_nview);

        [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.Awake)), HarmonyPostfix]
        static void TreeLogRegister(TreeLog __instance) =>
            RegisterSetKiller(__instance.m_nview);

        [HarmonyPatch(typeof(Destructible), nameof(Destructible.Start)), HarmonyPostfix]
        static void DestructibleRegister(Destructible __instance) =>
            RegisterSetKiller(__instance.m_nview);

        [HarmonyPatch(typeof(Character), nameof(Character.Awake)), HarmonyPostfix]
        static void CharacterRegister(Character __instance) =>
            RegisterSetKiller(__instance.m_nview);

        public static void RegisterSetKiller(ZNetView nview)
        {
            if (nview != null && nview.m_zdo != null)
                nview.Register<long, ZDOID>("RPC_SetKiller", RPC_SetKiller);
        }

        public static void RPC_SetKiller(long sender, long playerID, ZDOID nviewID)
        {
            //Jotunn.Logger.LogMessage($"sender is {sender}");
            //Jotunn.Logger.LogMessage($"player is {playerID}");
            //Jotunn.Logger.LogMessage($"nview ZDOID is {nviewID}");
            ZNetView nview = ZNetScene.instance.FindInstance(nviewID).GetComponent<ZNetView>();
            nview.m_zdo.Set(CFG.ZDOKiller, playerID);
            nview.m_zdo.Set(CFG.ZDOStaggerFlag, true);

            ZDOMan.instance.ForceSendZDO(sender, nviewID);
            nview.m_zdo.SetOwner(sender);
        }

        public static void IAmKiller_RPC(ZNetView nview)
        {
            if (!nview.m_functions.ContainsKey("RPC_SetKiller".GetStableHashCode()))
                nview.Register<long, ZDOID>("RPC_SetKiller", RPC_SetKiller);

            nview.InvokeRPC("RPC_SetKiller", Game.instance.GetPlayerProfile().GetPlayerID(), nview.m_zdo.m_uid);
        }
    }
}
