using System.Collections.Generic;
using MediaPipeUnity.Backend;
using UnityEngine;

public class DrawBody : MonoBehaviour
{
    [SerializeField] private PoseController poseController;
    [SerializeField] private Transform box;

    [SerializeField] private Color jointColor;
    [SerializeField] private Color jointColorInvisible;
    [SerializeField] private Color jointColorInpresence;
    [SerializeField] private Color jointLineColor;
    [SerializeField] private float gizmoSize;

    
    /// <summary>
    /// ПРИМЕР ИСПОЛЬЗОВАНИЯ!!!!!!!!!!!
    /// </summary>
    private void Update()
    {
        var body = poseController.DetectedBody;

        if (body.IsDetected)
        {
            box.transform.position = body[PoseJoint.LeftWrist].Position;
            box.gameObject.SetActive(true);
        }
        else
        {
            box.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        var body = poseController.DetectedBody;
        if (!body.IsDetected)
            return;


        foreach (var joint in body.Joints)
        {
            Color color;
            if (!joint.IsPresence)
                color = jointColorInpresence;
            else if (!joint.IsVisible)
                color = jointColorInvisible;
            else
                color = jointColor;

            Gizmos.color = color;
            Gizmos.DrawSphere(joint.Position, gizmoSize);
        }

        Gizmos.color = jointLineColor;
        foreach ((PoseJoint, PoseJoint) tuple in Lines)
        {
            var aJoint = body[tuple.Item1];
            var bJoints = body[tuple.Item2];
            Gizmos.DrawLine(aJoint.Position, bJoints.Position);
        }
    }

    private static readonly List<(PoseJoint, PoseJoint)> Lines = new List<(PoseJoint, PoseJoint)>()
    {
        (PoseJoint.Nose, PoseJoint.RightEyeInner),
        (PoseJoint.Nose, PoseJoint.LeftEyeInner),

        (PoseJoint.LeftEyeInner, PoseJoint.LeftEye),
        (PoseJoint.LeftEye, PoseJoint.LeftEyeOuter),
        (PoseJoint.LeftEyeOuter, PoseJoint.LeftEar),

        (PoseJoint.RightEyeInner, PoseJoint.RightEye),
        (PoseJoint.RightEye, PoseJoint.RightEyeOuter),
        (PoseJoint.RightEyeOuter, PoseJoint.RightEar),

        (PoseJoint.LeftShoulder, PoseJoint.RightShoulder),
        (PoseJoint.LeftShoulder, PoseJoint.LeftHip),
        (PoseJoint.LeftShoulder, PoseJoint.LeftElbow),

        (PoseJoint.RightShoulder, PoseJoint.RightElbow),
        (PoseJoint.RightShoulder, PoseJoint.RightHip),

        (PoseJoint.RightHip, PoseJoint.LeftHip),


        (PoseJoint.LeftElbow, PoseJoint.LeftWrist),
        (PoseJoint.LeftWrist, PoseJoint.LeftThumb),
        (PoseJoint.LeftWrist, PoseJoint.LeftIndex),
        (PoseJoint.LeftWrist, PoseJoint.LeftPinky),
        (PoseJoint.LeftPinky, PoseJoint.LeftIndex),

        (PoseJoint.RightElbow, PoseJoint.RightWrist),
        (PoseJoint.RightWrist, PoseJoint.RightThumb),
        (PoseJoint.RightWrist, PoseJoint.RightIndex),
        (PoseJoint.RightWrist, PoseJoint.RightPinky),
        (PoseJoint.RightPinky, PoseJoint.RightIndex),

        (PoseJoint.RightHip, PoseJoint.RightKnee),
        (PoseJoint.RightKnee, PoseJoint.RightAnkle),
        (PoseJoint.RightKnee, PoseJoint.RightHeel),
        (PoseJoint.RightAnkle, PoseJoint.RightFootIndex),
        (PoseJoint.RightFootIndex, PoseJoint.RightHeel),

        (PoseJoint.LeftHip, PoseJoint.LeftKnee),
        (PoseJoint.LeftKnee, PoseJoint.LeftAnkle),
        (PoseJoint.LeftKnee, PoseJoint.LeftHeel),
        (PoseJoint.LeftAnkle, PoseJoint.LeftFootIndex),
        (PoseJoint.LeftFootIndex, PoseJoint.LeftHeel),
    };
}