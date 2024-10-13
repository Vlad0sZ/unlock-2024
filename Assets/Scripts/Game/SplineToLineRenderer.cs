using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer))]
public class SplineToLineRenderer : MonoBehaviour
{
    public SplineContainer splineContainer;  // Ссылка на SplineContainer
    public LineRenderer lineRenderer;       // Ссылка на LineRenderer
    public int resolution = 50;        // Количество точек на один сегмент сплайна для сглаживания
    
    [Button]
    void BuildLineFromSpline()
    {
        Spline spline = splineContainer.Spline;  // Получаем сплайн из контейнера

        // Устанавливаем количество точек в LineRenderer
        lineRenderer.positionCount = resolution;

        // Интерполируем вдоль всей длины сплайна
        for (int i = 0; i < resolution; i++)
        {
            // t — нормализованная позиция вдоль сплайна (от 0 до 1)
            float t = i / (float)(resolution - 1);
            Vector3 position = spline.EvaluatePosition(t);  // Получаем позицию на сплайне

            // Устанавливаем позицию в LineRenderer
            lineRenderer.SetPosition(i, position);
        }
    }
}