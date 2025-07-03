namespace Controller.Player
{
    using UnityEngine;
    using View.Player;
    using CustomUtils;
    using Controller.Skill;
    using Controller.Enemy;
    using Model.Player;

    public class PlayerController : SingletonMono<PlayerController>
    {
        [Header(" View ")]
        public PlayerView PlayerPrefab;
        public RectTransform PlayerParent;

        private PlayerModel _playerModel;

        private void Start()
        {
            this.SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            this._playerModel = new PlayerModel();
            PlayerView playerView = Instantiate(this.PlayerPrefab, PlayerParent);
            this._playerModel.SetView(playerView);
        }

        public void Shooting()
        {
            SkillController.Instance.Shooting(EnemyController.Instance.GetIndex());
            this._playerModel.PlayerView.SetState(1);
        }

        public Vector2 GetPosPlayer()
        {
            return this._playerModel.PlayerView.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
