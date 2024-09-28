using System;
using Backend.Events;

namespace InGameBehaviours
{
    public class DataListener : MonoHubListener<UserDataEvent, UserDataEvent.UserData>
    {
        protected override void OnValueChanged(UserDataEvent.UserData userData)
        {
            // Нужно, чтобы потом этому пользователю (по id) отправить типа "смотри, твоя стена едет"
            string userId = userData.UserId;

            // Имя пользователя
            string userName = userData.UserName;

            // Задание пользователя
            string dataTask = userData.DrawTask;

            // Картинка пользователя 128 * 128 размером. 1 - черное, 0 - белое
            byte[] imageBytes = GetImage(userData.UserDrawBase64);
        }


        private byte[] GetImage(string base64String) =>
            Convert.FromBase64String(base64String);
    }
}