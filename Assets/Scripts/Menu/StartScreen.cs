using Game;
using UnityEngine;

namespace Menu
{
    public class StartScreen : ScreenWithButtons
    {
        [SerializeField] private Tutorial tutorialPlayer;

        protected override void OnPlay() =>
            tutorialPlayer.ShowTutorial(StartGame);

        private void StartGame()
        {
            GameController.StartGame();
            gameObject.SetActive(false);
        }
    }
}