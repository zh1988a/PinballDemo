using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    public Image Icon;
    public Text Number;

    public void Set(Buff buff)
    {
        Icon.sprite = Defines.Instance.GetSprite(buff.Skill.IconName);
        if(Number)
        {
            Number.gameObject.SetActive(true);
            Number.text = buff.LeftRound.ToString();
        }
    }

    public void Set(SkillConfigItem skll)
    {
        Icon.sprite = Defines.Instance.GetSprite(skll.IconName);
        if (Number)
        {
            Number.gameObject.SetActive(false);
        }
    }
}
