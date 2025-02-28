using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class TableMainTask
{
    public int ID;
    public int TaskType;
    public string Param1;
    public int Count;
    public string Desc;
}
public class TableMainTaskMod
{
    private static string csvFilePath = Path.Combine(Application.streamingAssetsPath, "MainTask.csv");
    private static Dictionary<int,TableMainTask> dic = new Dictionary<int,TableMainTask>();
    private static TableMainTask[] array;
    public static TableMainTask[] Array
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
        array = new TableMainTask[records.Length - 2];
        for (int i = 2;i < records.Length;i++)
        {
            TableMainTask cfg = new TableMainTask();
            var values = records[i].Split(',');
            for (int j = 0;j < values.Length;j++)
                values[j] = values[j].Replace("\"", "");
            cfg.ID = int.Parse(values[0]);
            cfg.TaskType = int.Parse(values[1]);
            cfg.Param1 = values[2];
            cfg.Count = int.Parse(values[3]);
            cfg.Desc = values[4];
            dic.Add(cfg.ID, cfg);
            array[i - 2] = cfg;
        }
    }
    public static TableMainTask Get(int id)
    {
        if (dic.Count == 0)
            Init();
        if (dic.ContainsKey(id))
            return dic[id];
        return null;
    }
}
