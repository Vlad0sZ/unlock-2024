using Backend.Events;

namespace InGameBehaviours
{
    public class RomeCodeSpawn : MonoHubListener<RoomCodeEvent, RoomCodeEvent.Room>
    {
        protected override void OnEnable() => 
            OnValueChanged(Event.Data);

        protected override void OnValueChanged(RoomCodeEvent.Room code)
        {
            string roomCode = code.Code;

            // TODO Теперь заспавни этот код на сцене
        }
    }
}