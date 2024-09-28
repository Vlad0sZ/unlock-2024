using Backend.Events;

namespace InGameBehaviours
{
    public class UpdateUsers : MonoHubListener<UpdateUsersEvent, UpdateUsersEvent.UsersCount>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            OnValueChanged(Event.Data);
        }

        protected override void OnValueChanged(UpdateUsersEvent.UsersCount data)
        {
            int users = data.Count;
            
            // TODO кол-во игроков (постоянно меняется)
        }
    }
}