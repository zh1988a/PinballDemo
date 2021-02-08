using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

//地图数据
public class MapConfigItem
{
    //索引
    public int Index;
    //概率
    public float Probability;
}

public class MapConfig
{
    public static Dictionary<int, MapConfigItem> Configs;
    public static void LoadConfig()
    {
        string json = Resources.Load<TextAsset>("MapConfig").text;
        Configs = JsonConvert.DeserializeObject<Dictionary<int, MapConfigItem>>(json);
        foreach (int key in Configs.Keys)
        {
            Configs[key].Index = key;
        }
    }
}

