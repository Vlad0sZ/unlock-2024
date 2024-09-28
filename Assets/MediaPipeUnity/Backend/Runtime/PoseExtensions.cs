using Mediapipe.Tasks.Components.Containers;
using UnityEngine;

namespace MediaPipeUnity.Backend
{
    public static class PoseExtensions
    {
        public static Vector3 ToVector3(this NormalizedLandmark landmark) =>
            new(landmark.x, -landmark.y, landmark.z);

        public static Vector3 ToVector3(this Landmark landmark) =>
            new(landmark.x, -landmark.y, landmark.z);
    }
}