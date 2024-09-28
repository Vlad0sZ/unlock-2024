using Backend.Invoker;
using Backend.Registration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameEndBehavior : MonoBehaviour
    {
        public void ReloadGame()
        {
            SceneManager.LoadScene("MainScene");
            var invoker = SignalRegistration<ISignalInvoke>.Resolve();
            invoker.SendCommandToChangeState(0);
        }
    }
}