namespace CustomData
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InforMatrix
    {
        public float Width, Height;
        public Vector2Int Size;
        public Vector2 Spacing;
        public Vector2 Padding;
        public Vector2Int Direction;
    }

    [Serializable]
    public class InforMatrixElement
    {
        // Square
        public float Size;
    }

    [CreateAssetMenu(menuName = "MatrixData", fileName = "MatrixData")]
    public class MatrixData : ScriptableObject
    {
        public InforMatrix InformationMatrix;
        public InforMatrixElement InformationElement;
    }
}
