namespace Model.Skill.Bullet
{
    using UnityEngine;
    using View.Skill.Bullet;

    public class BulletModel
    {
        public BulletView BulletView;
        public bool IsUsed = false;
        
        public void SetView(BulletView bulletView)
        {
            this.BulletView = bulletView;
        }
    }
}