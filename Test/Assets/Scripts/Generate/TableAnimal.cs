using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class TableAnimal
{
    public string ID;
    public string Desc;
}
public class TableAnimalMod
{
    private static string csvFilePath = Path.Combine(Application.streamingAssetsPath, "Animal.csv");
    private static Dictionary<string,TableAnimal> dic = new Dictionary<string,TableAnimal>();
    private static TableAnimal[] array;
    public static TableAnimal[] Array
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
        array = new TableAnimal[records.Length - 2];
        for (int i = 2;i < records.Length;i++)
        {
            TableAnimal cfg = new TableAnimal();
            var values = records[i].Split(',');
            for (int j = 0;j < values.Length;j++)
                values[j] = values[j].Replace("\"", "");
            cfg.ID = values[0];
            cfg.Desc = values[1];
            dic.Add(cfg.ID, cfg);
            array[i - 2] = cfg;
        }
    }
    public static TableAnimal Get(string id)
    {
        if (dic.Count == 0)
            Init();
        if (dic.ContainsKey(id))
            return dic[id];
        return null;
    }
}
