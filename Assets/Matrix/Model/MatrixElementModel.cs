namespace Model.Matrix
{
    using View.Matrix;
    using CustomData;
    using UnityEngine;

    public class MatrixElementModel
    {
        public MatrixElementView MatrixElementView;
        public MatrixElementType MatrixElementType;
        public bool IsMatch = false;

        public void Init (MatrixElementView matrixElementView, MatrixElementType matrixElementType, int i, int j)
        {
            this.MatrixElementView = matrixElementView;

            // Set View
            this.SetType(matrixElementType);
            Vector2Int pos = new Vector2Int(i, j);
            this.SetPosView(pos);

            this.SetMatch(false);
        }

        public void SetType (MatrixElementType matrixElementType)
        {
            this.MatrixElementType = matrixElementType;
            this.MatrixElementView.SetType(matrixElementType);
        }

        public void SetPosView(Vector2Int pos)
        {
            this.MatrixElementView.SetPos(pos);
        }

        public void SetMatch (bool isMatch)
        {
            this.IsMatch = isMatch;
            this.SetActiveView();
        }

        public void SetActiveView()
        {
            this.MatrixElementView.gameObject.SetActive(!this.IsMatch);
        }
    }
}
