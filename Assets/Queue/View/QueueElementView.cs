namespace View.Queue {
    using CustomData;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class QueueElementView : MonoBehaviour
    {
        [Header("Element")]
        public TextMeshProUGUI Count;
        public Image ImageBackground;
        public Image ImageSkill;

        public void SetType(MatrixElementType type)
        {
            this.ImageBackground.sprite = DataManager.Instance.queueElemntSkinData.QueueElementSkins[(int)type - 1].Sprite;
            this.ImageSkill.sprite = DataManager.Instance.IconSkillData.IconSkills[(int)type - 1].Sprite;
        }

        public void SetCount(int count)
        {
            this.Count.text = count.ToString();
        }
    }
}