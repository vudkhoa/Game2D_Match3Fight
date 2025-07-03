using View.Skill.Flag;

namespace Model.Skill.Flag
{
    public class FlagModel
    {
        public FlagView FlagView;
        public bool IsUsed = false;

        public void SetView(FlagView flagView)
        {
            this.FlagView = flagView;
        }

        public void SetActive(bool isActive)
        {
            this.FlagView.SetActive(isActive);
        }
    }
}