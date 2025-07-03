namespace View.Enemy
{
    using DG.Tweening;
    using UnityEngine;

    public class EnemyView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector2 _startPos;
        public RectTransform Bomb;

        private void Awake()
        {
            this._rectTransform = this.GetComponent<RectTransform>();
            this._startPos = this._rectTransform.anchoredPosition;
            ChangeY(Random.Range(0f, 190f));
        }

        private void ChangeY(float yOffset)
        {
            this._startPos.y = yOffset;
            this._rectTransform.anchoredPosition = this._startPos;
        }

        public void Move(Vector2 playerPos)
        {
            this._rectTransform.DOLocalMove(playerPos, 20f).SetEase(Ease.Linear);
        }

        public void PauseMove()
        {
            DOTween.Kill(this._rectTransform);
        }

        public void ThrowBomb(Vector2 pos)
        {
            this.Bomb.DOMove(pos, 1f).SetEase(Ease.OutQuad);
        }

        public Vector2 GetPosRectTransform()
        {
            return this._rectTransform.anchoredPosition;
        }

        public RectTransform GetRectTransform()
        {
            return this._rectTransform;
        }

        public void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }   

    }
}