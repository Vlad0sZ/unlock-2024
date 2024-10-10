using Backend.Events;
using TMPro;
using UnityEngine;

namespace InGameBehaviours
{
    public class RoomCodeSpawn : MonoHubListener<RoomCodeEvent, RoomCodeEvent.Room>
    {
        [SerializeField] private TMP_Text textComponent;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (Event)
                OnValueChanged(Event.Data);
        }

        protected override void OnValueChanged(RoomCodeEvent.Room data)
        {
            string roomCode = data.Code.ToUpper();
            textComponent.text = roomCode;
        }
    }
}