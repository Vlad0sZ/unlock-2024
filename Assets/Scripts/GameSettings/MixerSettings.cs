using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameSettings
{
    public class MixerSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private Slider slider;
        [SerializeField] private string levelName;

        private Coroutine _saveRoutine;

        private void Start()
        {
            float restoreValue = RestoreFromSettings();
            slider.SetValueWithoutNotify(restoreValue);
            UpdateMixerValue(restoreValue);
        }

        private void OnEnable() =>
            slider.onValueChanged.AddListener(OnSliderChanged);

        private void OnDisable() =>
            slider.onValueChanged.RemoveListener(OnSliderChanged);

        private void OnSliderChanged(float value)
        {
            if (_saveRoutine != null)
                StopCoroutine(_saveRoutine);

            _saveRoutine = StartCoroutine(SaveAfter(value));
            UpdateMixerValue(value);
        }


        private float RestoreFromSettings() =>
            !string.IsNullOrEmpty(levelName) ? PlayerPrefs.GetFloat(GetPreferenceKey(), 0.5f) : 0f;

        private void SaveToSettings(float value)
        {
            if (!string.IsNullOrEmpty(levelName))
                PlayerPrefs.SetFloat(GetPreferenceKey(), value);
        }

        private void UpdateMixerValue(float value)
        {
            if (!string.IsNullOrEmpty(levelName))
                mixer.SetFloat(levelName, ToMixerValue(value));
        }

        private string GetPreferenceKey() => $"audio_{levelName}";

        private IEnumerator SaveAfter(float value)
        {
            yield return new WaitForSecondsRealtime(1f);
            SaveToSettings(value);
        }

        private float ToMixerValue(float value)
        {
            if (value == 0)
                return -80;

            return Mathf.Log10(value) * 20;
        }

        [Button]
        private void ClearPreference()
        {
            if (!string.IsNullOrEmpty(levelName))
                PlayerPrefs.DeleteKey(GetPreferenceKey());
        }
    }
}