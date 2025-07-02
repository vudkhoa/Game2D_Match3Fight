namespace View.Enemy
{
    using DG.Tweening;
    using UnityEngine;

    public class EnemyView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector2 _startPos;

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

        public Vector2 GetRectTransform()
        {
            return this._rectTransform.anchoredPosition;
        }

        public void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }   

    }
}