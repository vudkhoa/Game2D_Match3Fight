namespace View.Matrix
{
    using Controller.Matrix;
    using CustomData;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class MatrixElementView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public TextMeshProUGUI Text;

        private Vector2Int _pos;

        public void SetType(MatrixElementType type)
        {
            this.Text.text = ((int)type).ToString();
        }

        public void SetPos(Vector2Int pos)
        {
            this._pos = pos;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            MatrixController.Instance.CurrentPos = this._pos;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}