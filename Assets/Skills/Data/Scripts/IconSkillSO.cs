namespace CustomData
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    
    [Serializable]
    public class SkillIcon
    {
        public Sprite Sprite;
    }

    [CreateAssetMenu(menuName = "IconSkillSO", fileName = "IconSkillSO")]
    public class IconSkillSO : ScriptableObject
    {
        public List<QElementSkin> IconSkills;
    }
}
