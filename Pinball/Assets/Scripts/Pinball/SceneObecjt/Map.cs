using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int Weight = 1;
    public int SkillCount = 0;

    private List<NPCFirePoint> m_firePoints;
    private NPCFirePoint m_targetPoint = null;
    public NPCFirePoint TargetPoint
    {
        get
        {
            if(!m_targetPoint)
            {
                m_firePoints = new List<NPCFirePoint>();
                GameObject fpParent = gameObject.transform.Find("NPCAttackPoint").gameObject;
                NPCFirePoint[] fpList = fpParent.GetComponentsInChildren<NPCFirePoint>();
                foreach (NPCFirePoint point in fpList)
                {
                    for (int i = 0; i < point.Weight; ++i)
                    {
                        m_firePoints.Add(point);
                    }
                }

                Random.seed = System.DateTime.Now.Millisecond;
                int randomIndex = Random.Range(0, m_firePoints.Count - 1);
                m_targetPoint = m_firePoints[randomIndex];
            }
            return m_targetPoint;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
       

        m_targetPoint = null;

        GameObject skillParent = gameObject.transform.Find("SkillPoint").gameObject;
        SkillObject[] skillList = skillParent.GetComponentsInChildren<SkillObject>();
        int count = PlayerData.Instance.Equips.Count;
        List<bool> indexs = new List<bool>();
        for(int i = 0; i < count; ++i)
        {
            indexs.Add(false);
        }

        bool haveStatic = false;
        foreach(SkillObject obj in skillList)
        {
            Random.seed = System.DateTime.Now.Millisecond;

            if(!haveStatic)
            {
                int percent = Random.RandomRange(0, 100);
                if (percent <= 5)
                {
                    percent = Random.RandomRange(0, 100);

                    string skillName = percent <= 50 ? "1027" : "1028";
                    obj.Init(SkillConfig.Get(skillName));
                    haveStatic = true;
                    continue;
                }
            }
           

            int index = Random.RandomRange(0, indexs.Count - 1);
            if(indexs[index])
            {
                for(int i = 0; i < indexs.Count; ++i)
                {
                    if(!indexs[i])
                    {
                        index = i;
                        break;
                    }
                }
            }
            obj.Init(PlayerData.Instance.Equips[index]);
            indexs[index] = true;

            SkillCount++;
        }
        //List<SkillConfigItem> skills = PlayerData.Instance.Equips;
        //skills.s
        //foreach()
    }
}
