using Game;
using InGameBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public abstract class ScreenWithButtons : ScreenController
    {
        [SerializeField] private Button playButton;

        [SerializeField] private UpdateUserListener userListener;
        [SerializeField] private GameController gameController;

        protected GameController GameController => gameController;

        protected virtual void OnEnable()
        {
            playButton.onClick.AddListener(OnPlay);
            userListener.OnUsersChanged.AddListener(UpdateUsers);
            UpdateUsers(userListener.UserCount);
            Debug.Log($"OnEnable on {gameObject} count is {userListener.UserCount}");
        }

        protected virtual void OnDisable()
        {
            playButton.onClick.RemoveListener(OnPlay);
            userListener.OnUsersChanged.RemoveListener(UpdateUsers);
        }

        private void UpdateUsers(int count) =>
            playButton.interactable = count > 0;

        protected abstract void OnPlay();
    }
}