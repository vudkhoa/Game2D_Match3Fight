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
    using CustomUtils.Vukhoa;
    using Controller.Queue;

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

        private List<FlagModel> _lstFlag;
        private int _flagSize = 20;

        [Header(" Fire Information ")]
        public FireView FirePrefab;
        public RectTransform FSParentAllPrefab;
        public RectTransform FireParentPrefab;
        public RectTransform FireMoveParentPrefab;
        public RectTransform FireMovePrefab;

        public RectTransform FSParentAll;
        public List<RectTransform> LstFireMove;

        private List<RectTransform> _lstFireParent;
        private List<FirestormClass> _lstFireStorm;
        private int _sizeFire = 9;
        private int _sizeFireStorm = 20;

        [Header(" Start Game ")]
        public RectTransform ParentAll;
        public RectTransform StartSkillBulletPrefab;
        public RectTransform StartSkillFlagPrefab;
        public RectTransform StartSkillFireStormPrefab;
        public RectTransform StartSkillBullet;
        public RectTransform StartSkillFlag;
        public RectTransform StartSkillFireStorm;

        [Header(" State ")]
        public int State = 0;
        private int _curEnemyIndex = -1;

        public class FirestormClass
        {
            public List<FireModel> Firestorm;
            public bool IsUsed;
        }

        private void Start()
        {
            this.SpawnStartPoint();
            this.SpawnBulletPoint();
            this.SpawnBullet();
            this.SpawnFireParents();
            this.SpawnFireMoves();
            this.SpawnFireStorms();
        }

        private void SpawnStartPoint()
        {
            this.StartSkillBullet = Instantiate(this.StartSkillBulletPrefab, this.ParentAll);
            this.StartSkillFlag = Instantiate(this.StartSkillFlagPrefab, this.ParentAll);
            this.StartSkillFireStorm = Instantiate(this.StartSkillFireStormPrefab, this.ParentAll);
        }
        private void SpawnBulletPoint()
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
            this.ShootBullets(index);
        }

        private void ShootBullets(int index)
        {
            for (int i = 0; i < this._bulletSize; ++i)
            {   
                if (!this._lstBullet[i].IsUsed)
                {
                    this._lstBullet[i].IsUsed = true;
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
        public void SpawnFlag()
        {
            // For Enemy
            this._lstFlag = new List<FlagModel>();
            for (int i = 0; i < this._flagSize; ++i)
            {
                FlagView flagGO = Instantiate(this.FlagPrefab, this.FlagParentEnemy);
                FlagModel flagModel = new FlagModel();

                flagModel.SetView(flagGO);
                flagModel.FlagView.SetActive(false);
                flagModel.IsUsed = false;
                this._lstFlag.Add(flagModel);
            }
        }

        public int GetIndexFlag(int indexEnemy)
        {
            int index = -1;
            foreach (var flag in this._lstFlag)
            {
                index++;
                if (!flag.IsUsed)
                {
                    flag.IsUsed = true;
                    flag.SetIndexEnemy(indexEnemy);
                    return index;
                }
            }
            return index;
        }

        public void ShowFlag()
        {
            int indexEnemy = EnemyController.Instance.GetIndex();
            int indexFlag = this.GetIndexFlag(indexEnemy);
            this._curEnemyIndex = indexEnemy;
            this._lstFlag[indexFlag].FlagView.SetActive(true);
            
            EnemyController.Instance.LstEnemy[indexEnemy].EnemyView.PauseMove();

            Vector3 showPoint = EnemyController.Instance.LstEnemy[indexEnemy].EnemyView.GetComponent<RectTransform>().transform.position;
            this._lstFlag[indexFlag].FlagView.SetPosition(showPoint);
            this._lstFlag[indexFlag].FlagView.ShowFlag(this._curEnemyIndex);
        }

        public void KillFlag(int indexEnemy)
        {
            int index = -1;
            foreach (FlagModel flag in this._lstFlag)
            {
                index++;
                if (flag.IsUsed == true && flag.IndexEnemy == indexEnemy)
                {
                    this._lstFlag[index].SetIndexEnemy(-1);
                    this._lstFlag[index].IsUsed = false;
                    this._lstFlag[index].FlagView.SetActive(false);
                }
            }
        }

        // ---------------------------------------------FireStorm--------------------------------------------//
        public void SpawnFireParents()
        {
            this.FSParentAll = Instantiate(this.FSParentAllPrefab, this.transform);
            this._lstFireParent = new List<RectTransform>();
            for (int i = 0; i < this._sizeFireStorm; ++i)
            {
                RectTransform fireParent = Instantiate(this.FireParentPrefab, this.FSParentAll);
                this._lstFireParent.Add(fireParent);
            }
        }

        public void SpawnFireMoves()
        {
            this.LstFireMove = new List<RectTransform>();
            RectTransform fiveMoveParent = Instantiate(this.FireMoveParentPrefab, this.FSParentAll);
            for (int i = 0; i < this._sizeFire; ++i)
            {
                RectTransform fireMove = Instantiate(this.FireMovePrefab, fiveMoveParent);
                if (i > 0)
                {
                    Vector2 pos = fireMove.anchoredPosition;
                    pos.x = (this.LstFireMove[i - 1].anchoredPosition.x + this.LstFireMove[i - 1].rect.width);
                    fireMove.anchoredPosition = pos;
                }

                this.LstFireMove.Add(fireMove);
            }
        }

        public void SpawnFireStorms()
        {
            this._lstFireStorm = new List<FirestormClass>();
            for (int i = 0; i < this._sizeFireStorm; ++i)
            {
                FirestormClass firestormClass = new FirestormClass();
                List <FireModel> fireStorm = new List<FireModel>();
                for (int j = 0; j < this._sizeFire; ++j)
                {
                    FireView fireView = Instantiate(this.FirePrefab, this._lstFireParent[i]);
                    FireModel fireModel = new FireModel();
                    fireModel.SetView(fireView);
                    fireModel.FireView.SetPosition(this.LstFireMove[j].anchoredPosition);
                    fireModel.FireView.gameObject.SetActive(false);
                    fireStorm.Add(fireModel);
                }
                firestormClass.Firestorm = fireStorm;
                firestormClass.IsUsed = false;
                this._lstFireStorm.Add(firestormClass);
            }
        }

        public int GetIndexFireStorm()
        {
            int index = -1;
            foreach (var firestorm in this._lstFireStorm)
            {
                index++;
                if (!firestorm.IsUsed)
                {
                    firestorm.IsUsed = true;
                    return index;
                }
            }
            return -1;
        }

        public void FirestormStart()
        {
            StartCoroutine(this.FirestormAttacking(GetIndexFireStorm()));
        }

        public IEnumerator FirestormAttacking(int index)
        {
            float total = 0f;
            if (index != -1)
            {
                this.LstFireMove.ShuffleList();
                for (int i = 0; i < this._sizeFire; ++i)
                {
                    this._lstFireStorm[index].Firestorm[i].FireView.gameObject.SetActive(true);
                    this._lstFireStorm[index].Firestorm[i].FireView.FireFalling(i);
                    float time = 0f;
                    if (total < 2f && i == this._sizeFire - 1)
                    {
                        time = 2f - total;
                    }
                    else
                    {
                        time = Random.Range(0.1f, 0.22f);
                        total += time;
                    }
                    yield return new WaitForSeconds(time);
                }
            }
            QueueController.Instance.QueueElementModelList[2].QueueElementView.EndSkill();
        }
    }
}