using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustbinData
{
    public static readonly DustbinData Inst = new DustbinData();
    private Dictionary<GameObject, TableDustbin> _dic;
    private DustbinData()
    {
        _dic = new Dictionary<GameObject, TableDustbin>();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        var arr = TableDustbinMod.Array;
        int length = arr.Length;
        for (int i = 0;i < length;i++)
        {
            GameObject obj = GameObject.Find(arr[i].Name);
            _dic.Add(obj, arr[i]);
        }
    }
    /// <summary>
    /// 根据游戏物体获取垃圾桶配置表数据
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public TableDustbin GetDustbinCfgByObj(GameObject obj)
    {
        if (_dic.TryGetValue(obj, out var val))
        {
            return val;
        }
        Debug.LogError("DustbinData GetDustbinCfgByObj 传入的obj未被缓存");
        return null;
    }
}
