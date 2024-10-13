using System.Collections;
using System.Collections.Generic;
using Game;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private StartScreen startScreen;
        [SerializeField] private SettingsScreen settingsScreen;
        [SerializeField] private FinalScreen finalScreen;

        [SerializeField] private Button[] applicationCloseButtons;

        private ScreenController[] _screens;

        private void Awake()
        {
            _screens = new ScreenController[] {startScreen, settingsScreen, finalScreen};
        }

        public IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            for (var i = 1; i < _screens.Length; i++) 
                _screens[i].gameObject.SetActive(false);

            foreach (var closeButton in applicationCloseButtons) 
                closeButton.onClick.AddListener(CloseApp);
            
            GameStateController.CurrentState = GameState.MainMenu;
        }

        private void CloseApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void ShowFinishGame(IList<PlayerWithScore> playerWithScores)
        {
            finalScreen.gameObject.SetActive(true);
            finalScreen.UpdateScores(playerWithScores);
        }
    }
}