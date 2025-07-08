namespace View.Skill.Fire
{
    using Controller.Skill;
    using DG.Tweening;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class FireView : MonoBehaviour
    {
        public Image FireImg;
        public RectTransform RectTransform;
        public List<Image> FireExplosionImages;

        public void SetPosition(Vector2 position)
        {
            this.RectTransform.anchoredPosition = position;
        }
        
        public void FireFalling(int index)
        {
            Vector3 newPos = SkillController.Instance.LstFireMove[index].transform.position;
            Ease fallingEase = Ease.InQuad;
            float duration = Random.Range(0.4f, 0.8f);
            Sequence fallSequence = DOTween.Sequence();
            fallSequence.Append(
                this.RectTransform.DOMove(newPos, duration)
                .SetEase(fallingEase).OnComplete(() => 
                {
                    StartCoroutine(this.PlayFireExplosion());
                })
            );
            float rotationAmount = Random.Range(-30f, 30f);
            fallSequence.Join(
                this.RectTransform.DORotate(new Vector3(0, 0, rotationAmount), duration)
                .SetEase(Ease.OutQuad)
            );
            Vector3 originalScale = this.RectTransform.localScale;
            fallSequence.Join(
                this.RectTransform.DOScale(originalScale * 0.9f, duration * 0.7f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    this.RectTransform.DOScale(originalScale * 1.1f, duration * 0.3f)
                    .SetEase(Ease.OutBack);
                })
            );
            fallSequence.OnComplete(() => {
                if (FireImg != null)
                {
                    FireImg.DOFade(0.7f, 0.2f).SetEase(Ease.OutQuad);
                }
            });

            // Bắt đầu sequence
            fallSequence.Play();
        }

        public void PauseFall()
        {
            DOTween.Kill(this.gameObject);
            StartCoroutine(PlayFireExplosion());
        }

        public IEnumerator PlayFireExplosion()
        {
            foreach (Image fireExplosion in FireExplosionImages)
            {
                fireExplosion.gameObject.SetActive(true);
                StartCoroutine(this.EndFireExplosion(fireExplosion));
            }
            yield return new WaitForSeconds(0.3f);
            this.gameObject.SetActive(false);
        }

        public IEnumerator EndFireExplosion(Image fireExplosion)
        {
            yield return new WaitForSeconds(0.25f);
            fireExplosion.gameObject.SetActive(false);
        }
    }
}