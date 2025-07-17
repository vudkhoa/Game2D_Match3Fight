namespace Controller.Queue
{
    using Controller.Enemy;
    using Controller.Player;
    using Controller.Skill;
    using Cotroller;
    using CustomData;
    using CustomUtils;
    using Manager;
    using Model.Queue;
    using UnityEngine;
    using View.Queue;

    public class QueueController : SingletonMono<QueueController>
    {
        [Header("Queue Draw")]
        public QueueElementView QueuePrefab;
        public RectTransform QueueParent;

        [Header("Queue Data")]
        public QueueElementModel[] QueueElementModelList;

        public bool IsStart = false;

        private void Start()
        {
            this.CreateQueue();
        }

        private void Update()
        {
            // GameOver
            if (CoreGamePlayController.Instance.State == 2)
            {
                return;
            }
            // Phase 1
            if (!this.IsStart)
            {
                return;
            }
            // Not Skill
            if (!SetStateSkill())
            {
                return;
            }

            if (this.QueueElementModelList == null)
            {
                return;
            }

            int state = SkillController.Instance.State;
            if (!EnemyController.Instance.ExistEnemy(state))
            {
                return;
            }

            if (this.QueueElementModelList[0].Count > 0 && SkillController.Instance.State == 1 && this.QueueElementModelList[0].ReduceCount(1))
            {
                this.QueueElementModelList[0].QueueElementView.StartSkill();
                PlayerController.Instance.Shooting();
                this.QueueElementModelList[0].QueueElementView.StartGlow();
            }
            else if (this.QueueElementModelList[1].Count > 0 && SkillController.Instance.State == 2 && this.QueueElementModelList[1].ReduceCount(1))
            {
                this.QueueElementModelList[1].QueueElementView.StartSkill();
                SkillController.Instance.ShowFlag();
                this.QueueElementModelList[1].QueueElementView.StartGlow();
            }
            else if (this.QueueElementModelList[2].Count > 0 && SkillController.Instance.State == 3 && this.QueueElementModelList[2].ReduceCount(1))
            {
                this.QueueElementModelList[2].QueueElementView.StartSkill();
                SkillController.Instance.FirestormStart();
                this.QueueElementModelList[2].QueueElementView.StartGlow();
            }
        }

        private bool SetStateSkill()
        {
            if (this.QueueElementModelList[0].Count > 0 && !this.QueueElementModelList[0].QueueElementView.IsGlow && !this.QueueElementModelList[0].QueueElementView.OnCooldown)
            {
                SkillController.Instance.State = 1;
                return true;
            }
            else if (this.QueueElementModelList[1].Count > 0 && !this.QueueElementModelList[1].QueueElementView.IsGlow && !this.QueueElementModelList[1].QueueElementView.OnCooldown)
            {
                SkillController.Instance.State = 2;
                return true;
            }
            else if (this.QueueElementModelList[2].Count > 0 && !this.QueueElementModelList[2].QueueElementView.IsGlow && !this.QueueElementModelList[2].QueueElementView.OnCooldown)
            {
                SkillController.Instance.State = 3;
                return true;
            }
            else 
            if (
                    this.QueueElementModelList[0].Count <= 0 &&
                    this.QueueElementModelList[1].Count <= 0 &&
                    this.QueueElementModelList[2].Count <= 0 &&
                    EnemyController.Instance.CountEnemyDead 
                        != EnemyController.Instance.EnemySize
                ) 
            {
                CoreGamePlayController.Instance.State = 2;
                GameManager.Instance.LoseGame();
                SkillController.Instance.State = 0;
                return false;
            }
            return false;
        }

        private void CreateQueue()
        {
            // Init Quee Element Model List
            this.QueueElementModelList = new QueueElementModel[DataManager.Instance.QueueData.InformationQueue.Size];
            // Queue
            int size            = DataManager.Instance.QueueData.InformationQueue.Size;
            int direction       = DataManager.Instance.QueueData.InformationQueue.Direction;
            float widthQueue    = DataManager.Instance.QueueData.InformationQueue.Width;
            float heightQueue   = DataManager.Instance.QueueData.InformationQueue.Height;
            float spacing       = DataManager.Instance.QueueData.InformationQueue.Spacing;
            float paddingHon    = DataManager.Instance.QueueData.InformationQueue.PaddingHon;
            float paddingVer    = DataManager.Instance.QueueData.InformationQueue.PaddingVer;

            // Element
            float heightElement = DataManager.Instance.QueueData.InformationElement.Height;
            float widthElement  = DataManager.Instance.QueueData.InformationElement.Width;

            Vector3 pos = new Vector3((paddingHon + (widthElement / 2)) * (direction * (-1)), paddingVer, 0);
            pos.y = 0;
            pos.z = 0;
            Vector2 scale = new Vector2(widthElement, heightElement);

            for (int i = 0;  i < size; i++)
            {
                QueueElementView queueView = Instantiate(QueuePrefab, QueueParent);
                RectTransform trans = queueView.GetComponent<RectTransform>();

                trans.anchoredPosition = pos;
                trans.sizeDelta = scale;

                pos.x += direction * (widthElement) + direction * spacing;

                QueueElementModel queueModel = new QueueElementModel();
                queueModel.SetView(queueView);
                queueModel.SetType(DataManager.Instance.SkillLevelData.skillLevels[0].skills[i].nameSkill);
                queueModel.PlusCount(0);
                queueModel.SetCooldownTime(1f);
                queueModel.QueueElementView.GlowImage.gameObject.SetActive(false);
                this.QueueElementModelList[i] = queueModel;
            }
        }

        public void PlusCount(MatrixElementType type, int count)
        {
            if (this.QueueElementModelList == null) return;
            for (int i = 0; i < this.QueueElementModelList.Length; ++i)
            {
                if (this.QueueElementModelList[i].Type == type)
                {
                    this.QueueElementModelList[i].PlusCount(count);
                }
            }
        }
    }
}
