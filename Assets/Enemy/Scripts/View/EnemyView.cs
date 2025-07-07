namespace View.Enemy
{
    using Controller.Enemy;
    using Controller.Skill;
    using Cotroller;
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
            ChangeY(Random.Range(0f, 100f));
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag.ToString() == "PlayerControlled")
            {
                EnemyView enemy = collision.gameObject.GetComponent<EnemyView>();
                if (enemy != null)
                {
                    if (enemy.GetComponent<RectTransform>().transform.position.x <= SkillController.Instance.StartSkillFlag.transform.position.x)
                    {
                        this.gameObject.tag = "Enemy";
                        enemy.gameObject.tag = "Enemy";
                        EnemyController.Instance.KillEnemy(this.Index);
                        EnemyController.Instance.KillEnemy(enemy.Index);
                        SkillController.Instance.KillFlag(enemy.Index);
                        if (enemy.Index == EnemyController.Instance.EnemySize - 1 ||
                            this.Index == EnemyController.Instance.EnemySize - 1
                            )
                        {
                            CoreGamePlayController.Instance.State = 1;
                            Debug.Log("Win Game");
                        }
                    }
                }
                else 
                {
                    EnemyController.Instance.KillEnemy(this.Index);
                    if (this.Index == EnemyController.Instance.EnemySize - 1)
                    {
                        CoreGamePlayController.Instance.State = 1;
                        Debug.Log("Win Game");
                    }
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
            this._rectTransform.DOLocalMove(playerPos, 5f).SetEase(Ease.Linear);
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