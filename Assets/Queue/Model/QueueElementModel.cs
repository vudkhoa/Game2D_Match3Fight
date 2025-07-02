namespace Model.Queue {
    using View.Queue;
    using CustomData;

    public class QueueElementModel
    {
        public QueueElementView QueueElementView;
        public bool IsActive = false;
        public int Count = 0;
        public MatrixElementType Type;

        public void Init(QueueElementView queueElementView)
        {
            this.QueueElementView = queueElementView;
        }

        public void PlusCount(int count)
        {
            this.Count += count;
            this.QueueElementView.SetCount(this.Count);
        }

        public void SetType(MatrixElementType type)
        {
            this.Type = type;
            this.QueueElementView.SetType(type);
        }
    }
}