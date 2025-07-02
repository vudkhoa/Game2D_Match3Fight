namespace CustomData
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    public enum MatrixElementType
    {
        Null = 0,
        Type1 = 1,
        Type2 = 2,
        Type3 = 3
    }

    [Serializable]
    public class MatrixElementInfor
    {
        public Vector2Int Position;
        public MatrixElementType Type;
    }

    [Serializable]
    public class MatrixList
    {
        public Vector2Int Size;
        public List<MatrixElementInfor> ElementList;
    }

    [Serializable]
    public class MatrixLevel
    {
        public int LevelId;
        public List<MatrixList> MatrixList;
    }

    [CreateAssetMenu(menuName = "MatrixLevelData", fileName = "MatrixLevelData")]
    public class MatrixLevelData : ScriptableObject
    {
        public List<MatrixLevel> MatrixLevelList;
    }
}
