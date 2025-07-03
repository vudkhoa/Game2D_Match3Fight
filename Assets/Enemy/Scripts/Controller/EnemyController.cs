namespace Controller.Enemy 
{
    using Controller.Skill;
    using CustomUtils;
    using Model.Enemy;
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;
    using View.Enemy;

    public class EnemyController : SingletonMono<EnemyController>
    {
        [Header(" Information ")]
        public EnemyView EnemyPrefab;
        public RectTransform EnemyParentPrefab;
        public RectTransform EnemyMovePointPrefab;

        private RectTransform _enemyParent;
        private RectTransform _enemyMovePoint;

        public List<EnemyModel> LstEnemy;
        private int _enemySize = 10;
        private void Start()
        {
            SkillController.Instance.SpawnFlagEnemy();
            this.SpawnPoints();
            this.SpawnEnemy(this._enemySize);
            this.MoveEnemy(this._enemyMovePoint.anchoredPosition);
        }

        private void SpawnEnemy(int enemyCount)
        {
            for (int i = 0; i < this._enemySize; ++i)
            {
                EnemyView enemyGO = Instantiate(this.EnemyPrefab, this._enemyParent);
                EnemyModel enemyModel = new EnemyModel();
                enemyModel.SetView(enemyGO.GetComponent<EnemyView>());
                enemyModel.EnemyView.SetActive(false);
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
                yield return new WaitForSeconds(Random.Range(0f, 20f));
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



        public bool ExistEnemy()
        {
            foreach (EnemyModel enemy in this.LstEnemy)
            {
                if (!enemy.IsDead && enemy.EnemyView.gameObject.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
    }
}