using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;

    public Image SkillIcon;

    public void Init(bool isWin)
    {
        win.SetActive(isWin);
        lose.SetActive(!isWin);

        if(isWin)
        {
            string id = PlayerData.Instance.StudySkill();
            SkillIcon.sprite = Defines.Instance.GetSprite(SkillConfig.Get(id).IconName);
        }
    }
}
