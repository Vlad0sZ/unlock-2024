namespace MediaPipeUnity.Backend
{
    /// <summary>
    /// Структура тела
    /// https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker
    /// </summary>
    public struct Body
    {
        /// <summary>
        /// Структура с joints
        /// </summary>
        public Joint[] Joints { get; set; }

        public bool IsDetected { get; set; }

        public Joint this[PoseJoint jointType]
        {
            get
            {
                var index = (int) jointType;
                return this[index];
            }
        }

        public Joint this[int jointIndex]
        {
            get
            {
                if (Joints == null || Joints.Length <= jointIndex)
                    return default;

                return Joints[jointIndex];
            }

            set
            {
                if (Joints == null || Joints.Length <= jointIndex)
                    return;

                Joints[jointIndex] = value;
            }
        }
    }
}