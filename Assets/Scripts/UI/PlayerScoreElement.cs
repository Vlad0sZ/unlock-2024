using Models;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerScoreElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text playerScoreText;


        public void SetScore(PlayerWithScore score)
        {
            playerNameText.text = score.Name;
            playerScoreText.text = score.Score.ToString();
        }
    }
}