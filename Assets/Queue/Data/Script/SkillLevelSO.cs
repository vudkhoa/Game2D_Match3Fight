namespace CustomData
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class Skill
    {
        public MatrixElementType nameSkill;
    }

    [Serializable]
    public class SkillLevel
    {
        public int levelId;
        public Skill[] skills = new Skill[3]
        {
            new Skill {nameSkill = MatrixElementType.Null },
            new Skill {nameSkill = MatrixElementType.Null },
            new Skill {nameSkill = MatrixElementType.Null }
        };
    }

    [CreateAssetMenu(menuName = "SkillLevelData", fileName = "SkillLevelData")]
    public class SkillLevelSO : ScriptableObject
    {
        public List<SkillLevel> skillLevels = new List<SkillLevel>();
    }
}