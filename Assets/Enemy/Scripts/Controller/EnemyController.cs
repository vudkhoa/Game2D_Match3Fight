namespace Controller.Enemy 
{
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
        public RectTransform EnemyParent;
        public RectTransform EnemyMovePoint;

        public List<EnemyModel> LstEnemy;
        private int _enemySize = 10;
        private void Start()
        {
            this.SpawnEnemy(this._enemySize);
            this.MoveEnemy(this.EnemyMovePoint.anchoredPosition);
        }

        private void SpawnEnemy(int enemyCount)
        {
            for (int i = 0; i < this._enemySize; ++i)
            {
                EnemyView enemyGO = Instantiate(this.EnemyPrefab, this.EnemyParent);
                EnemyModel enemyModel = new EnemyModel();
                enemyModel.SetView(enemyGO.GetComponent<EnemyView>());
                if (this.LstEnemy == null)
                {
                    this.LstEnemy = new List<EnemyModel>();
                }
                this.LstEnemy.Add(enemyModel);
            }
        }

        public void MoveEnemy(Vector2 playerPos)
        {
            StartCoroutine(this.MoveEnemys(playerPos));
        }

        private IEnumerator MoveEnemys(Vector2 playerPos)
        {
            foreach (EnemyModel enemy in this.LstEnemy)
            {
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
    }
}