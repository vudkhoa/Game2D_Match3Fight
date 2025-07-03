namespace Controller.Queue
{
    using Controller.Enemy;
    using Controller.Player;
    using Controller.Skill;
    using CustomData;
    using CustomUtils;
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

        private void Start()
        {
            this.CreateQueue();
        }

        private void Update()
        {
            if (this.QueueElementModelList == null || !EnemyController.Instance.ExistEnemy())
            {
                return;
            }

            if (!SetStateSkill())
            {
                return;
            }

            if (this.QueueElementModelList[0].Count > 0 && SkillController.Instance.State == 1 && this.QueueElementModelList[0].ReduceCount(1))
            { 
                PlayerController.Instance.Shooting();
            }
            else if (this.QueueElementModelList[1].Count > 0 && SkillController.Instance.State == 2 && this.QueueElementModelList[1].ReduceCount(1))
            {
                SkillController.Instance.ShowFlag();
            }
        }

        private bool SetStateSkill()
        {
            if (this.QueueElementModelList[0].Count > 0)
            {
                SkillController.Instance.State = 1;
                return true;
            }
            else if (this.QueueElementModelList[1].Count > 0)
            {
                SkillController.Instance.State = 2;
                return true;
            }
            else
            {
                SkillController.Instance.State = 0;
                return false;
            }
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
                this.QueueElementModelList[i] = queueModel;
            }
        }

        public void PlusCount(MatrixElementType type)
        {
            if (this.QueueElementModelList == null) return;
            for (int i = 0; i < this.QueueElementModelList.Length; ++i)
            {
                if (this.QueueElementModelList[i].Type == type)
                {
                    this.QueueElementModelList[i].PlusCount(1);
                }
            }
        }
    }
}
