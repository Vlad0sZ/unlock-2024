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

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip successAudioClip;
        [SerializeField] private AudioClip failAudioClip;
        
        [SerializeField] private AudioClip menuAudioClip;
        [SerializeField] private AudioClip mainAudioClip;
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
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}