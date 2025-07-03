namespace Controller.Skill
{
    using Controller.Enemy;
    using CustomUtils;
    using Model.Skill.Bullet;
    using Model.Skill.Flag;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using View.Skill.Bullet;
    using View.Skill.Flag;

    public class SkillController : SingletonMono<SkillController>
    {
        [Header(" Bullet Information ")]
        public BulletView BulletPrefab;
        public RectTransform BulletParentPointPrefab;
        private RectTransform _bulletParentPoint;

        [Header(" Shoot ")]
        public RectTransform PointPrefab;
        private RectTransform _point;

        private List<BulletModel> _lstBullet;
        private int _bulletSize = 10;

        [Header(" Flag Information ")]
        public FlagView FlagPrefab;
        public RectTransform FlagParentEnemy;

        private List<FlagModel> _lstFlagEnemy;
        private int _flagSize = 20;

        public int State = 0;
        private int _curEnemyIndex = -1;

        private void Start()
        {
            this.SpawnPoint();
            this.SpawnBullet();
        }

        
        private void SpawnPoint()
        {
            this._bulletParentPoint = Instantiate(this.BulletParentPointPrefab, this.transform);
            this._point = Instantiate(this.PointPrefab, this.transform);
        }

        private void SpawnBullet()
        {
            this._lstBullet = new List<BulletModel>();

            for (int i = 0; i < this._bulletSize; ++i)
            {
                BulletView bulletGO = Instantiate(this.BulletPrefab, this._bulletParentPoint);

                BulletModel bulletModel = new BulletModel();
                bulletModel.SetView(bulletGO.GetComponent<BulletView>());
                bulletModel.BulletView.SetIndex(i);
                bulletModel.BulletView.SetActive(false);
                this._lstBullet.Add(bulletModel);
            }
        }

        public void Shooting(int index)
        {
            StartCoroutine(this.ShootBullets(index));
        }

        private IEnumerator ShootBullets(int index)
        {
            for (int i = 0; i < this._bulletSize; ++i)
            {   
                if (!this._lstBullet[i].IsUsed)
                {
                    this._lstBullet[i].IsUsed = true;
                    yield return new WaitForSeconds(0.4f);
                    this._lstBullet[i].BulletView.SetActive(true);
                    this._lstBullet[i].BulletView.Shooting(
                        this._point.anchoredPosition,
                        index
                    );
                    break;
                }
            }
        }

        public void ResetBullet(int index)
        {
            this._lstBullet[index].IsUsed = true;
        }


        // ---------------------------------------------Flag--------------------------------------------//
        public void SpawnFlagEnemy()
        {
            // For Enemy
            this._lstFlagEnemy = new List<FlagModel>();
            for (int i = 0; i < this._flagSize; ++i)
            {
                FlagView flagGO = Instantiate(this.FlagPrefab, this.FlagParentEnemy);
                FlagModel flagModel = new FlagModel();

                flagModel.SetView(flagGO);
                flagModel.FlagView.SetActive(false);
                this._lstFlagEnemy.Add(flagModel);
            }
        }

        public void ShowFlag()
        {
            Debug.Log("Show Flag");
            this._lstFlagEnemy[0].FlagView.SetActive(true);
            int index = EnemyController.Instance.GetIndex();
            Debug.Log("Index: " + index);
            EnemyController.Instance.LstEnemy[index].EnemyView.PauseMove();
            EnemyController.Instance.LstEnemy[index].IsDead = true;
            Vector2 showPoint = EnemyController.Instance.LstEnemy[index].EnemyView.GetComponentInChildren<RectTransform>().anchoredPosition;
            showPoint.y -= this.FlagParentEnemy.rect.height / 2 + 50f;
            showPoint.x += 170f;
            this._lstFlagEnemy[0].IsUsed = true;
            this._lstFlagEnemy[index].FlagView.ShowFlag();
            this._lstFlagEnemy[index].FlagView.SetPosition(showPoint);
            this._curEnemyIndex = index;

        }

        public IEnumerator ThrowBomb()
        {
            Debug.Log("Throw Bomb");
            int i = EnemyController.Instance.GetIndex();
            Vector3 newPos = EnemyController.Instance.LstEnemy[i].EnemyView.GetRectTransform().transform.position;
            EnemyController.Instance.LstEnemy[i].IsDead = true;
            EnemyController.Instance.LstEnemy[_curEnemyIndex].EnemyView.ThrowBomb(newPos);
            yield return new WaitForSeconds(1f);
            EnemyController.Instance.LstEnemy[i].EnemyView.SetActive(false);
            EnemyController.Instance.LstEnemy[_curEnemyIndex].EnemyView.SetActive(false);
            this._lstFlagEnemy[0].FlagView.SetPosition(Vector2.zero);
            //this._lstFlagEnemy[0].FlagView.SetActive(false);
        }

    }
}