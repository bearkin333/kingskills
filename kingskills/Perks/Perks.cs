using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kingskills.Perks
{
    class Perks
    {
    }

    [HarmonyPatch(typeof(PlayerProfile))]
    public static class SaveLoad
    {
        public const string saveDirectory = "/characters/KS";
        public const string fileEnding = "_ks.fch";

        [HarmonyPatch(nameof(PlayerProfile.SavePlayerToDisk))]
        [HarmonyPostfix]
        public static void SaveKSPerks(PlayerProfile __instance)
        {
            //Decide on the save paths for our new files
            Directory.CreateDirectory(Utils.GetSaveDataPath(__instance.m_fileSource)
                + saveDirectory);
            string oldSavePath = Utils.GetSaveDataPath(__instance.m_fileSource)
                + saveDirectory + "/" + __instance.m_filename + fileEnding;
            string newSavePath = Utils.GetSaveDataPath(__instance.m_fileSource)
                + saveDirectory + "/" + __instance.m_filename + fileEnding + ".new";
            ZPackage zPackage = new ZPackage();
            //Jotunn.Logger.LogWarning($"saving in path {oldSavePath}");

            //Write all of our parseable information to a package
            zPackage.Write(true);


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
            string filePath = Utils.GetSaveDataPath(__instance.m_fileSource)
                + saveDirectory + "/" + __instance.m_filename + fileEnding;

            ZPackage zPackage = StreamFromFile(filePath);
            //Jotunn.Logger.LogWarning($"loading from {filePath}");

            if (zPackage == null) return;

            //handle data extraction from file
            bool testData = zPackage.ReadBool();
            Jotunn.Logger.LogMessage($"Just read {testData} from {filePath}");

            }
            catch
            {
                Jotunn.Logger.LogWarning("Didn't load from King Skills");
                return;
            }

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
    }
}
