﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace kingskills
{
    [HarmonyPatch]
    class CustomWorldTextManager
    {
        public const float InFrontRange = 2f;
        public const float AboveRange = 2.7f;
        public const float RandomRange = 0.8f;

        public static Vector3 GetInFrontOfCharacter(Character character)
        {
            return character.transform.position + character.transform.forward * InFrontRange;
        }
        public static Vector3 GetAboveCharacter(Character character)
        {
            return character.transform.position + Vector3.up * AboveRange;
        }

        public static Vector3 GetRandomPosOffset()
        {
            float x = UnityEngine.Random.Range(-RandomRange / 2, RandomRange / 2);
            float y = UnityEngine.Random.Range(-RandomRange / 2, RandomRange / 2);
            float z = UnityEngine.Random.Range(-RandomRange / 2, RandomRange / 2);
            return new Vector3(x, y, z);
        }

        //[HarmonyPatch(typeof(DamageText))]
        //[HarmonyPatch(nameof(DamageText.UpdateWorldTexts))]
        //[HarmonyPrefix]
        public static void AddCustomWorldText(Color msgColor, Vector3 pos, int fontSize, string text, bool useColor = true)
        {
            DamageText.WorldTextInstance worldText = new DamageText.WorldTextInstance();
            worldText.m_worldPos = pos;
            worldText.m_gui = UnityEngine.Object.Instantiate<GameObject>
                (DamageText.instance.m_worldTextBase, DamageText.instance.transform);
            worldText.m_textField = worldText.m_gui.GetComponent<Text>();
            DamageText.instance.m_worldTexts.Add(worldText);

            //Jotunn.Logger.LogMessage($"spawning message of font size {fontSize}");
            worldText.m_textField.color = msgColor;
            worldText.m_textField.text = text;
            worldText.m_textField.fontSize = fontSize;
            worldText.m_textField.horizontalOverflow = HorizontalWrapMode.Overflow;
            worldText.m_textField.verticalOverflow = VerticalWrapMode.Overflow;
            worldText.m_timer = 0f;
        }
    }
}
