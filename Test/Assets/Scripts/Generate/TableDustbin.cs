using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class TableDustbin
{
    public string ID;
    public int Type;
    public string Desc;
    public string Name;
}
public class TableDustbinMod
{
    private static string csvFilePath = Path.Combine(Application.streamingAssetsPath, "Dustbin.csv");
    private static Dictionary<string,TableDustbin> dic = new Dictionary<string,TableDustbin>();
    private static TableDustbin[] array;
    public static TableDustbin[] Array
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
        array = new TableDustbin[records.Length - 2];
        for (int i = 2;i < records.Length;i++)
        {
            TableDustbin cfg = new TableDustbin();
            var values = records[i].Split(',');
            for (int j = 0;j < values.Length;j++)
                values[j] = values[j].Replace("\"", "");
            cfg.ID = values[0];
            cfg.Type = int.Parse(values[1]);
            cfg.Desc = values[2];
            cfg.Name = values[3];
            dic.Add(cfg.ID, cfg);
            array[i - 2] = cfg;
        }
    }
    public static TableDustbin Get(string id)
    {
        if (dic.Count == 0)
            Init();
        if (dic.ContainsKey(id))
            return dic[id];
        return null;
    }
}
