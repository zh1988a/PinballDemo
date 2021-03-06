﻿using System.Collections;
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

    public Launcher m_playerLauncher;
    public Launcher m_npcLauncher;

    public Transform m_playerFirePos;
    public Transform m_npcFirePos;

    public GameObject GameRoot;

    public List<Map> m_mapPrefabs;

    public List<Follower> m_trails;

    private void Awake()
    {
        _instance = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Application.targetFrameRate = 60;

        SkillConfig.LoadConfig();
        //MapConfig.LoadConfig();
        PlayerData.Instance.Init();

        SimplePool.Preload(ballPrefab, 1);

        m_curBall = SimplePool.Spawn(ballPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<Ball>();
        m_curBall.transform.parent = GameRoot.transform;
        m_curBall.gameObject.SetActive(false);

        Init();

        //start ui
        //StartGame();
    }

    public void Init()
    {

        IsGameStart = false;

        m_playerLauncher.gameObject.SetActive(false);
        m_npcLauncher.gameObject.SetActive(false);

        m_ingameUI.Init();
        OpenMainUI();

        //m_curMap.Init();
    }


    #region game logic

    public bool IsGameStart = false;
    public bool IsPlayerRound = false;

    public Player m_player;
    public Player m_npc;


    public int m_curRound = 1;

    public Ball m_curBall = null;
    public Map m_curMap = null;

    public void DoDamage(Ball ball)
    {
        if(!IsPlayerRound)
        {
            m_player.OnDamage(ball);
        }
        else
        {
            m_npc.OnDamage(ball);
        }

        //effect
        m_ingameUI.RefreshHP();
        m_ingameUI.RefreshBuff();

        SwitchTurn();
    }

    public void DoRelease(Ball ball)
    {
        //if()
        m_ingameUI.ShowMiss(true);
        //SwitchTurn();
    }

    public void FireBall(Vector2 dir)
    {
        Transform tt = IsPlayerRound ? m_playerFirePos.transform : m_npcFirePos;
        m_curBall.gameObject.SetActive(true);
        m_curBall.transform.position = new Vector3(tt.position.x, tt.position.y, tt.position.z);
        m_curBall.Fire(dir);
    }

    public void StartGame()
    {
        AudioPlayer.Instance.PlayBgm();
        OpenIngameUI();

        m_player = new Player();
        m_player.Init(true);
        m_npc = new Player();
        m_npc.Init(false);

        m_curBall.gameObject.SetActive(false);
        GetTrails(0, m_curBall);

        //launcher init pos,rot
        m_playerLauncher.gameObject.SetActive(false);
        m_npcLauncher.gameObject.SetActive(false);

        //VS UI

        IsGameStart = true;
        //start round1
        //StartRound(1);
        IsPlayerRound = false;
        SwitchTurn();
    }

    public void GameOver(bool isWin)
    {
        AudioPlayer.Instance.StopBgm();
        OpenGameOverUI(isWin);
    }

    //public void StartRound(int roundIndex)
    //{
    //    m_curRound = roundIndex;
    //    //round ui

        
    //}

    public void SwitchTurn()
    {
        //IsPlayerRound = false;
        

        IsPlayerRound = !IsPlayerRound;
        m_curBall.gameObject.SetActive(false);
        GetTrails(0, m_curBall);

        //player ui
        m_ingameUI.ShowTurn(true, IsPlayerRound);
        

        //if(!IsPlayerRound)
        //{
        //    StartRound(m_curRound++);
        //}
    }

    public void DelayTurn()
    {
        m_ingameUI.ShowTurn(false, IsPlayerRound);
        if (IsPlayerRound)
        {
            m_player.OnSwitchTurn();
            m_ingameUI.RefreshHP();
            m_ingameUI.RefreshBuff();
            if (m_player.m_hp <= 0)
            {
                return;
            }

            if (m_player.m_isIce)
            {
                m_player.m_isIce = false;
                //SwitchTurn();
                m_ingameUI.ShowMiss(true);
            }
            else
            {
                RandomMap();
                m_playerLauncher.gameObject.SetActive(true);
                m_npcLauncher.gameObject.SetActive(false);
                m_playerLauncher.DoRot();
            }
        }
        else
        {
            m_npc.OnSwitchTurn();
            m_ingameUI.RefreshHP();
            m_ingameUI.RefreshBuff();
            if (m_npc.m_hp <= 0)
            {
                return;
            }

            if (m_npc.m_isIce)
            {
                m_npc.m_isIce = false;
                m_ingameUI.ShowMiss(true);
            }
            else
            {
                RandomMap();
                m_playerLauncher.gameObject.SetActive(false);
                m_npcLauncher.gameObject.SetActive(true);
                m_npcLauncher.DoRot();
                m_npcLauncher.StartAI();
            }
        }
    }

    public void DelayMiss()
    {
        m_ingameUI.ShowMiss(false);
        SwitchTurn();
    }

    public void RandomMap()
    {
        if(m_curMap)
        {
            GameObject.Destroy(m_curMap.gameObject);
            m_curMap = null;
        }

        List<Map> mapPool = new List<Map>();
        foreach(Map map in m_mapPrefabs)
        {
            for(int i = 0; i < map.Weight; ++i)
            {
                mapPool.Add(map);
            }
        }
        Random.seed = System.DateTime.Now.Millisecond;
        int random = Random.RandomRange(0, mapPool.Count - 1);
        m_curMap = GameObject.Instantiate(mapPool[random],GameRoot.transform);
        m_curMap.transform.localPosition = Vector3.zero;
        int rot = IsPlayerRound ? 0 : 180;
        m_curMap.transform.localRotation = Quaternion.Euler(0, 0, rot);
        m_curMap.Init();
    }

    public void ClickFire()
    {
        if(!IsGameStart || !IsPlayerRound)
        {
            return;
        }

        if(m_curBall.m_isOnfire || m_curBall.gameObject.activeSelf == true)
        {
            return;
        }

        m_playerLauncher.OnFire();
    }

    public void GetTrails(int level,Ball ball)
    {
        for(int i = 0; i < m_trails.Count; ++i)
        {
            if(i == level)
            {
                m_trails[i].gameObject.SetActive(true);
                m_trails[i].transform.localPosition = ball.transform.localPosition;
                m_trails[i].FollowObj = ball.transform;
            }
            else
            {
                m_trails[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region ui
    public Ingame m_ingameUI;
    public GameObject m_mainUI;
    public GameObject m_vsUI;
    public GameOver m_gameoverUI;
    public SkillSelect m_skillSelectUI;

    public void CloseAllUI()
    {
        m_ingameUI.gameObject.SetActive(false);
        m_mainUI.gameObject.SetActive(false);
        m_vsUI.gameObject.SetActive(false);
        m_gameoverUI.gameObject.SetActive(false);
        m_skillSelectUI.gameObject.SetActive(false);
    }

    public void OnClickStartButton()
    {
        //OpenVSUI();
        OpenSkillSelectUI();
    }

    public void OnVsTweenEnd()
    {
        m_vsUI.gameObject.SetActive(false);
        StartGame();
    }

    public void OpenMainUI()
    {
        CloseAllUI();
        m_mainUI.gameObject.SetActive(true);
    }

    public void OpenIngameUI()
    {
        CloseAllUI();
        m_ingameUI.gameObject.SetActive(true);
        m_ingameUI.Init();
    }

    public void OpenVSUI()
    {
        CloseAllUI();
        m_vsUI.gameObject.SetActive(true);
    }

    public void OpenGameOverUI(bool isWin)
    {
        CloseAllUI();
        m_gameoverUI.gameObject.SetActive(true);
        m_gameoverUI.Init(isWin);
    }

    public void OpenSkillSelectUI()
    {
        CloseAllUI();
        m_skillSelectUI.gameObject.SetActive(true);
        m_skillSelectUI.Init();
    }

    public void OnTurnTweenEnd()
    {
        DelayTurn();
    }

    public void OnMissTweenEnd()
    {
        DelayMiss();
    }

    public void OnPerfectTweenEnd()
    {
        m_ingameUI.ShowPerfect(false);
    }

    public void OnClickGameOver()
    {
        Init();
    }
    #endregion
}
