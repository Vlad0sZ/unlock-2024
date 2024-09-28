using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private List<Point> points;

    public event Action<GameObject> Trigger; 
    
    private void Awake()
    {
        foreach (var point in points)
        {
            point.Trigger += TriggerHandle;
        }
    }
    
    private void TriggerHandle(GameObject obj)
    {
        if (!obj.TryGetComponent<Point>(out _))
        {
            Trigger?.Invoke(obj);
        }
    }
    
    private void OnDestroy()
    {
        foreach (var point in points)
        {
            point.Trigger -= TriggerHandle;
        }
    }
}
