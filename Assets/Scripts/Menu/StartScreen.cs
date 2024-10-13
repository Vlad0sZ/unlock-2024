using InGameBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class StartScreen : ScreenController
    {
        [SerializeField] private Button playButton;

        [SerializeField] private UpdateUserListener userListener;
        [SerializeField] protected GameController gameController;

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnPlay);
            userListener.OnUsersChanged.AddListener(UpdateUsers);
            UpdateUsers(userListener.UserCount);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnPlay);
            userListener.OnUsersChanged.RemoveListener(UpdateUsers);
        }

        protected virtual void OnPlay()
        {
            // TODO show tutorial tutorial
            gameController.StartGame();
            gameObject.SetActive(false);
        }

        private void UpdateUsers(int count) =>
            playButton.interactable = count > 0;
    }
}