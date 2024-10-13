using System;
using System.Collections;
using Game;
using InGameBehaviours;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Menu
{
    public class StartScreen : ScreenController
    {
        [SerializeField] private VideoPlayer tutorialPlayer;
        [SerializeField] private Button playButton;

        [SerializeField] private UpdateUserListener userListener;
        [SerializeField] protected GameController gameController;

        private Coroutine _coTutorial;

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
            if (_coTutorial != null)
            {
                StopCoroutine(_coTutorial);
                _coTutorial = null;
            }
            _coTutorial = StartCoroutine(StartGame());
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.J))
            {
                if (_coTutorial != null)
                {
                    StopCoroutine(_coTutorial);
                    tutorialPlayer.gameObject.SetActive(false);
                    gameController.StartGame();
                    gameObject.SetActive(false);
                    _coTutorial = null;
                }
            }
        }

        private IEnumerator StartGame()
        {
            tutorialPlayer.gameObject.SetActive(true);
            var clipLength = (float)tutorialPlayer.clip.length;
            tutorialPlayer.Play();
            yield return new WaitForSeconds(clipLength);
            tutorialPlayer.gameObject.SetActive(false);
            gameController.StartGame();
            gameObject.SetActive(false);
            _coTutorial = null;
        }

        private void UpdateUsers(int count) =>
            playButton.interactable = count > 0;
    }
}