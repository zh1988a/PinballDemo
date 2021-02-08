using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool m_bIsPlayer = true;
    public int m_maxHp;
    public int m_hp;
    public List<Buff> m_buffs = new List<Buff>();
    public bool m_isIce = false;
    public bool m_isProtect = false;

    public void Init(bool isPlayer)
    {
        m_bIsPlayer = isPlayer;
        m_maxHp = Defines.Instance.BaseHp;
        m_hp = Defines.Instance.BaseHp;
        m_isIce = false;
        m_isProtect = false;
    }

    private void LogPlayer()
    {
        Debug.LogFormat("Player::{0}    m_maxHp::{1}    m_hp::{2}", m_bIsPlayer, m_maxHp, m_hp);
    }

    public void OnDamage(Ball ball)
    {
        if(m_isProtect)
        {
            return;
            LogPlayer();
        }

        int addAttack = 0;
        float critDamage = 0;
        float vampire = 0;
        int heal = 0;
        int life = 0;
        bool isIce = false;
        bool isProtect = false;

        foreach(SkillConfigItem skill in ball.m_skills)
        {
            switch (skill.Type)
            {
                case SkillType.Attack:
                    addAttack += (int)skill.Value;
                    break;
                case SkillType.Crit:
                    Random.seed = System.DateTime.Now.Millisecond;
                    float percent = Random.RandomRange(0.0f, 1.0f);
                    if(percent < skill.Percent)
                    {
                        critDamage += skill.Value;
                    }
                    break;
                case SkillType.Vampire:
                    vampire += skill.Value;
                    break;
                case SkillType.Gas:
                    m_buffs.Add(new Buff(skill));
                    break;
                case SkillType.Heal:
                    heal += (int)skill.Value;
                    break;
                case SkillType.Life:
                    life += (int)skill.Value;
                    break;
                case SkillType.Ice:
                    isIce = true;
                    break;
                case SkillType.Protect:
                    isProtect = true;
                    break;
            }
        }

        int attack = ball.m_baseAttack + addAttack;
        float damage = (float)attack;
        damage = damage + damage * critDamage;

        m_hp -= (int)damage;
        LogPlayer();
        if (m_hp <= 0)
        {
            GameManager.Instance.GameOver(!m_bIsPlayer);
            return;
        }

        Player other = GameManager.Instance.IsPlayerRound ? GameManager.Instance.m_player : GameManager.Instance.m_npc;
        other.m_maxHp += life;
        float healHp = heal + damage * vampire;
        other.m_hp += (int)healHp;
        if(other.m_hp > other.m_maxHp)
        {
            other.m_hp = other.m_maxHp;
        }

        if(isIce)
        {
            m_isIce = true;
        }

        if(isProtect)
        {
            other.m_isProtect = true;
        }
    }

    public void OnSwitchTurn()
    {
        int buffDamage = 0;
        List<Buff> newBuffs = new List<Buff>();
        foreach(Buff buff in m_buffs)
        {
            buffDamage += (int)buff.Skill.Value;
            buff.LeftRound--;

            if(buff.LeftRound >0)
            {
                newBuffs.Add(buff);
            }
        }

        m_buffs = newBuffs;

        m_hp -= buffDamage;
        LogPlayer();
        if (m_hp <= 0)
        {
            GameManager.Instance.GameOver(!m_bIsPlayer);
            return;
        }

        m_isProtect = false;
    }

}
