namespace Controller.Skill
{
    using CustomUtils;
    using Model.Skill.Bullet;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using View.Skill.Bullet;

    public class SkillController : SingletonMono<SkillController>
    {
        [Header(" Information ")]
        public BulletView BulletPrefab;
        public RectTransform BulletParent;

        [Header(" Test ")]
        public RectTransform Point1;
        //public RectTransform Point2;

        private List<BulletModel> _lstBullet;
        private int _bulletSize = 10;

        private void Start()
        {
            this.SpawnBullet();
        }

        private void SpawnBullet()
        {
            this._lstBullet = new List<BulletModel>();

            for (int i = 0; i < this._bulletSize; ++i)
            {
                BulletView bulletGO = Instantiate(this.BulletPrefab, this.BulletParent);

                BulletModel bulletModel = new BulletModel();
                bulletModel.SetView(bulletGO.GetComponent<BulletView>());
                bulletModel.BulletView.SetIndex(i);

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
                    this._lstBullet[i].BulletView.Shooting(
                        this.Point1.anchoredPosition,
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
    }
}