namespace View.Enemy
{
    using Controller.Enemy;
    using Controller.Skill;
    using DG.Tweening;
    using UnityEngine;

    public class EnemyView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector2 _startPos;
        public RectTransform Bomb;
        public int Index;

        private void Awake()
        {
            this._rectTransform = this.GetComponent<RectTransform>();
            this._startPos = this._rectTransform.anchoredPosition;
            ChangeY(Random.Range(0f, 190f));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //Debug.Log("Collision with: " + collision.gameObject.name);
            if (collision.gameObject.tag.ToString() == "PlayerControlled")
            {
                EnemyView enemy = collision.gameObject.GetComponent<EnemyView>();
                if (enemy != null)
                {
                    if (enemy.GetComponent<RectTransform>().transform.position.x <= SkillController.Instance.StartSkillFlag.transform.position.x)
                    {
                        EnemyController.Instance.KillEnemy(this.Index);
                        EnemyController.Instance.KillEnemy(enemy.Index);
                        SkillController.Instance.KillFlag(enemy.Index);
                    }
                }
                else 
                {
                    EnemyController.Instance.KillEnemy(this.Index);
                }
            }
        }

        private void ChangeY(float yOffset)
        {
            this._startPos.y = yOffset;
            this._rectTransform.anchoredPosition = this._startPos;
        }

        public void SetIndex(int index)
        {
            this.Index = index;
        }

        public void Move(Vector2 playerPos)
        {
            this._rectTransform.DOLocalMove(playerPos, 20f).SetEase(Ease.Linear);
        }

        public void PauseMove()
        {
            DOTween.Kill(this._rectTransform);
        }

        public void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }
    }
}