namespace View.Enemy
{
    using Controller.Enemy;
    using Controller.Skill;
    using Cotroller;
    using DG.Tweening;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Skill.Fire;

    public class EnemyView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector2 _startPos;
        public RectTransform Bomb;
        public GameObject BombWalkSideOver;
        public Image Enemy;
        public Image BombExplosion;
        public Image Vanish;
        public int Index;

        private void Awake()
        {
            this.Init();
        }

        private void Init()
        {
            this.BombExplosion.gameObject.SetActive(false);
            this._rectTransform = this.GetComponent<RectTransform>();
            this._startPos = this._rectTransform.anchoredPosition;
            ChangeY(Random.Range(0f, 100f));
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag.ToString() == "PlayerControlled")
            {
                EnemyView enemy = collision.gameObject.GetComponent<EnemyView>();
                if (EnemyController.Instance.LstEnemy[this.Index].IsDead) return;
                if (enemy != null)
                {
                    if (enemy.GetComponent<RectTransform>().transform.position.x <= SkillController.Instance.StartSkillFlag.transform.position.x)
                    {
                        this.gameObject.tag = "Enemy";
                        enemy.gameObject.tag = "Enemy";
                        EnemyController.Instance.KillEnemy(this.Index, 2);
                        EnemyController.Instance.KillEnemy(enemy.Index, 2);
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
                    FireView fireView = collision.gameObject.GetComponent<FireView>();
                    fireView.PauseFall();
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

        public IEnumerator PlayExplosionAnim()
        {
            this.Bomb.gameObject.SetActive(false);
            this.BombExplosion.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            this.BombExplosion.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

        public IEnumerator PlayVanishAnim()
        {
            this.Enemy.enabled = false;
            this.Bomb.gameObject.SetActive(false);
            this.BombWalkSideOver.SetActive(false);
            this.PauseMove();
            this.Vanish.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            this.Vanish.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            this.Enemy.enabled = true;
            this.Bomb.gameObject.SetActive(true);
            this.BombWalkSideOver.SetActive(true);
        }
    }
}