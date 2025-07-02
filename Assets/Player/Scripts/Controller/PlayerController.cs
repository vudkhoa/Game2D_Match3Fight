namespace Controller.Player
{
    using UnityEngine;
    using View.Player;
    using CustomUtils;
    using Controller.Skill;
    using Controller.Enemy;

    public class PlayerController : SingletonMono<PlayerController>
    {
        [Header(" View ")]
        public PlayerView PlayerView;

        public void Shooting()
        {
            this.PlayerView.SetState(1);
            SkillController.Instance.Shooting(EnemyController.Instance.GetIndex());
        }
    }
}
