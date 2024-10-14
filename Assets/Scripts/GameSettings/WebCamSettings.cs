using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameSettings
{
    public class WebCamSettings : MonoBehaviour
    {
        [Serializable]
        public class WebCamChangedEvent : UnityEvent<WebCamDevice>
        {
        }

        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private GameObject noDeviceWarning;
        [SerializeField] private WebCamChangedEvent onWebCamChanged;

        private WebCamDevice[] _devices;
        public WebCamChangedEvent OnWebCamChanged => onWebCamChanged;

        private void Start() =>
            SetupDropdown();

        private void OnEnable() =>
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        private void OnDisable() =>
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);

        private void OnDropdownValueChanged(int selectedIndex)
        {
            if (_devices.Length < selectedIndex)
                dropdown.SetValueWithoutNotify(0);
            else
                ChangeDevice(_devices[selectedIndex]);
        }


        private void SetupDropdown()
        {
            _devices = WebCamTexture.devices;
            dropdown.ClearOptions();

            bool noDevices = _devices.Length == 0;
            noDeviceWarning.SetActive(noDevices);

            if (noDevices)
                return;

            List<TMP_Dropdown.OptionData> options = _devices
                .Select(device => new TMP_Dropdown.OptionData() {text = device.name})
                .ToList();

            dropdown.AddOptions(options);
            var cameraIndex = LoadLastCameraIndex();
            dropdown.SetValueWithoutNotify(cameraIndex);
        }

        private void ChangeDevice(WebCamDevice device)
        {
            Debug.Log($"[WebCam Settings]: Device was changed to {device.name}");
            WebCameraCache.SaveCamera(device);
            onWebCamChanged?.Invoke(device);
        }


        private int LoadLastCameraIndex()
        {
            WebCamDevice? loadDevice = WebCameraCache.LoadCamera();
            if (loadDevice == null)
                return 0;

            int indexDevice = Array.IndexOf(_devices, loadDevice);
            return indexDevice < 0 ? 0 : indexDevice;
        }
    }
}