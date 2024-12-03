using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class TableAnimal
{
    public int ID;
    public string Name;
    public float Attack;
}
public class TableAnimalMod
{
    private static string csvFilePath = "D:/Unity/proj/Unity/Test/Assets/StreamingAssets/Animal.csv";
    private static Dictionary<int,TableAnimal> dic = new Dictionary<int,TableAnimal>();
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
            cfg.ID = int.Parse(values[0]);
            cfg.Name = values[1];
            cfg.Attack = float.Parse(values[2]);
            dic.Add(cfg.ID, cfg);
            array[i - 2] = cfg;
        }
    }
    public static TableAnimal Get(int id)
    {
        if (dic.Count == 0)
            Init();
        if (dic.ContainsKey(id))
            return dic[id];
        return null;
    }
}
