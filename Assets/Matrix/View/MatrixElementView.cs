namespace View.Matrix
{
    using Controller.Matrix;
    using CustomData;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class MatrixElementView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Image ImageBackground;
        public Image ImageIcon;

        private Vector2Int _pos;

        public void SetType(MatrixElementType type)
        {
            this.ImageBackground.sprite = DataManager.Instance.SkillData.Skills[(int)type - 1].SkillBackground.MatrixBackground.Sprite;
            this.ImageIcon.sprite = DataManager.Instance.SkillData.Skills[(int)type - 1].SkillIcon.MatrixIcon.Sprite;
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