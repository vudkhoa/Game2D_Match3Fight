using CustomData;
using CustomUtils;
using UnityEngine;

public class QueueController : SingletonMono<QueueController>
{
    [Header("Queue Draw")]
    public GameObject QueuePrefab;
    public RectTransform QueueParent;
    public QueueData QueueData;

    protected override void Awake()
    {
        this.CreateQueue();
    }

    private void CreateQueue()
    {
        // Queue
        int size = QueueData.InformationQueue.Size;
        float widthQueue = QueueData.InformationQueue.Width;
        float heightQueue = QueueData.InformationQueue.Height;
        float spacing = QueueData.InformationQueue.Spacing;
        float padding = QueueData.InformationQueue.Padding;

        // Element
        float ratioHeight = QueueData.InformationElement.RatioHeight;
        float widthElement = QueueData.InformationElement.Width;


        Vector3 pos = new Vector3(padding, 1 - ratioHeight, ratioHeight);
        Vector2 scale = new Vector2(widthQueue - padding - widthElement, ratioHeight * heightQueue);

        

        for (int i = 0;  i < size; i++)
        {
            GameObject QueueElementGO = Instantiate(QueuePrefab, QueueParent);
            RectTransform trans = QueueElementGO.GetComponent<RectTransform>();

            trans.offsetMin = new Vector2(pos.x, pos.y);
            trans.offsetMax = new Vector2(-scale.x, scale.y);
            Vector2 newPos = trans.anchoredPosition;
            newPos.y = 0;
            trans.anchoredPosition = newPos;
           

            pos.x += (widthElement + spacing);
            scale.x -= (widthElement + spacing);
        }
    }

}
