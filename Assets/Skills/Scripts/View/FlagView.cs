namespace View.Skill.Flag
{
    using DG.Tweening;
    using UnityEngine;

    public class FlagView : MonoBehaviour
    {
        public RectTransform RectTransform;
        public RectTransform PLatform;
        public RectTransform Flag;

        public void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }

        public void SetPosition(Vector3 position)
        {
            position.y -= (this.PLatform.rect.height * 2);
            this.RectTransform.DOMove(position, 0f).SetEase(Ease.Linear);
        }

        public void ShowFlag()
        {
            this.Flag.gameObject.SetActive(false);
            Vector2 flagPos = Vector2.zero;
            Vector3 newPosFlag = this.Flag.transform.position;
            newPosFlag += new Vector3(0f, 800f, 0f);
            flagPos.x += ((this.PLatform.rect.width - (this.PLatform.rect.height * 6/5)));
            this.Flag.DOMove(newPosFlag, 0f);

            Vector3 scale = this.PLatform.localScale;
            this.PLatform.localScale = Vector3.zero;
            this.PLatform.DOScale(scale, 0.25f).SetEase(Ease.OutBack).OnComplete(() => 
            {
                this.Flag.gameObject.SetActive(true);
                this.Flag.DOLocalMove(flagPos, 0.25f).SetEase(Ease.InElastic);

            });
        }
    }
}