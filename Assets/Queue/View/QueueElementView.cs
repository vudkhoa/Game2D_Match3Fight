namespace View.Queue
{
    using CustomData;
    using DG.Tweening;
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class QueueElementView : MonoBehaviour
    {
        [Header("Element")]
        public Image GlowImage;
        public TextMeshProUGUI Count;
        public Image ImageBackground;
        public Image ImageCooldown;
        public Image ImageSkill;
        public bool OnCooldown = false;
        public bool IsGlow = false;
        private float _cooldownTime = 0f;
        private float _castTime = 0f;
        private Vector3 upPos = new Vector3(0f, 50f, 0f);
        private float upScale = 1.05f;
        private Coroutine glowCoroutine;

        private void Start()
        {
            ImageCooldown.fillAmount = 0f;
            this.ImageCooldown.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (this.OnCooldown)
            {
                this.OnCoooldown();
            }

            if (this.IsGlow)
            {
                this.OnGlowAnimation();
            }
        }

        private void OnCoooldown()
        {
            this.ImageCooldown.fillAmount -= 1 / this._cooldownTime * Time.deltaTime;
            if (this.ImageCooldown.fillAmount <= 0f)
            {
                this.ImageCooldown.fillAmount = 0f;
            }
        }

        public void StartCooldown()
        {
            if (this.OnCooldown)
            {
                return;
            }
            StartCoroutine(this.Cooldown());
        }

        private IEnumerator Cooldown()
        {
            this.OnCooldown = true;
            this.ImageCooldown.gameObject.SetActive(true);
            this.ImageCooldown.fillAmount = 1f;
            yield return new WaitForSeconds(this._cooldownTime);
            this.OnCooldown = false;
            this.ImageCooldown.gameObject.SetActive(false);
        }

        public void SetType(MatrixElementType type)
        {
            this.ImageBackground.sprite = DataManager.Instance.SkillData.Skills[(int)type - 1].SkillBackground.QueueBackground.Sprite;
            this.ImageSkill.sprite = DataManager.Instance.SkillData.Skills[(int)type - 1].SkillIcon.QueueIcon.Sprite;
        }

        public void SetCount(int count)
        {
            this.Count.text = count.ToString();
        }

        public void SetTimes(float cooldownTime, float castTime)
        {
            this._cooldownTime = cooldownTime;
            this._castTime = castTime;
        }

        public void StartSkill()
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            rectTransform.DOMove(rectTransform.transform.position + upPos, 0.2f).SetEase(Ease.OutQuad);
            rectTransform.DOScale(rectTransform.localScale * upScale, 0.2f).SetEase(Ease.OutSine);
        }

        public void EndSkill()
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.DOMove(rectTransform.transform.position - upPos, 0.2f).SetEase(Ease.InQuad);
            rectTransform.DOScale(rectTransform.localScale / upScale, 0.2f).SetEase(Ease.InSine).OnComplete(() => 
            {
                this.StartCooldown();
                this.IsGlow = false;
            });
        }


        private void OnGlowAnimation()
        {
            this.GlowImage.fillAmount -= 1 / this._castTime * Time.deltaTime;
            if (this.GlowImage.fillAmount <= 0f)
            {
                this.GlowImage.fillAmount = 0f;
            }
        }

        public void StartGlow()
        {
            if (this.IsGlow)
            {
                if (glowCoroutine != null)
                {
                    StopCoroutine(glowCoroutine);
                }
                this.GlowImage.fillAmount = 1f;
            }
            glowCoroutine = StartCoroutine(this.DetailGlow());
        }

        private IEnumerator DetailGlow()
        {
            this.IsGlow = true;
            this.GlowImage.gameObject.SetActive(true);
            this.GlowImage.fillAmount = 1f;
            yield return new WaitForSeconds(this._castTime);
            this.ImageCooldown.gameObject.SetActive(false);
        }
    }
}