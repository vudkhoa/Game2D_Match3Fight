namespace Model.Skill.Flag
{
    using View.Skill.Bullet;

    public class FireModel
    {
        public FireView FireView; 
        
        public void SetView(FireView fireView)
        {
            this.FireView = fireView;
        }
    }
}