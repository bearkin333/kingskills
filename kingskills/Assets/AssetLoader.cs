using Jotunn.Managers;
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
        public static string filePath = "kingskills/Assets/";

        public static Dictionary<string, Sprite> perkBoxSprites;
        public static AssetBundle assets;


        public static void InitAssets()
        {
            perkBoxSprites = new Dictionary<string, Sprite>();


            perkBoxSprites.Add("perkbox", LoadSpriteFromFilename("perkbox.png"));
            perkBoxSprites.Add("perkboxLocked", LoadSpriteFromFilename("perkboxLocked.png"));
            perkBoxSprites.Add("graytint", LoadSpriteFromFilename("graytint.png"));
            perkBoxSprites.Add("goldtint", LoadSpriteFromFilename("goldtint.png"));
        }


        public static Sprite LoadSpriteFromFilename(string filename)
        {
            //Jotunn.Logger.LogMessage($"Attempting to load a sprite from bepinex/plugins/{filePath + filename}");
            try
            {
                Texture2D texture = AssetUtils.LoadTexture(filePath + filename);
                return Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height),
                    new Vector2(0.5f, 0.5f));
            }
            catch
            {
                Jotunn.Logger.LogMessage($"Could not load sprite '{filename}'");
                return GUIManager.Instance.GetSprite("ArmorBronzeChest");
            }
        }
    }
}
