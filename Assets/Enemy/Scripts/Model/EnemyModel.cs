namespace Model.Enemy
{
    using View.Enemy;
    public class EnemyModel
    {
        public EnemyView EnemyView;
        public bool IsDead = false;

        public void SetView(EnemyView enemyView)
        {
            this.EnemyView = enemyView;
        }
    }
}