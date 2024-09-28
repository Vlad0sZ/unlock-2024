using Sirenix.OdinInspector;
using UnityEngine;

public class PlaneCamera : MonoBehaviour
{
    public Camera targetCamera;
    public Vector3 sss;

    [Button]
    private void SetPos()
    {
        transform.LookAt(targetCamera.transform);
        transform.Rotate(sss.x, sss.y, sss.z);
    }
}
