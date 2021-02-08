using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingame : MonoBehaviour
{
    public Image playerBar;
    public Text playerHP;
    public Image npcBar;
    public Text npcHP;

    public void Init()
    {
        playerBar.transform.localScale = Vector3.one;
        playerHP.text = "";
        npcBar.transform.localScale = Vector3.one;
        npcHP.text = "";
    }

    public void RefreshHP()
    {
        int pHP = GameManager.Instance.m_player.m_hp;
        if(pHP < 0)
        {
            pHP = 0;
        }
        int pLife = GameManager.Instance.m_player.m_maxHp;
        int nHP = GameManager.Instance.m_npc.m_hp;
        if (nHP < 0)
        {
            nHP = 0;
        }
        int nLife = GameManager.Instance.m_npc.m_maxHp;

        playerBar.transform.localScale = new Vector3((float)pHP / pLife, 1, 1);
        playerHP.text = pHP + "/" + pLife;
        npcBar.transform.localScale = new Vector3((float)nHP / nLife, 1, 1);
        npcHP.text = nHP + "/" + nLife;
    }
}
