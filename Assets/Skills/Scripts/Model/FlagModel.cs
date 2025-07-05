namespace Model.Skill.Flag
{
    using View.Skill.Flag;
    public class FlagModel
    {
        public FlagView FlagView;
        public bool IsUsed = false;
        public int IndexEnemy = -1;

        public void SetView(FlagView flagView)
        {
            this.FlagView = flagView;
        }

        public void SetIndexEnemy(int index)
        {
            this.IndexEnemy = index;
        }   

        public void SetActive(bool isActive)
        {
            this.FlagView.SetActive(isActive);
        }
    }
}