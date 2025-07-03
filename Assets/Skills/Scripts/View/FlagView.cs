namespace View.Skill.Flag
{
    using Controller.Skill;
    using DG.Tweening;
    using System.Collections;
    using UnityEngine;

    public class FlagView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        public RectTransform PLatform;
        public RectTransform Flag;

        private void Awake()
        {
            this._rectTransform = this.GetComponent<RectTransform>();
        }

        public void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }

        public void SetPosition(Vector2 position)
        {
            this._rectTransform.anchoredPosition = position;
        }

        public void ShowFlag()
        {
            this.Flag.gameObject.SetActive(false);
            Vector2 flagPos = this.Flag.anchoredPosition;
            Vector2 newPosFlag = this._rectTransform.anchoredPosition + new Vector2(850f, 400f);
            this.Flag.anchoredPosition = newPosFlag;
            Vector3 scale = this.PLatform.localScale;
            this.PLatform.localScale = Vector3.zero;
            this.PLatform.DOScale(scale, 0.25f).SetEase(Ease.OutBack).OnComplete(() => 
            {
                this.Flag.gameObject.SetActive(true);
                this.Flag.DOLocalMove(flagPos, 0.25f).SetEase(Ease.Linear);
            });
        }

        private IEnumerator ActiveThrowBombSkill()
        {
            yield return new WaitForSeconds(0.6f);
            StartCoroutine(SkillController.Instance.ThrowBomb());
        }
    }
}