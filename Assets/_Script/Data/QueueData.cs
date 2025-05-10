namespace CustomData
{
    using UnityEngine;
    using System;

    [Serializable]
    public class InforQueue
    {
        public int Size;
        public float Width;
        public float Height;
        public float Spacing;
        public float Padding;
    }

    [Serializable]
    public class InforQueueElement
    {
        public float Width;
        public float RatioHeight;
    }

    [CreateAssetMenu(menuName = "QueueData", fileName = "QueueData")]
    public class QueueData : ScriptableObject
    {
        public InforQueue InformationQueue;
        public InforQueueElement InformationElement;
    }
}