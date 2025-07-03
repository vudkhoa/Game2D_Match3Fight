namespace View.Skill.Bullet
{
    using Controller.Enemy;
    using Controller.Skill;
    using DG.Tweening;
    using System.Collections;
    using UnityEngine;

    public class BulletView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private int _index;
        private Vector2 pos2;

        private void Awake()
        {
            this._rectTransform = this.GetComponent<RectTransform>();
        }

        public void SetIndex(int index)
        {
            this._index = index;
        }

        public void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }

        public IEnumerator GetEnemyAncho(int index)
        {
            yield return new WaitForSeconds(0.95f);
            pos2 = EnemyController.Instance.LstEnemy[index].EnemyView.GetComponentInChildren<RectTransform>().anchoredPosition;
            pos2.x = pos2.x + 170f + 884f - 120f;
            pos2.y = pos2.y + 168f - 120f - 38f;
        }

        public void KillEnemy(int index)
        {
            RePosBullet();
            EnemyController.Instance.LstEnemy[index].IsDead = true;
            EnemyController.Instance.LstEnemy[index].EnemyView.SetActive(false);
        }

        private void RePosBullet()
        {
            this._rectTransform.anchoredPosition = Vector2.zero;
            this._rectTransform.localRotation = Quaternion.identity;
            this.SetActive(false);
            SkillController.Instance.ResetBullet(this._index);
        }

        public void Shooting(Vector2 pos1, int index)
        {
            StartCoroutine(GetEnemyAncho(index));
            Vector2 direction;
            direction = pos1;
            float angle1 = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            this._rectTransform.DOLocalRotate(new Vector3(0, 0, -angle1), 0.2f).SetEase(Ease.Linear);
            this._rectTransform.DOLocalMove(pos1, 1f).SetEase(Ease.Linear).OnComplete(() => 
            {
                direction = pos2 - pos1;
                float angle2 = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                angle2 = -angle2;

                this._rectTransform.DOLocalRotate(new Vector3(0, 0, angle2), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    this._rectTransform.DOLocalMove(pos2, 0.4f).SetEase(Ease.Linear).OnComplete(() => 
                    {
                        KillEnemy(index);
                    });
                });
            });

        }
    }
}