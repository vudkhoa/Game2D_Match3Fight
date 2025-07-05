namespace View.Skill.Bullet
{
    using Controller.Skill;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    public class FireView : MonoBehaviour
    {
        public Image FireImg;
        public RectTransform RectTransform;

        public void SetPosition(Vector2 position)
        {
            this.RectTransform.anchoredPosition = position;
        }
        
        public void FireFalling(int index)
        {
            Vector3 newPos = SkillController.Instance.LstFireMove[index].transform.position;

            // Chọn một trong những Ease được đề xuất dưới đây
            Ease fallingEase = Ease.InQuad; // Tăng tốc nhẹ, giống rơi tự nhiên

            // Tạo hiệu ứng rơi với thời gian ngẫu nhiên
            float duration = Random.Range(0.4f, 0.8f);

            // Sequence để kết hợp nhiều hiệu ứng
            Sequence fallSequence = DOTween.Sequence();

            // Thêm hiệu ứng rơi xuống
            fallSequence.Append(
                this.RectTransform.DOMove(newPos, duration)
                .SetEase(fallingEase)
            );

            // Thêm hiệu ứng xoay nhẹ khi rơi (tùy chọn)
            float rotationAmount = Random.Range(-30f, 30f);
            fallSequence.Join(
                this.RectTransform.DORotate(new Vector3(0, 0, rotationAmount), duration)
                .SetEase(Ease.OutQuad)
            );

            // Thêm hiệu ứng co giãn nhẹ (tùy chọn)
            Vector3 originalScale = this.RectTransform.localScale;
            fallSequence.Join(
                this.RectTransform.DOScale(originalScale * 0.9f, duration * 0.7f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    this.RectTransform.DOScale(originalScale * 1.1f, duration * 0.3f)
                    .SetEase(Ease.OutBack);
                })
            );

            // Thêm hiệu ứng mờ dần cuối cùng (tùy chọn)
            fallSequence.OnComplete(() => {
                if (FireImg != null)
                {
                    FireImg.DOFade(0.7f, 0.2f).SetEase(Ease.OutQuad);
                }
            });

            // Bắt đầu sequence
            fallSequence.Play().OnComplete(() => { 
                gameObject.SetActive(false);
            });
        }
    }
}