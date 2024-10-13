using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Game
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private Button skipButton;

        private Coroutine _tutorialCoroutine;
        private Action _lastAction;

        private void OnEnable() =>
            skipButton.onClick.AddListener(SkipTutorial);

        private void OnDisable() =>
            skipButton.onClick.RemoveListener(SkipTutorial);


        public void ShowTutorial(Action afterAction)
        {
            if (_tutorialCoroutine != null)
            {
                StopCoroutine(_tutorialCoroutine);
                _tutorialCoroutine = null;
            }

            _lastAction = afterAction;
            _tutorialCoroutine = StartCoroutine(TutorialRoutine());
        }

        private void SkipTutorial()
        {
            if (_tutorialCoroutine == null)
                return;

            videoPlayer.Stop();
            videoPlayer.gameObject.SetActive(false);
            _lastAction?.Invoke();
            _tutorialCoroutine = null;
        }

        private IEnumerator TutorialRoutine()
        {
            GameStateController.CurrentState = GameState.Tutorial;
            videoPlayer.gameObject.SetActive(true);
            var clipLength = (float) videoPlayer.clip.length;
            videoPlayer.Play();
            yield return new WaitForSeconds(clipLength);
            SkipTutorial();
        }
    }
}