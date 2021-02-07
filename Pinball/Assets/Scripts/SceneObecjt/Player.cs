using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool m_bIsPlayer = true;
    public float m_hp;

    public void Init(bool isPlayer)
    {
        m_bIsPlayer = isPlayer;
        m_hp = Defines.Instance.BaseHp;
    }

    public void OnDamage(Ball ball)
    {
        m_hp -= ball.m_damage;
        if(m_hp <= 0)
        {
            GameManager.Instance.GameOver(!m_bIsPlayer);
            return;
        }

        //skill effect
    }
}
