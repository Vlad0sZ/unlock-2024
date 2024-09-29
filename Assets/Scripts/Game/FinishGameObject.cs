using TMPro;
using UnityEngine;

internal class FinishGameObject : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    public void ShowFinishGame(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = $"{score}";
    }
}