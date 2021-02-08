using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour
{
    public SkillConfigItem m_data;

    public void Init(SkillConfigItem skill)
    {
        m_data = skill;
        //set iamge
        gameObject.GetComponent<SpriteRenderer>().sprite = Defines.Instance.GetSprite(skill.IconName);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
