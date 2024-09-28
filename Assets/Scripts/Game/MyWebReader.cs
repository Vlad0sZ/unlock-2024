using System;
using System.Collections.Generic;
using Backend.Events;
using Backend.Invoker;
using Backend.Registration;
using InGameBehaviours;
using UnityEngine;

namespace Game
{
    public class MyWebReader : MonoHubListener<UserDataEvent, UserDataEvent.UserData>
    {
        public Action<WallDataModel> DataTrigger;

        private readonly Queue<UserDataEvent.UserData> _queueData = new(128);
        private bool _isNeedData;
        private Coroutine _coroutine;

        public void NeedData() => _isNeedData = true;

        protected override void OnValueChanged(UserDataEvent.UserData arg0) =>
            _queueData.Enqueue(arg0);

        public void Update()
        {
            if (_isNeedData == false)
                return;

            if (_queueData.Count == 0)
                return;

            var peek = _queueData.Dequeue();
            var wallData = new WallDataModel()
            {
                UserId = peek.UserId,
                UserName = peek.UserName,
                TaskText = peek.UserCustomData.task,
                Data = peek.UserCustomData.GetImage()
            };

            _isNeedData = false;
            DataTrigger?.Invoke(wallData);
        }

        public void GameWasStarted() =>
            SignalRegistration<ISignalInvoke>.Resolve()?.SendCommandToChangeState(1);

        public void GameWasStopped() =>
            SignalRegistration<ISignalInvoke>.Resolve()?.SendCommandToChangeState(2);
    }
}