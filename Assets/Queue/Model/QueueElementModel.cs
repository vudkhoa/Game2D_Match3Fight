namespace Model.Queue {
    using View.Queue;
    using CustomData;
    using UnityEngine;

    public class QueueElementModel
    {
        public QueueElementView QueueElementView;
        public bool IsActive = false;
        public int Count = 0;
        public MatrixElementType Type;
        private float _cooldownTime = 0f;
        private float _castTime = 0f;

        public void SetView(QueueElementView queueElementView)
        {
            this.QueueElementView = queueElementView;
        }

        public void SetCooldownTime(float time)
        {
            this._cooldownTime = time;
        }

        public void PlusCount(int count)
        {
            this.Count += count;
            this.QueueElementView.SetCount(this.Count);
        }

        public bool ReduceCount(int count)
        {
            if (this.Count > 0 && !this.QueueElementView.OnCooldown && !this.QueueElementView.IsGlow)
            {
                this.Count -= count;
                this.QueueElementView.SetCount(this.Count);
                return true;
            }
            return false;
        }

        public void SetType(MatrixElementType type)
        {
            this.Type = type;
            this.QueueElementView.SetType(type);
            this._cooldownTime = DataManager.Instance.SkillData.Skills[(int)type - 1].cooldownTime;
            this._castTime = DataManager.Instance.SkillData.Skills[(int)type - 1].castTime;
            this.QueueElementView.SetTimes(this._cooldownTime, this._castTime);
        }
    }
}