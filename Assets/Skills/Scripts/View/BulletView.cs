namespace View.Skill.Bullet
{
    using Controller.Enemy;
    using Controller.Queue;
    using Controller.Skill;
    using Cotroller;
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
            yield return new WaitForSeconds(0.55f);
            pos2 = EnemyController.Instance.LstEnemy[index].EnemyView.GetComponent<RectTransform>().transform.position;
        }

        public void KillEnemy(int index)
        {
            RePosBullet();
            EnemyController.Instance.KillEnemy(index, 1);
            QueueController.Instance.QueueElementModelList[0].QueueElementView.EndSkill();
            if (index == EnemyController.Instance.EnemySize - 1)
            {
                CoreGamePlayController.Instance.State = 1;
                Debug.Log("Win Game");
            }
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
            this._rectTransform.DOLocalRotate(new Vector3(0, 0, -45), 0.2f).SetEase(Ease.Linear);
            this._rectTransform.DOMove(pos1, 0.6f).SetEase(Ease.Linear).OnComplete(() => 
            {
                direction = pos2 - pos1;
                float angle2 = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                angle2 = -angle2;

                this._rectTransform.DOLocalRotate(new Vector3(0, 0, -95), 0.2f).SetEase(Ease.Linear);
                this._rectTransform.DOMove(pos2, 0.3f).SetEase(Ease.Linear).OnComplete(() => 
                {
                    KillEnemy(index);
                });
            });
        }
    }
}