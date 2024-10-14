using System.Linq;
using UnityEngine;

namespace GameSettings
{
    public static class WebCameraCache
    {
        private const string PreferenceKey = "cache_camera";


        public static void SaveCamera(WebCamDevice device) =>
            PlayerPrefs.SetString(PreferenceKey, device.name);


        public static WebCamDevice? LoadCamera()
        {
            string lastCameraDeviceName = PlayerPrefs.GetString(PreferenceKey, string.Empty);
            if (lastCameraDeviceName == null)
                return null;


            var availableDevice = WebCamTexture.devices.ToList();
            var device = availableDevice.FirstOrDefault(x => x.name == lastCameraDeviceName);
            return device;
        }
    }
}