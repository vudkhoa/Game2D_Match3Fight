namespace Manager
{
    using CustomUtils;
    using System.Collections;
    using System.Collections.Generic;
    using UI;
    using UI.Lose;
    using UnityEngine;

    public class GameManager : SingletonMono<GameManager>
    {
        private void Start()
        {
            
        }

        public void LoseGame()
        {
            this.ShowUILose();
        }

        private void ShowUILose()
        {
            UIManager.Instance.OpenUI<LoseUI>();
        }
    }
}