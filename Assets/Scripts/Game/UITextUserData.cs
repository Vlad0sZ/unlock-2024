﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class UITextUserData : MonoBehaviour
    {
        public Color[] itemsColors;
        public Color[] namesColors;
        public string[] formatString;

        public TMP_Text textElement;

        public void ClearText() => textElement.text = string.Empty;
        
        public void SetText(string userName, string itemName)
        {
            var randomString = GetRandom(formatString);
            var randomColor = GetRandom(itemsColors);
            var randomColor2 = GetRandom(namesColors);

            var userColor = GetColorizedRichText(userName, randomColor);
            var itemColor = GetColorizedRichText(itemName, randomColor2);
            var output = string.Format(randomString, userColor, itemColor);

            textElement.text = output;
        }

        private static string GetColorizedRichText(string text, Color color) =>
            $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";

        private static T GetRandom<T>(IReadOnlyList<T> array)
        {
            if (array == null || array.Count == 0)
                return default;

            return array[Random.Range(0, array.Count)];
        }
    }
}