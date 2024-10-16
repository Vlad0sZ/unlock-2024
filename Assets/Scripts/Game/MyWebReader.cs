﻿using System;
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

        private readonly HashSet<string> _userQueue = new HashSet<string>(128);
        private readonly Queue<UserDataEvent.UserData> _queueData = new(128);
        private bool _isNeedData;
        private Coroutine _coroutine;

        public void NeedData() => _isNeedData = true;

        protected override void OnValueChanged(UserDataEvent.UserData data)
        {
            if (!_userQueue.Add(data.UserId))
                return;

            _queueData.Enqueue(data);
        }

        public void Update()
        {
            if (_isNeedData == false)
                return;

            if (_queueData.Count == 0)
                return;

            var peek = _queueData.Dequeue();
            _userQueue.Remove(peek.UserId);

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
    }
}