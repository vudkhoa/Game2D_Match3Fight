namespace CustomData
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    
    [Serializable]
    public class SkillIconForQueue
    {
        public Sprite Sprite;
    }

    [Serializable]
    public class SkillIconForMatrix
    {
        public Sprite Sprite;
    }

    [Serializable]
    public class SkillIcon
    {
        public SkillIconForQueue QueueIcon;
        public SkillIconForMatrix MatrixIcon;
    }

    [Serializable]
    public class SkillBackgroundForQueue
    {
        public Sprite Sprite;
    }

    [Serializable]
    public class SkillBackgroundForMatrix
    {
        public Sprite Sprite;
    }

    [Serializable]
    public class SkillBackground
    {
        public SkillBackgroundForQueue QueueBackground;
        public SkillBackgroundForMatrix MatrixBackground;
    }

    [Serializable]
    public class SkillSkin
    {
        public SkillIcon SkillIcon;
        public SkillBackground SkillBackground;
        public float cooldownTime;
    }

    [CreateAssetMenu(menuName = "SkillSO", fileName = "SkillSO")]
    public class SkillSO : ScriptableObject
    {
        public List<SkillSkin> Skills;
    }
}
