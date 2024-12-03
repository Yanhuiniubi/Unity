using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class TableRole
{
    public int ID;
    public string Name;
}
public class TableRoleMod
{
    private static string csvFilePath = "D:/Unity/proj/Unity/Test/Assets/StreamingAssets/Role.csv";
    private static Dictionary<int,TableRole> dic = new Dictionary<int,TableRole>();
    private static void Init()
    {
        var csvContent = File.ReadAllText(csvFilePath);
        string[] records = csvContent.Split('\n');
        for (int i = 2;i < records.Length;i++)
        {
            TableRole cfg = new TableRole();
            var values = records[i].Split(',');
            for (int j = 0;j < values.Length;j++)
                values[j] = values[j].Replace("\"", "");
            //write init code in here
            

            dic.Add(cfg.ID, cfg);
        }
    }
    public static TableRole Get(int id)
    {
        if (dic.Count == 0)
            Init();
        if (dic.ContainsKey(id))
            return dic[id];
        return null;
    }
}
