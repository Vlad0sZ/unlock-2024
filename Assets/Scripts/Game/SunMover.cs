using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SunMover : MonoBehaviour
{
    // Ссылка на SplineContainer
    [SerializeField] private SplineContainer splineContainer;

    // Время в секундах для полного прохождения сплайна
    [SerializeField] private float duration; // 2 минуты

    private float elapsedTime = 0f;

    // Начальная позиция спрайта на сплайне
    public float progress { get; protected set; }

    public bool IsDone = true;

    public void StartMove()
    {
        IsDone = false;
        elapsedTime = 0;
    }

    // Update вызывается каждый кадр
    void Update()
    {
        if(!IsDone)
        {
            // Увеличиваем время
            elapsedTime += Time.deltaTime;

            // Рассчитываем процент прогресса вдоль сплайна (от 0 до 1)
            progress = Mathf.Clamp01(elapsedTime / duration);

            if ((elapsedTime / duration) >= 1f)
            {
                IsDone = true;
            }

            // Получаем позицию на сплайне по прогрессу
            Vector3 splinePosition = splineContainer.Spline.EvaluatePosition(progress);

            // Перемещаем объект в новую позицию на сплайне, без изменения поворота
            transform.localPosition = splinePosition;
        }
    }
}
