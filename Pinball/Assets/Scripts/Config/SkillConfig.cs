using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SkillConfigItem
{
    //技能ID 字符型
    public string ID;
    //技能类型 整数
    public SkillType Type;
    //等级 整数
    public int Level;
    //游戏内概率 浮点数 0.25=25%
    public float Percent;
    //回合数 整数
    public int Round;
    //数值 浮点数或整数 0.25=25%(百分比技能) 1=1(固定增加技能)
    public float Value;
    //数值类型 整数 1:固定值，例如攻击增加 2:百分比值，例如吸血、恢复
    public int ValueType;
    //图标名 字符型
    public string IconName;
    //特效名 字符型
    public string EffectName;
    //技能名称 字符型
    public string Name;
    //技能描述 字符型
    public string Describe;
    //获得权重 整数
    public int Weight;
}

public class SkillConfig
{
    public static Dictionary<string, SkillConfigItem> Configs;
    public static void LoadConfig()
    {
        string json = Resources.Load<TextAsset>("SkillConfig").text;
        Configs = JsonConvert.DeserializeObject<Dictionary<string, SkillConfigItem>>(json);
        foreach(string key in Configs.Keys)
        {
            Configs[key].ID = key;
        }
    }
}
