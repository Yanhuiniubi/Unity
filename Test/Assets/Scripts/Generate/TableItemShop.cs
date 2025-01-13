using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class TableItemShop
{
    public int ID;
    public string ItemID;
    public int Price;
    public int Page;
}
public class TableItemShopMod
{
    private static string csvFilePath = Path.Combine(Application.streamingAssetsPath, "ItemShop.csv");
    private static Dictionary<int,TableItemShop> dic = new Dictionary<int,TableItemShop>();
    private static TableItemShop[] array;
    public static TableItemShop[] Array
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
        array = new TableItemShop[records.Length - 2];
        for (int i = 2;i < records.Length;i++)
        {
            TableItemShop cfg = new TableItemShop();
            var values = records[i].Split(',');
            for (int j = 0;j < values.Length;j++)
                values[j] = values[j].Replace("\"", "");
            cfg.ID = int.Parse(values[0]);
            cfg.ItemID = values[1];
            cfg.Price = int.Parse(values[2]);
            cfg.Page = int.Parse(values[3]);
            dic.Add(cfg.ID, cfg);
            array[i - 2] = cfg;
        }
    }
    public static TableItemShop Get(int id)
    {
        if (dic.Count == 0)
            Init();
        if (dic.ContainsKey(id))
            return dic[id];
        return null;
    }
}
