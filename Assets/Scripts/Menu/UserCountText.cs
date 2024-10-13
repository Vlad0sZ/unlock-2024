using TMPro;
using UnityEngine;

namespace Menu
{
    public class UserCountText : MonoBehaviour
    {
        [SerializeField] private TMP_Text textComponent;
        [SerializeField] private string userText;
        [SerializeField] private Color countColor;

        public void UpdateCount(int count)
        {
            var colorText = GetCountColor(countColor, count.ToString());
            var text = string.Format(userText, colorText);

            textComponent.text = text;
        }

        private string GetCountColor(Color color, string text) =>
            $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
    }
}