using Cysharp.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Backend.Invoker
{
    internal sealed class SignalInvoke : ISignalInvoke
    {
        private readonly HubConnection _connection;

        public SignalInvoke(HubConnection connection) =>
            _connection = connection;


        public bool WithConnection => _connection.State == HubConnectionState.Connected;

        public async UniTask SendCommandToChangeState(int state)
        {
            if (_connection.State != HubConnectionState.Connected)
                return;

            await SendMethod("gameState", state);
        }

        private async UniTask SendMethod(string method, object data)
        {
            var sendData = $"{method}:{data}";
            await _connection.SendAsync("sendtoclients", sendData);
        }
    }
}