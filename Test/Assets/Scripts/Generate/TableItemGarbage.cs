using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class TableItemGarbage
{
    public string ID;
    public string Name;
    public string IconPath;
    public string PrefabPath;
    public int Type;
    public string Desc;
}
public class TableItemGarbageMod
{
    private static string csvFilePath = Path.Combine(Application.streamingAssetsPath, "ItemGarbage.csv");
    private static Dictionary<string,TableItemGarbage> dic = new Dictionary<string,TableItemGarbage>();
    private static TableItemGarbage[] array;
    public static TableItemGarbage[] Array
    {
        get
        {
            if (array == null)
                Init();
            return array;
        }
    }
    private static void Init()
    {
        var csvContent = File.ReadAllText(csvFilePath);
        string[] records = csvContent.Split('\n');
        array = new TableItemGarbage[records.Length - 2];
        for (int i = 2;i < records.Length;i++)
        {
            TableItemGarbage cfg = new TableItemGarbage();
            var values = records[i].Split(',');
            for (int j = 0;j < values.Length;j++)
                values[j] = values[j].Replace("\"", "");
            cfg.ID = values[0];
            cfg.Name = values[1];
            cfg.IconPath = values[2];
            cfg.PrefabPath = values[3];
            cfg.Type = int.Parse(values[4]);
            cfg.Desc = values[5];
            dic.Add(cfg.ID, cfg);
            array[i - 2] = cfg;
        }
    }
    public static TableItemGarbage Get(string id)
    {
        if (dic.Count == 0)
            Init();
        if (dic.ContainsKey(id))
            return dic[id];
        return null;
    }
}
