using View.Player;

namespace Model.Player
{

    public class PlayerModel
    {
        public PlayerView PlayerView;

        public void SetView(PlayerView playerView)
        {
            PlayerView = playerView;
        }
    }
}
