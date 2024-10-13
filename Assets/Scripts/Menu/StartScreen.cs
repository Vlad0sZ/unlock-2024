using InGameBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class StartScreen : ScreenController
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private UpdateUserListener userListener;
        [SerializeField] private GameController gameController;

        private void OnEnable()
        {
            exitButton.onClick.AddListener(CloseProgram);
            playButton.onClick.AddListener(OnPlay);
            userListener.OnUsersChanged.AddListener(UpdateUsers);
            UpdateUsers(userListener.UserCount);
        }

        private void OnDisable()
        {
            exitButton.onClick.RemoveListener(CloseProgram);
            playButton.onClick.RemoveListener(OnPlay);
            userListener.OnUsersChanged.RemoveListener(UpdateUsers);
        }

        private void UpdateUsers(int count)
        {
            playButton.interactable = count > 0;
        }

        private void OnPlay()
        {
            gameController.StartGame();
            this.gameObject.SetActive(false);
        }

        private void CloseProgram()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}