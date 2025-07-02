namespace CustomData
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class QElementSkin
    {
        public Sprite Sprite;
    }

    [CreateAssetMenu(menuName = "QueueElemntSkinSO", fileName = "QueueElemntSkinSO")]
    public class QueueElemntSkinSO : ScriptableObject
    {
        public List<QElementSkin> QueueElementSkins;
    }
}