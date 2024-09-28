using System;
using Backend.Events;
using UnityEngine;

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

            
        }
        


        private byte[] GetImage(string base64String) =>
            Convert.FromBase64String(base64String);
    }
}