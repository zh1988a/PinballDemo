using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect : MonoBehaviour
{
    public List<Image> equipImages;
    public List<Image> selectImages;
    public Image goImage;

    private List<SkillConfigItem> m_skills;
    private List<int> m_selectStatus = new List<int>();
    private List<int> m_equipsStatus = new List<int>();

    public void Init()
    {
        m_skills = PlayerData.Instance.RandomSkill();
        m_selectStatus.Clear();
        for(int i = 0; i < 9; ++i)
        {
            if(i < m_skills.Count)
            {
                m_selectStatus.Add(1);
            }
            else
            {
                m_selectStatus.Add(-1);
            }
        }

        m_equipsStatus.Clear();
        for(int i = 0; i < 6; ++i)
        {
            m_equipsStatus.Add(-1);
        }

        Refresh();
    }

    public void Equip(int index)
    {
        if(m_selectStatus[index] == 0)
        {
            return;
        }

        for(int i = 0; i < 6; ++i)
        {
            if(-1 == m_equipsStatus[i])
            {
                m_equipsStatus[i] = index;
                m_selectStatus[index] = 0;
                break;
            }
        }
        Refresh();
    }

    public void UnEquip(int index)
    {
        if(-1 == m_equipsStatus[index])
        {
            return;
        }

        m_selectStatus[m_equipsStatus[index]] = 1;
        m_equipsStatus[index] = -1;
        Refresh();
    }

    public void Refresh()
    {
        for(int i = 0; i < m_equipsStatus.Count; ++i)
        {
            if(m_equipsStatus[i] == -1)
            {
                equipImages[i].sprite = null;
            }
            else
            {
                SkillConfigItem skill = m_skills[m_equipsStatus[i]];
                equipImages[i].sprite = Defines.Instance.GetSprite(skill.IconName);
            }
        }

        for(int i = 0; i < m_selectStatus.Count; ++i)
        {
            int status = m_selectStatus[i];
            if (status == -1)
            {
                selectImages[i].gameObject.SetActive(false);
            }
            else
            {
                selectImages[i].gameObject.SetActive(true);
                selectImages[i].sprite = Defines.Instance.GetSprite(m_skills[i].IconName);
                if (status == 0)
                {
                    selectImages[i].color = Color.gray;
                }
                else
                {
                    selectImages[i].color = Color.white;
                }
            }
        }

        bool cango = true;
        for (int i = 0; i < m_equipsStatus.Count; ++i)
        {
            if (m_equipsStatus[i] == -1)
            {
                cango = false;
                break;
            }
        }

        goImage.color = cango ? Color.white : Color.gray;
    }

    public void OnClickGo()
    {
        for(int i = 0; i < m_equipsStatus.Count; ++i)
        {
            if(m_equipsStatus[i] == -1)
            {
                return;
            }
        }

        List<SkillConfigItem> equipSkills = new List<SkillConfigItem>();
        for(int i = 0; i < m_equipsStatus.Count; ++i)
        {
            equipSkills.Add(m_skills[m_equipsStatus[i]]);
        }

        PlayerData.Instance.m_equips = equipSkills;
        GameManager.Instance.OpenVSUI();
    }

}
