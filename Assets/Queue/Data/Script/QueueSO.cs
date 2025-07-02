namespace CustomData
{
    using UnityEngine;
    using System;

    [Serializable]
    public class InforQueue
    {
        public int Size;
        public int Direction;
        public float Width;
        public float Height;
        public float Spacing;
        public float PaddingHon;
        // 0 --> y.
        public float PaddingVer;
    }

    [Serializable]
    public class InforQueueElement
    {
        public float Width;
        public float Height;
    }

    [CreateAssetMenu(menuName = "QueueData", fileName = "QueueData")]
    public class QueueSO : ScriptableObject
    {
        public InforQueue InformationQueue;
        public InforQueueElement InformationElement;
    }
}