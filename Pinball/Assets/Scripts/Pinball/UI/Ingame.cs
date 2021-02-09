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
    public SkillItem skillPrefab;
    public Transform playerBuffRoot;
    public Transform npcBuffRoot;

    public GameObject turnBg;
    public GameObject playerTurn;
    public GameObject npcTurn;

    public GameObject miss;
    public GameObject perfect;

    private List<SkillItem> m_buffs = new List<SkillItem>();
    private List<SkillItem> m_npcBuffs = new List<SkillItem>();

    public void Init()
    {
        playerBar.transform.localScale = Vector3.one;
        playerHP.text = "";
        npcBar.transform.localScale = Vector3.one;
        npcHP.text = "";
        turnBg.SetActive(false);
        playerTurn.gameObject.SetActive(false);
        npcTurn.gameObject.SetActive(false);
        miss.gameObject.SetActive(false);
        perfect.gameObject.SetActive(false);
        SimplePool.Preload(skillPrefab.gameObject, 10);
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

    public void RefreshBuff()
    {
        foreach(SkillItem skill in m_buffs)
        {
            SimplePool.Despawn(skill.gameObject);
        }
        m_buffs.Clear();

        Player player = GameManager.Instance.m_player;
        if(player.m_isIce)
        {
            SkillItem so = SimplePool.Spawn(skillPrefab.gameObject,Vector3.zero,Quaternion.Euler(0,0,0)).GetComponent<SkillItem>();
            so.transform.parent = playerBuffRoot;
            so.gameObject.SetActive(true);
            so.Set(SkillConfig.Get("1027"));
            m_buffs.Add(so);
        }

        if (player.m_isProtect)
        {
            SkillItem so = SimplePool.Spawn(skillPrefab.gameObject, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<SkillItem>();
            so.transform.parent = playerBuffRoot;
            so.gameObject.SetActive(true);
            so.Set(SkillConfig.Get("1028"));
            m_buffs.Add(so);
        }

        foreach(Buff buff in player.m_buffs)
        {
            SkillItem so = SimplePool.Spawn(skillPrefab.gameObject, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<SkillItem>();
            so.transform.parent = playerBuffRoot;
            so.gameObject.SetActive(true);
            so.Set(buff);
            m_buffs.Add(so);
        }

        foreach (SkillItem skill in m_npcBuffs)
        {
            SimplePool.Despawn(skill.gameObject);
        }
        m_npcBuffs.Clear();

        Player npc = GameManager.Instance.m_npc;
        if (npc.m_isIce)
        {
            SkillItem so = SimplePool.Spawn(skillPrefab.gameObject, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<SkillItem>();
            so.transform.parent = npcBuffRoot;
            so.gameObject.SetActive(true);
            so.Set(SkillConfig.Get("1027"));
            m_npcBuffs.Add(so);
        }

        if (npc.m_isProtect)
        {
            SkillItem so = SimplePool.Spawn(skillPrefab.gameObject, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<SkillItem>();
            so.transform.parent = npcBuffRoot;
            so.gameObject.SetActive(true);
            so.Set(SkillConfig.Get("1028"));
            m_npcBuffs.Add(so);
        }

        foreach (Buff buff in npc.m_buffs)
        {
            SkillItem so = SimplePool.Spawn(skillPrefab.gameObject, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<SkillItem>();
            so.transform.parent = npcBuffRoot;
            so.gameObject.SetActive(true);
            so.Set(buff);
            m_npcBuffs.Add(so);
        }
    }

    public void ShowTurn(bool show, bool isPlayer)
    {
        turnBg.SetActive(show);
        GameObject turn = isPlayer ? playerTurn : npcTurn;
        turn.SetActive(show);
    }

    public void ShowMiss(bool show)
    {
        miss.gameObject.SetActive(show);
    }

    public void ShowPerfect(bool show)
    {
        perfect.gameObject.SetActive(show);
    }
}
