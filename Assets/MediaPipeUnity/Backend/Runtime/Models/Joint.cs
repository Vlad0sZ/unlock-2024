using UnityEngine;

namespace MediaPipeUnity.Backend
{
    public struct Joint
    {
        /// <summary>
        /// Тип сустава
        /// </summary>
        public readonly PoseJoint JointType;

        /// <summary>
        /// Позиция сустава (относительные координаты)
        /// </summary>
        public readonly Vector3 Position;

        /// <summary>
        /// Видимость сустава (например, не перекрывается ли другими частями тела)
        /// </summary>
        public readonly bool IsVisible;

        /// <summary>
        /// Наличие сустава (присутствует ли на входящем изображении)
        /// </summary>
        public readonly bool IsPresence;

        public Joint(PoseJoint jointType, Vector3 position, bool isVisible, bool isPresence)
        {
            JointType = jointType;
            Position = position;
            IsVisible = isVisible;
            IsPresence = isPresence;
        }
    }
}