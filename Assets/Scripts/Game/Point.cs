using System;
using UnityEngine;

public class Point : MonoBehaviour
{
    public event Action<GameObject> Trigger;
    private void OnTriggerEnter(Collider other)
    {
        Trigger?.Invoke(other.gameObject);
    }
}
