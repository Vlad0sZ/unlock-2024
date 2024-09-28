using Newtonsoft.Json;

namespace Backend.Events
{
    public class UserDataEvent : AbstractEvent<string, UserDataEvent.UserData>
    {
        protected override string MethodName => "data";

        protected override UserData ConvertToOutput(string input) =>
            JsonConvert.DeserializeObject<UserData>(input);

        public struct UserData
        {
            [JsonProperty("id")] public string UserId;

            [JsonProperty("name")] public string UserName;
            
            [JsonProperty("draw_task")] public string DrawTask;

            [JsonProperty("data")] public string UserDrawBase64;
        }
    }
}