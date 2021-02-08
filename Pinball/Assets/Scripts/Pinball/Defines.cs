using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines: MonoBehaviour
{
    private static Defines _instance;
    public static Defines Instance
    {
        get
        {
            return _instance;
        }
    }
    

    private void Awake()
    {
        _instance = this;
    }

    public float BaseDamage = 1;
    public float BaseHp;
}

public class LayerName
{
    public const string TopWall = "TopWall";
    public const string BottomWall = "BottomWall";
    public const string LeftWall = "LeftWall";
    public const string RightWall = "RightWall";
    public const string Obstacle = "Obstacle";
    public const string Skill = "Skill";
    public const string Bullet = "Bullet";
}

public enum SkillType
{
    Default = 0,

    Attack = 1,
    Crit = 2,
    Vampire = 3,
    Ice = 4,
    Gas = 5,
    Protect = 6,
    Heal = 7,
    Life = 8
}
