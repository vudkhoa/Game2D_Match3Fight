namespace Cotroller 
{
    using Controller.Queue;
    using CustomUtils;
    using UnityEngine;
    using UnityEngine.UI;

    public class CoreGamePlayController : SingletonMono<CoreGamePlayController>
    {
        [Header(" Container Prefab ")]
        public GameObject MatrixContainerPrefab;
        public GameObject QueueContainerPrefab;
        public GameObject FightContainerPrefab;
        public Image BgFight;
        public Image BgAll;

        private GameObject _matrixContainer;
        private GameObject _queueContainer;
        private GameObject _fightContainer;


        // 0: Đang Game, 1: Thắng, 2: Thua
        public int State = 0;

        private void Start()
        {
            this.SpawnBackground();
            this.SpawnContainer();
            this.StarPhase1();
        }

        private void SpawnBackground()
        {
            Instantiate(this.BgAll, this.transform);
            Instantiate(this.BgFight, this.transform);
        }

        private void SpawnContainer()
        {
            this._matrixContainer = Instantiate(this.MatrixContainerPrefab, this.transform);
            this._queueContainer = Instantiate(this.QueueContainerPrefab, this.transform);
            this._fightContainer = Instantiate(this.FightContainerPrefab, this.transform);
        }

        public void StarPhase1()
        {
            this._fightContainer.SetActive(false);
            QueueController.Instance.IsStart = false;
        }

        public void StartPhase2()
        {
            this._fightContainer.SetActive(true);
            QueueController.Instance.IsStart = true;
        }
    }
}