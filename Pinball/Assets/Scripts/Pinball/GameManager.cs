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

    public Launcher m_playerLauncher;
    public Launcher m_npcLauncher;

    public Transform m_playerFirePos;
    public Transform m_npcFirePos;

    public GameObject GameRoot;

    public List<Map> m_mapPrefabs;

    private void Awake()
    {
        _instance = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Init();

        //start ui

        StartGame();
    }

    public void Init()
    {
        IsGameStart = false;

        SkillConfig.LoadConfig();
        //MapConfig.LoadConfig();

        SimplePool.Preload(ballPrefab, 1);

        m_curBall = SimplePool.Spawn(ballPrefab,Vector3.zero,Quaternion.Euler(0,0,0)).GetComponent<Ball>();
        m_curBall.transform.parent = GameRoot.transform;
        m_curBall.gameObject.SetActive(false);

        m_playerLauncher.gameObject.SetActive(false);
        m_npcLauncher.gameObject.SetActive(false);

        m_ingameUI.Init();
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

        SwitchTurn();
    }

    public void DoRelease(Ball ball)
    {
        //if()
        SwitchTurn();
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
        m_player = new Player();
        m_player.Init(true);
        m_npc = new Player();
        m_npc.Init(false);

        m_curBall.gameObject.SetActive(false);

        //launcher init pos,rot
        m_playerLauncher.gameObject.SetActive(false);
        m_npcLauncher.gameObject.SetActive(false);

        //VS UI

        IsGameStart = true;
        //start round1
        StartRound(1);
        IsPlayerRound = false;
        SwitchTurn();
    }

    public void GameOver(bool isWin)
    {
        
    }

    public void StartRound(int roundIndex)
    {
        m_curRound = roundIndex;
        //round ui

        
    }

    public void SwitchTurn()
    {
        //IsPlayerRound = false;
        

        IsPlayerRound = !IsPlayerRound;

        //player ui

        m_curBall.gameObject.SetActive(false);
        if(IsPlayerRound)
        {
            m_player.OnSwitchTurn();
            m_ingameUI.RefreshHP();
            if (m_player.m_hp <=0)
            {
                return;
            }

            if (m_player.m_isIce)
            {
                m_player.m_isIce = false;
                SwitchTurn();
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
            if (m_npc.m_hp <= 0)
            {
                return;
            }

            if (m_npc.m_isIce)
            {
                m_npc.m_isIce = false;
                SwitchTurn();
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

        if(!IsPlayerRound)
        {
            StartRound(m_curRound++);
        }
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

        m_playerLauncher.OnFire();
    }
    #endregion

    #region ingame ui
    public Ingame m_ingameUI;
    #endregion

    #region out game ui

    #endregion
}
