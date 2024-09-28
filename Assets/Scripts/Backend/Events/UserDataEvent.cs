using System;
using Backend.Registration;
using Newtonsoft.Json;

namespace Backend.Events
{
    public class UserDataEvent : JsonEvent<UserDataEvent.UserData>
    {
        protected override string MethodName => "image";

        private void Start() =>
            SignalRegistration<UserDataEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<UserDataEvent>.Unregister();

        public struct UserData
        {
            [JsonProperty("userId")] public string UserId;

            [JsonProperty("userName")] public string UserName;

            [JsonProperty("data")] public ImageData UserCustomData;
        }

        public struct ImageData
        {
            [JsonProperty("task")] public string task;

            [JsonProperty("image")] public string encryptedImage;

            public byte[] GetImage()
            {
                if (string.IsNullOrEmpty(encryptedImage))
                    return Array.Empty<byte>();

                return Convert.FromBase64String(encryptedImage);
            }
        }
    }
}