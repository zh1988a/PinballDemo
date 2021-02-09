using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using Newtonsoft.Json;

public class PlayerData
{
    public const string SKILL_NAME = "skills";

    private static PlayerData _instance;
    public static PlayerData Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new PlayerData();
                //_instance.Init();
            }

            return _instance;
        }
    }

    public List<bool> m_skillIDs;
    public List<SkillConfigItem> m_skills
    {
        get
        {
            List<SkillConfigItem> ret = new List<SkillConfigItem>();
            for(int i = 0; i <= 26; ++ i)
            {
                if(m_skillIDs[i])
                {
                    ret.Add(SkillConfig.Get(StrID(i)));
                }
            }
            return ret;
        }
    }
    public List<SkillConfigItem> m_equips;
    public List<SkillConfigItem> Equips
    {
        get
        {
            return m_equips;
            //List<SkillConfigItem> skills = new List<SkillConfigItem>();
            ////skills.Add(SkillConfig.Get("1001"));
            ////skills.Add(SkillConfig.Get("1002"));
            ////skills.Add(SkillConfig.Get("1005"));
            ////skills.Add(SkillConfig.Get("1009"));
            ////skills.Add(SkillConfig.Get("1010"));
            ////skills.Add(SkillConfig.Get("1013"));
            //skills.Add(SkillConfig.Get("1001"));
            //skills.Add(SkillConfig.Get("1002"));
            //skills.Add(SkillConfig.Get("1005"));
            //skills.Add(SkillConfig.Get("1021"));
            //skills.Add(SkillConfig.Get("1020"));
            //skills.Add(SkillConfig.Get("1019"));
            //return skills;
        }
    }

    public void Init()
    {
        if(ObscuredPrefs.HasKey(SKILL_NAME))
        {
            string json = ObscuredPrefs.GetString(SKILL_NAME);
            m_skillIDs = JsonConvert.DeserializeObject<List<bool>>(json);
        }
        else
        {
            m_skillIDs = new List<bool>();
            for(int i = 0; i <= 26; ++i)
            {
                if(i == 1 || i == 2 || i ==5 || i ==9 || i ==10 || i == 13)
                {
                    m_skillIDs.Add(true);
                }
                else
                {
                    m_skillIDs.Add(false);
                }
                
            }

            SaveSkills();
        }
    }

    public void SaveSkills()
    {
        string json = JsonConvert.SerializeObject(m_skillIDs);
        ObscuredPrefs.SetString(SKILL_NAME, json);
    }

    public string StudySkill()
    {
        List<SkillConfigItem> needList = new List<SkillConfigItem>();
        for(int i = 1; i <= 26; ++i)
        {
            if(!m_skillIDs[i])
            {
                needList.Add(SkillConfig.Get(StrID(i)));
            }
        }

        List<SkillConfigItem> weightList = new List<SkillConfigItem>();
        foreach(SkillConfigItem item in needList)
        {
            for(int i = 0; i < item.Weight; ++i)
            {
                weightList.Add(item);
            }
        }

        Random.seed = System.DateTime.Now.Millisecond;
        int randomIndex = Random.Range(0, weightList.Count - 1);
        m_skillIDs[IntIndex(weightList[randomIndex].ID)] = true;
        SaveSkills();

        return weightList[randomIndex].ID;
    }

    public string StrID(int index)
    {
        if(index < 10)
        {
            return "100" + index; 
        }
        else
        {
            return "10" + index;
        }
    }

    public int IntIndex(string id)
    {
        return int.Parse(id) - 1000;
    }

    public List<SkillConfigItem> RandomSkill()
    {
        List<SkillConfigItem> ret = new List<SkillConfigItem>();

        List<SkillConfigItem> mySkills = m_skills;
        if(mySkills.Count <= 9)
        {
            return mySkills;
        }
        else
        {
            List<bool> indexs = new List<bool>();
            for(int i = 0; i < mySkills.Count; ++i)
            {
                indexs.Add(false);
            }

            Random.seed = System.DateTime.Now.Millisecond;
            while(ret.Count < 9)
            {
                int index = Random.RandomRange(0, indexs.Count - 1);
                if(indexs[index])
                {
                    for (int i = 0; i < indexs.Count; ++i)
                    {
                        if (!indexs[i])
                        {
                            index = i;
                            break;
                        }
                    }
                }

                ret.Add(mySkills[index]);
                indexs[index] = true;
            }

        }

        return ret;
    }
}
