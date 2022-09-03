using Jotunn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills.Assets
{
    public static class AssetLoader
    {
        public const string filePath = "kingskills/Assets/";
        public static Dictionary<string, Sprite> perkSprites;


        public static void InitPerkSprites()
        {
            perkSprites = new Dictionary<string, Sprite>();

            perkSprites.Add("perkbox",
                LoadSpriteFromFilename("perkbox.png"));
            perkSprites.Add("perkboxLocked",
                LoadSpriteFromFilename("perkboxLocked.png"));
        }


        public static Sprite LoadSpriteFromFilename(string filename)
        {
            Jotunn.Logger.LogMessage($"Loading a sprite from {filePath + filename}");
            Texture2D texture = AssetUtils.LoadTexture(filePath + filename);
            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
            return sprite;
        }
    }
}
