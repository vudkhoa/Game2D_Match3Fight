namespace Controller.Enemy 
{
    using Controller.Skill;
    using CustomUtils;
    using Model.Enemy;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using View.Enemy;

    public class EnemyController : SingletonMono<EnemyController>
    {
        [Header(" Information ")]
        public EnemyView EnemyPrefab;
        public RectTransform EnemyParentPrefab;
        public RectTransform EnemyMovePointPrefab;
        public int EnemySize { get; private set; }
        public int CountEnemyDead = 0;

        private RectTransform _enemyParent;
        private RectTransform _enemyMovePoint;

        public List<EnemyModel> LstEnemy;
        private void Start()
        {
            SkillController.Instance.SpawnFlag();
            this.SpawnPoints();
            this.SpawnEnemy(this.EnemySize);
            this.MoveEnemy(this._enemyMovePoint.anchoredPosition);
        }

        private void SpawnEnemy(int enemyCount)
        {
            this.EnemySize = 100;
            for (int i = 0; i < this.EnemySize; ++i)
            {
                EnemyView enemyGO = Instantiate(this.EnemyPrefab, this._enemyParent);
                EnemyModel enemyModel = new EnemyModel();
                enemyModel.SetView(enemyGO.GetComponent<EnemyView>());
                enemyModel.EnemyView.SetActive(false);
                enemyModel.EnemyView.SetIndex(i);
                if (this.LstEnemy == null)
                {
                    this.LstEnemy = new List<EnemyModel>();
                }
                this.LstEnemy.Add(enemyModel);
            }
        }

        private void SpawnPoints()
        {
            this._enemyParent = Instantiate(this.EnemyParentPrefab, this.transform);
            this._enemyMovePoint = Instantiate(this.EnemyMovePointPrefab, this.transform);
        }

        public void MoveEnemy(Vector2 playerPos)
        {
            StartCoroutine(this.MoveEnemys(playerPos));
        }

        private IEnumerator MoveEnemys(Vector2 playerPos)
        {
            foreach (EnemyModel enemy in this.LstEnemy)
            {
                enemy.EnemyView.SetActive(true);
                enemy.EnemyView.Move(playerPos);
                yield return new WaitForSeconds(Random.Range(0f, 5f));
            }
        }

        public int GetIndex()
        {
            int index = -1;
            foreach (EnemyModel enemy in this.LstEnemy)
            {
                index++;
                if (!enemy.IsDead)
                {
                    enemy.IsDead = true;
                    return index;
                }
            }
            return index;
        }

        public bool ExistEnemy(int i)
        {
            foreach (EnemyModel enemy in this.LstEnemy)
            {
                if (!enemy.IsDead && enemy.EnemyView.gameObject.activeSelf)
                {
                    if (i == 1 && enemy.EnemyView.GetComponent<RectTransform>().transform.position.x <= SkillController.Instance.StartSkillBullet.transform.position.x)
                    {
                        return true;
                    }
                    else if (i == 2 && enemy.EnemyView.GetComponent<RectTransform>().transform.position.x <= SkillController.Instance.StartSkillFlag.transform.position.x)
                    {
                        return true;
                    }
                    else if (i == 3 && enemy.EnemyView.GetComponent<RectTransform>().transform.position.x <= SkillController.Instance.StartSkillFireStorm.transform.position.x)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void KillEnemy(int index)
        {
            this.CountEnemyDead++;
            this.LstEnemy[index].IsDead = true;
            this.LstEnemy[index].EnemyView.SetActive(false);
            this.LstEnemy[index].EnemyView.PauseMove();
        }
    }
}