namespace Controller.Queue
{
    using Model.Queue;
    using View.Queue;
    using CustomData;
    using CustomUtils;
    using UnityEngine;

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
                queueModel.Init(queueView);
                queueModel.SetType(DataManager.Instance.SkillLevelData.skillLevels[0].skills[i].nameSkill);
                queueModel.PlusCount(0);
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
                    //Debug.Log("Ok");
                    this.QueueElementModelList[i].PlusCount(1);
                }
            }
        }
    }
}
