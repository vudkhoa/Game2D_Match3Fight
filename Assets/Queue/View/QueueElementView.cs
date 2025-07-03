namespace View.Queue {
    using CustomData;
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class QueueElementView : MonoBehaviour
    {
        [Header("Element")]
        public TextMeshProUGUI Count;
        public Image ImageBackground;
        public Image ImageCooldown;
        public Image ImageSkill;
        public bool OnCooldown = false;
        private float _cooldownTime = 0f;

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

        public void SetCooldownTime(float cooldownTime)
        {
            this._cooldownTime = cooldownTime;
        }
    }
}