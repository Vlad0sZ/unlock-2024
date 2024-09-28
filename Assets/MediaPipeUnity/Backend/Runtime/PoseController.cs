using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using UnityEngine;

namespace MediaPipeUnity.Backend
{
    public class PoseController : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        public Body DetectedBody { get; private set; }

        private readonly object _lockObject = new();
        private PoseLandmarkerResult _lastPoseResult;

        private Camera _mainCamera;
        private bool _wasUpdated;

        private void Start()
        {
            if (targetCamera)
                _mainCamera = targetCamera;
            else
                _mainCamera = Camera.main;
        }


        public void UpdatePose(PoseLandmarkerResult poseResult)
        {
            lock (_lockObject)
            {
                poseResult.CloneTo(ref _lastPoseResult);
                _wasUpdated = true;
            }
        }


        private void LateUpdate()
        {
            if (!_wasUpdated)
                return;

            var worldLandmarks = _lastPoseResult.poseLandmarks;
            if (worldLandmarks == null || worldLandmarks.Count == 0)
            {
                DetectedBody = default;
                return;
            }

            var firstBody = worldLandmarks[0];
            UpdateDetectedBody(firstBody);
            _wasUpdated = false;
        }

        private void UpdateDetectedBody(NormalizedLandmarks landmark)
        {
            int count = landmark.landmarks.Count;

            var landmarks = landmark.landmarks;
            var joints = new Joint[count];
            for (int i = 0; i < count; i++)
            {
                var j = landmarks[i];

                var type = (PoseJoint) i;
                
                var position = GetCachedOrCalculatePosition(j, i);
                var visibility = j.visibility >= 0.75f;
                var presence = j.presence >= 0.75f;

                joints[i] = new Joint(type, position, visibility, presence);
            }

            DetectedBody = new Body()
            {
                Joints = joints,
                IsDetected = true,
            };
        }

        private Vector3 GetCachedOrCalculatePosition(NormalizedLandmark landmark, int index)
        {
            var newPosition = GetPosition(landmark);

            if (DetectedBody.IsDetected == false)
                return newPosition;

            if (DetectedBody.Joints == null || DetectedBody.Joints.Length <= index)
                return newPosition;

            var oldPosition = DetectedBody.Joints[index];

            if (Vector3.Distance(newPosition, oldPosition.Position) <= 0.02f)
                return oldPosition.Position;

            return newPosition;
        }

        private Vector3 GetPosition(NormalizedLandmark landmark)
        {
            var x = landmark.x;
            var y = 1f-landmark.y; // invert y coordinate
            var z = landmark.z;

            var offset = _mainCamera.ViewportToWorldPoint(new Vector3(x, y, _mainCamera.nearClipPlane + 0.1f));
            offset.z = z * 0.4f;

            return offset;
        }
    }
}