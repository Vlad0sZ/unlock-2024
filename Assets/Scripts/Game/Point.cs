using System;
using UnityEngine;

public class Point : MonoBehaviour
{
    public event Action<GameObject> Trigger;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name}    {other.gameObject.name}");
        Trigger?.Invoke(other.gameObject);
    }
}
