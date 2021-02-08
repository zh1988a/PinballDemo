using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private static PlayerData _instance;
    public static PlayerData Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new PlayerData();
            }

            return _instance;
        }
    }

    public List<SkillConfigItem> m_skills;
    private List<SkillConfigItem> m_equips;
    public List<SkillConfigItem> Equips
    {
        get
        {
            List<SkillConfigItem> skills = new List<SkillConfigItem>();
            skills.Add(SkillConfig.Get("1001"));
            skills.Add(SkillConfig.Get("1002"));
            skills.Add(SkillConfig.Get("1005"));
            skills.Add(SkillConfig.Get("1009"));
            skills.Add(SkillConfig.Get("1010"));
            skills.Add(SkillConfig.Get("1013"));
            return skills;
            return m_skills;
        }
    }
}
