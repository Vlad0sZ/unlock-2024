using System;
using System.Collections;
using System.Collections.Generic;
using MediaPipeUnity.Backend;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private float maxRadiusHead;
    [SerializeField] private PoseController poseController;
    [SerializeField] private List<LineRenderer> lines;
    [SerializeField] private LineRenderer headLine;
    [SerializeField] private List<Point> points;

    [SerializeField] private Transform ldl;
    [SerializeField] private Transform rdl;
    [SerializeField] private Transform ldk;
    [SerializeField] private Transform rdk;
    [SerializeField] private Transform p;
    [SerializeField] private Transform h;
    [SerializeField] private Transform ls;
    [SerializeField] private Transform rs;
    [SerializeField] private Transform lmu;
    [SerializeField] private Transform rmu;
    [SerializeField] private Transform lhu;
    [SerializeField] private Transform rhu;
    [SerializeField] private int segments = 100;
    public event Action<GameObject> Trigger; 
    
    private void Awake()
    {
        foreach (var point in points)
        {
            point.Trigger += TriggerHandle;
        }
    }

    private void Update()
    {
        var body = poseController.DetectedBody;

        if (body.IsDetected)
        {
            ldl.transform.position = body[28].Position; 
            rdl.transform.position = body[27].Position; 
            ldk.transform.position = body[26].Position; 
            rdk.transform.position = body[25].Position; 
            p.transform.position = (body[24].Position+body[23].Position)/2f; 
            h.transform.position = body[0].Position; 
            ls.transform.position = body[12].Position; 
            rs.transform.position = body[11].Position; 
            lmu.transform.position = body[14].Position; 
            rmu.transform.position = body[13].Position; 
            lhu.transform.position = body[16].Position; 
            rhu.transform.position = body[15].Position; 
        }
        
        lines[0].SetPositions(new []{ ldl.position, ldk.position });
        lines[1].SetPositions(new []{ rdl.position, rdk.position });
        lines[2].SetPositions(new []{ ldk.position, p.position });
        lines[3].SetPositions(new []{ rdk.position, p.position });
        var s = new Vector3(
            (ls.position.x + rs.position.x) / 2f, 
            (ls.position.y + rs.position.y) / 2f+0.1f,
            (ls.position.z + rs.position.z) / 2f
            );
        lines[4].SetPositions(new []{ s, p.position });
        lines[5].SetPositions(new []{ s, ls.position });
        lines[6].SetPositions(new []{ s, rs.position });
        lines[7].SetPositions(new []{ lmu.position, ls.position });
        lines[8].SetPositions(new []{ rmu.position, rs.position });
        lines[9].SetPositions(new []{ lmu.position, lhu.position });
        lines[10].SetPositions(new []{ rmu.position, rhu.position });

        var sp = h.position - s;
        sp *= Mathf.Min(sp.magnitude, maxRadiusHead);
        var headCenter = s + sp;
        DrawCircle(headCenter, Vector3.Distance(s, sp));
        //lines[5].SetPositions(new []{ s, h.position });
    }
    
    void DrawCircle(Vector3 center, float radius)
    {
        headLine.positionCount = segments + 1; // +1 чтобы замкнуть круг
        headLine.useWorldSpace = true; // Рисуем в мировых координатах

        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            Vector3 position = new Vector3(x, y, 0) + center;
            headLine.SetPosition(i, position);
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
