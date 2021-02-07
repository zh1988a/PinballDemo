using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject ballPrefab;
    public GameObject skillPrefab;

    private void Awake()
    {
        _instance = Instance;
        GameObject.DontDestroyOnLoad(gameObject);

        
    }

    private void Start()
    {
        
    }

    public void Init()
    {
        SkillConfig.LoadConfig();
        MapConfig.LoadConfig();

        SimplePool.Preload(ballPrefab, 5);
        SimplePool.Preload(skillPrefab, 10);
    }

    
    #region game logic

    public bool IsPlayerRound = false;

    public Player m_player;
    public Player m_npc;

    public void DoDamage(bool isPlayer, Ball ball)
    {
        if(isPlayer)
        {
            m_player.OnDamage(ball);
        }
        else
        {
            m_npc.OnDamage(ball);
        }
    }

    public void FireBall(Vector2 dir)
    {

    }

    public void GameOver(bool isWin)
    {

    }

    public void NextRound()
    {
        IsPlayerRound = !IsPlayerRound;
    }
    #endregion

    #region ingame ui

    #endregion

    #region out game ui

    #endregion
}
