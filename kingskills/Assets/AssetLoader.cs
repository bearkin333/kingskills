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
        public static Dictionary<string, Sprite> skillIconSprites;
        public static AssetBundle assets;


        public static void InitAssets()
        {
            perkBoxSprites = new Dictionary<string, Sprite>();
            skillIconSprites = new Dictionary<string, Sprite>();

            perkBoxSprites.Add("skillbox", LoadSpriteFromFilename("skillbox.png"));
            perkBoxSprites.Add("perkbox", LoadSpriteFromFilename("perkbox.png"));
            perkBoxSprites.Add("perkboxLocked", LoadSpriteFromFilename("perkboxLocked.png"));
            perkBoxSprites.Add("graytint", LoadSpriteFromFilename("graytint.png"));
            perkBoxSprites.Add("goldtint", LoadSpriteFromFilename("goldtint.png"));
            perkBoxSprites.Add("deactivated", LoadSpriteFromFilename("deactivated.png"));

            skillIconSprites.Add("Agriculture", LoadSpriteFromFilename("SkillIcons/Agriculture.png"));
            skillIconSprites.Add("Axes", LoadSpriteFromFilename("SkillIcons/Axes.png"));
            skillIconSprites.Add("Blocking", LoadSpriteFromFilename("SkillIcons/Blocking.png"));
            skillIconSprites.Add("Blood Magic", LoadSpriteFromFilename("SkillIcons/BloodMagic.png"));
            skillIconSprites.Add("Bows", LoadSpriteFromFilename("SkillIcons/Bows.png"));
            skillIconSprites.Add("Building", LoadSpriteFromFilename("SkillIcons/Building.png"));
            skillIconSprites.Add("Clubs", LoadSpriteFromFilename("SkillIcons/Clubs.png"));
            skillIconSprites.Add("Cooking", LoadSpriteFromFilename("SkillIcons/Cooking.png"));
            skillIconSprites.Add("Crossbows", LoadSpriteFromFilename("SkillIcons/Crossbows.png"));
            skillIconSprites.Add("Elemental Magic", LoadSpriteFromFilename("SkillIcons/ElementalMagic.png"));
            skillIconSprites.Add("Fishing", LoadSpriteFromFilename("SkillIcons/Fishing.png"));
            skillIconSprites.Add("Fists", LoadSpriteFromFilename("SkillIcons/Fists.png"));
            skillIconSprites.Add("Jump", LoadSpriteFromFilename("SkillIcons/Jump.png"));
            skillIconSprites.Add("Knives", LoadSpriteFromFilename("SkillIcons/Knives.png"));
            skillIconSprites.Add("Mining", LoadSpriteFromFilename("SkillIcons/Mining.png"));
            skillIconSprites.Add("Polearms", LoadSpriteFromFilename("SkillIcons/Polearms.png"));
            skillIconSprites.Add("Run", LoadSpriteFromFilename("SkillIcons/Run.png"));
            skillIconSprites.Add("Sailing", LoadSpriteFromFilename("SkillIcons/Sailing.png"));
            skillIconSprites.Add("Spears", LoadSpriteFromFilename("SkillIcons/Spears.png"));
            skillIconSprites.Add("Sneak", LoadSpriteFromFilename("SkillIcons/Sneak.png"));
            skillIconSprites.Add("Swim", LoadSpriteFromFilename("SkillIcons/Swim.png"));
            skillIconSprites.Add("Swords", LoadSpriteFromFilename("SkillIcons/Swords.png"));
            skillIconSprites.Add("Woodcutting", LoadSpriteFromFilename("SkillIcons/Woodcutting.png"));
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
                //Jotunn.Logger.LogMessage($"Could not load sprite '{filename}'");
                return GUIManager.Instance.GetSprite("ArmorBronzeChest");
            }
        }
    }
}
