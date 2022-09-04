﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills
{
    [HarmonyPatch(typeof(PlayerProfile))]
    public static class SaveLoad
    {
        public const string saveDirectory = "/KS";
        public const string fileEnding = "_ks.fch";

        [HarmonyPatch(nameof(PlayerProfile.SavePlayerToDisk))]
        [HarmonyPostfix]
        public static void SaveKSPerks(PlayerProfile __instance)
        {
            //Decide on the save paths for our new files
            string saveFolderPath = PlayerProfile.GetCharacterFolderPath(__instance.m_fileSource)
                + saveDirectory;
            if (!Directory.Exists(saveFolderPath) && __instance.m_fileSource != FileHelpers.FileSource.SteamCloud)
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            string oldSavePath = PlayerProfile.GetCharacterFolderPath(__instance.m_fileSource)
                + saveDirectory + "/" + __instance.m_filename + fileEnding;
            string newSavePath = PlayerProfile.GetCharacterFolderPath(__instance.m_fileSource)
                + saveDirectory + "/" + __instance.m_filename + fileEnding + ".new";
            ZPackage zPackage = new ZPackage();
            //Jotunn.Logger.LogWarning($"saving in path {oldSavePath}");

            //Write all of our parseable information to a package
            if (Perks.loaded)
            {
                Dictionary<int, bool> savePerkFlags = new Dictionary<int, bool>();
                Dictionary<int, bool> saveSkillAscendedFlags = new Dictionary<int, bool>();

                foreach (KeyValuePair<Perks.PerkType, bool> datum in Perks.perkFlags)
                {
                    savePerkFlags.Add((int)datum.Key, datum.Value);
                }

                foreach (KeyValuePair<Skills.SkillType, bool> datum in Perks.skillAscendedFlags)
                {
                    saveSkillAscendedFlags.Add((int)datum.Key, datum.Value);
                }

                SaveFlagsToZPackage(ref zPackage, savePerkFlags);
                SaveFlagsToZPackage(ref zPackage, saveSkillAscendedFlags);
            }
            else
            {
                Jotunn.Logger.LogWarning("Dictionaries haven't been set up");
            }

            //Writing the zpackage to binary
            //Which writes to a file
            byte[] array = zPackage.GenerateHash();
            byte[] array2 = zPackage.GetArray();
            FileStream fileStream = File.Create(newSavePath);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            binaryWriter.Write(array2.Length);
            binaryWriter.Write(array2);
            binaryWriter.Write(array.Length);
            binaryWriter.Write(array);
            binaryWriter.Flush();

            //True means flushing to disk
            fileStream.Flush(true);

            //Close the filestream
            fileStream.Close();
            fileStream.Dispose();

            //Delete the old save file (if it exists)
            if (File.Exists(oldSavePath))
                File.Delete(oldSavePath);

            //Replace it with the new one
            File.Move(newSavePath, oldSavePath);
        }

        [HarmonyPatch(nameof(PlayerProfile.LoadPlayerFromDisk))]
        [HarmonyPostfix]
        public static void LoadKSPerks(PlayerProfile __instance)
        {
            try
            {
                string filePath = PlayerProfile.GetCharacterFolderPath(__instance.m_fileSource)
                    + saveDirectory + "/" + __instance.m_filename + fileEnding;

                ZPackage zPackage = StreamFromFile(filePath);
                //Jotunn.Logger.LogWarning($"loading from {filePath}");

                if (zPackage == null) return;


                //handle the new data from the zpackage
                Dictionary<int, bool> loadPerkFlags = new Dictionary<int, bool>();
                Dictionary<int, bool> loadSkillAscendedFlags = new Dictionary<int, bool>();
                Perks.Awake();

                loadPerkFlags = LoadFlagsFromZPackage(zPackage);
                loadSkillAscendedFlags = LoadFlagsFromZPackage(zPackage);

                foreach (KeyValuePair<int, bool> datum in loadPerkFlags)
                {
                    Perks.perkFlags.Add((Perks.PerkType)datum.Key, datum.Value);
                }
                foreach (KeyValuePair<int, bool> datum in loadSkillAscendedFlags)
                {
                    Perks.skillAscendedFlags[(Skills.SkillType)datum.Key] = datum.Value;
                    //Jotunn.Logger.LogMessage($"Set {((Skills.SkillType)datum.Key).ToString()} to {datum.Value}");
                }

                Perks.UpdatePerkList();
                Perks.loaded = true;

            }
            catch
            {
                Jotunn.Logger.LogWarning("Didn't load from King Skills");
                return;
            }

        }

        [HarmonyPatch(nameof(PlayerProfile.LoadPlayerData))]
        [HarmonyPostfix]
        public static void LoadPlayerPatch(PlayerProfile __instance, Player player)
        {
            StatsPatch.UpdateStats(player);
        }

        public static ZPackage StreamFromFile(string filePath)
        {
            FileStream fileStream;
            try
            {
                fileStream = File.OpenRead(filePath);
            }
            catch
            {
                Jotunn.Logger.LogWarning("No save data for Kings Skills");
                return null;
            }

            byte[] data;
            try
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                int count = binaryReader.ReadInt32();
                data = binaryReader.ReadBytes(count);
                count = binaryReader.ReadInt32();
                binaryReader.ReadBytes(count);
            }
            catch
            {
                Jotunn.Logger.LogError("File found, but error loading Kings Skills data");
                fileStream.Dispose();
                return null;
            }

            return new ZPackage(data);
        }

        public static void SaveFlagsToZPackage(ref ZPackage zp, Dictionary<int, bool> dict)
        {
            zp.Write(dict.Count);
            foreach (KeyValuePair<int, bool> flag in dict)
            {
                zp.Write(flag.Key);
                zp.Write(flag.Value);
            }
        }

        public static Dictionary<int, bool> LoadFlagsFromZPackage(ZPackage zp)
        {
            Dictionary<int, bool> dict = new Dictionary<int, bool>();

            int count = zp.ReadInt();
            for (int i = 0; i < count; i++)
            {
                dict.Add(zp.ReadInt(), zp.ReadBool());
            }

            return dict;
        }

    }
}
