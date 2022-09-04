using Jotunn.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Assets
{
    public static class AssetLoader
    {
        //public static string filePath = "/Assets/";
        //public const string assetPath = "Assets/";
        public static string Folder;

        public static Dictionary<string, Sprite> perkSprites;
        public static AssetBundle assets;


        public static void InitAssets()
        {
            perkSprites = new Dictionary<string, Sprite>();
            /*
            assets = AssetUtils.LoadAssetBundleFromResources("ks_assetbundle");

            perkSprites.Add("perkbox",
                LoadSpriteFromFilename("Assets/perkbox.png"));
            perkSprites.Add("perkboxLocked",
                LoadSpriteFromFilename("Assets/perkboxLocked.png"));
            */

            Jotunn.Logger.LogMessage("King Skills attempting to find Assets/ in the directory with " + Folder);

            Texture2D perk = LoadTextureFromAssets("perkbox.png");
            Sprite perkS = Sprite.Create(perk,
                new Rect(0f, 0f, (float)perk.width, (float)perk.height), new Vector2(0.5f, 0.5f));
            perkSprites.Add("perkbox", perkS);

            Texture2D perkLock = LoadTextureFromAssets("perkboxLocked.png");
            Sprite perkLockS = Sprite.Create(perkLock,
                new Rect(0f, 0f, (float)perkLock.width, (float)perkLock.height), new Vector2(0.5f, 0.5f));
            perkSprites.Add("perkbox", perkLockS);
        }



            public static Sprite LoadSpriteFromFilename(string filename)
        {
            //Jotunn.Logger.LogMessage($"Loading a sprite from {filePath + filename}");

            return assets.LoadAsset<Sprite>(filename);
            
            /*
             * byte[] data = File.ReadAllBytes(Path.Combine(filePath, assetPath, filename));

            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(data);
            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
            return sprite;
            */
        }

        public static Texture2D LoadTextureFromAssets(string path)
        {
            try
            {
                byte[] data = File.ReadAllBytes(Path.Combine(Folder, "Assets/", path));
                Texture2D texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(data);
                return texture2D;
            }
            catch
            {
                byte[] data = File.ReadAllBytes(Path.Combine(Folder, path));
                Texture2D texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(data);
                return texture2D;
            }
        }
    }
}
