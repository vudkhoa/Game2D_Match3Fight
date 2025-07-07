namespace View.Matrix
{
    using Controller.Matrix;
    using CustomData;
    using DG.Tweening;
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

        public void OnPointerUp(PointerEventData eventData) { }
        
        public void PlayMoveAnimation(Vector2 pos)
        {
            Vector2 tmpPos = gameObject.GetComponent<RectTransform>().anchoredPosition;

            gameObject.GetComponent<RectTransform>().DOLocalMove(pos, 0.4f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                gameObject.SetActive(false);
                gameObject.GetComponent<RectTransform>().anchoredPosition = tmpPos;
            });
        }

        public void PlayMatchAnimation()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(gameObject.transform.DOScale(1.2f, 0.1f));
            seq.Append(gameObject.transform.DOScale(0.8f, 0.1f));
            seq.Append(gameObject.transform.DOScale(0f, 0.2f));
            seq.OnComplete(() =>
            {
                gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(false);
            });
            seq.Play();
        }

        public void PlaySwapAnimation(Vector2 pos)
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            Vector2 oldPos = rect.anchoredPosition;

            rect.DOLocalMove(pos, 0.4f).SetEase(Ease.InOutQuad).OnComplete(() => 
            {
                this.gameObject.SetActive(false);
                rect.anchoredPosition = oldPos;
                this.gameObject.SetActive(true);
            });
        }
    }
}