using System.Linq;
using UnityEngine;

namespace Game
{
    public class AudioController : MonoBehaviour
    {
        [System.Serializable]
        private class AudioClipWithState
        {
            public GameState gameState;
            public AudioClip audioClip;
        }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource effectsSource;
        
        [SerializeField] private AudioClip successAudioClip;
        [SerializeField] private AudioClip failAudioClip;
        [SerializeField] private AudioClip finishGameClip;

        [SerializeField] private AudioClipWithState[] audioClips;
        
        private void OnEnable() => 
            GameStateController.OnStateChanged += OnStateChanged;

        private void OnDisable() => 
            GameStateController.OnStateChanged -= OnStateChanged;

        private void OnStateChanged(GameState obj)
        {
            var clip = audioClips.FirstOrDefault(x => x.gameState == obj);
            if (clip != null && clip.audioClip != null) 
                ChangeAudioClip(clip.audioClip);
        }

        private void ChangeAudioClip(AudioClip audioClip)
        {
            musicSource.Stop();
            musicSource.clip = audioClip;
            musicSource.Play();
        }

        public void PlaySuccess(float volume = 1f) => 
            effectsSource.PlayOneShot(successAudioClip, volume);

        public void PlayFail(float volume = 1f) => 
            effectsSource.PlayOneShot(failAudioClip, volume);

        public void PlayFinished()
        {
            OnStateChanged(GameState.MainMenu);
            musicSource.PlayOneShot(finishGameClip);
        }
    }
}