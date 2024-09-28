using Backend.Events;
using TMPro;
using UnityEngine;

namespace InGameBehaviours
{
    public class UpdateUsers : MonoHubListener<UpdateUsersEvent, UpdateUsersEvent.UsersCount>
    {
        [SerializeField] private TMP_Text textComponent;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (Event != null)
                OnValueChanged(Event.Data);
        }

        protected override void OnValueChanged(UpdateUsersEvent.UsersCount data)
        {
            int users = data.Count;
            textComponent.text = $"{users}";
            // TODO кол-во игроков (постоянно меняется)
        }
    }
}