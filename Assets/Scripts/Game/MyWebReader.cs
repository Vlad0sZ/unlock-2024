using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class MyWebReader: MonoBehaviour
{
    private string _url="";
    private bool _isNeedData;

    public Action<WallDataModel> DataTrigger;

    public void NeedData()
    {
        StartCoroutine(SendGetRequest());
    }

    public void GameStart()
    {
        
    }
    
    public void GameEnd()
    {
        
    }

    IEnumerator SendGetRequest()
    {
        while (true)
        {
            if (_url == "") break;
            using (var webRequest = UnityWebRequest.Get(_url))
            {
                // Отправляем запрос и ожидаем ответ
                yield return webRequest.SendWebRequest();

                // Проверяем на ошибки
                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Ошибка: {webRequest.error}");
                }
                else
                {
                    // Получаем ответ как текст
                    string responseText = webRequest.downloadHandler.text;
                    var data = JsonConvert.DeserializeObject<WallDataModel>(responseText);
                    if (data != null)
                    {
                        Debug.Log($"Ответ сервера: {responseText}");
                        DataTrigger?.Invoke(data);
                        break;
                    }
                }
            }
        }
    }
}