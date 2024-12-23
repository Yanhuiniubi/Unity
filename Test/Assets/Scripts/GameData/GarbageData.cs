using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 垃圾类型
/// </summary>
public enum eGarbageType
{
    Kehuishou = 0,
    Youhai = 1,
    Chuyu = 2,
    Qita = 3
}
/// <summary>
/// 垃圾数据
/// </summary>
public class GarbageInfo
{
    public GameObject GameObject;
    public TableItemGarbage Cfg;
}
/// <summary>
/// 垃圾数据管理
/// </summary>
public class GarbageData
{
    public static readonly GarbageData Inst = new GarbageData();
    private Dictionary<GameObject, GarbageInfo> _garDic_Obj;
    private Dictionary<eGarbageType, List<TableItemGarbage>> _garDic_Type;
    private Dictionary<string, Queue<GarbageInfo>> _poor;//对象池
    private TableItemGarbage[] _tableItems;
    private GarbageData()
    {
        _garDic_Obj = new Dictionary<GameObject, GarbageInfo>();

        _poor = new Dictionary<string, Queue<GarbageInfo>>();

        _tableItems = TableItemGarbageMod.Array;
        int length = _tableItems.Length;
        _garDic_Type = new Dictionary<eGarbageType, List<TableItemGarbage>>(length);
        for (int i = 0;i < length;i++)
        {
            eGarbageType type = (eGarbageType)_tableItems[i].Type;
            if (_garDic_Type.ContainsKey(type))
                _garDic_Type[type].Add(_tableItems[i]);
            else
                _garDic_Type[type] = new List<TableItemGarbage>() { _tableItems[i] };
        }
    }
    /// <summary>
    /// 新增垃圾信息到对象池中
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GarbageInfo InitNewGarbageInfo(string id, Vector3 pos)
    {
        GarbageInfo info = new GarbageInfo();
        var cfg = TableItemGarbageMod.Get(id);
        info.Cfg = cfg;
        GameObject obj = GameObject.Instantiate<GameObject>(ResData.Inst.GetResByPath<GameObject>(cfg.PrefabPath)
           ,GameMod.Inst.GarbageRoot);
        info.GameObject = obj;
        obj.transform.position = new Vector3(pos.x,pos.y + 3,pos.z);
        return info;
    }
    /// <summary>
    /// 从对象池中获取一个垃圾
    /// </summary>
    /// <param name="type"></param>
    private GarbageInfo GetGarbageFromPoor(eGarbageType type, Vector3 pos)
    {
        var list = _garDic_Type[type];
        int index = Random.Range(0, list.Count);
        string id = list[index].ID;
        if (_poor.TryGetValue(id, out Queue<GarbageInfo> queue))
        {
            if (queue.Count > 0)
                return queue.Dequeue();
            else
                return InitNewGarbageInfo(id, pos);
        }
        else
        {
            _poor[id] = new Queue<GarbageInfo>();
            return InitNewGarbageInfo(id, pos);
        }
    }
    /// <summary>
    /// 垃圾回收至对象池
    /// </summary>
    /// <param name="type"></param>
    private void ReturnGarbageToPoor(string id, GarbageInfo info)
    {
        var queue = _poor[id];
        queue.Enqueue(info);
    }
    /// <summary>
    /// 根据垃圾类型生成垃圾
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    public void GenerateGarbage(eGarbageType type,Vector3 pos)
    {
        GarbageInfo info = GetGarbageFromPoor(type, pos);
        _garDic_Obj.Add(info.GameObject, info);
        info.GameObject.SetActive(true);
    }
    /// <summary>
    /// 根据垃圾Obj回收垃圾
    /// </summary>
    /// <param name="type"></param>
    public void DeleteGarbage(GameObject obj)
    {
        if (_garDic_Obj.TryGetValue(obj,out var info))
        {
            obj.SetActive(false);
            ReturnGarbageToPoor(info.Cfg.ID, info);
            _garDic_Obj.Remove(obj);
            BagData.Inst.AddItem(info.Cfg.ID, 1);
        }
        else
        {
            Debug.LogError($"_garDic_Obj 不存在{obj.name}");
        }
    }
    /// <summary>
    /// 根据垃圾物体获取相应的配置表数据
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public TableItemGarbage GetGarbageCfgByObj(GameObject obj)
    {
        if (_garDic_Obj.TryGetValue(obj,out var val))
        {
            return val.Cfg;
        }
        Debug.LogError("GarbageData GetGarbageCfgByObj 传入的obj未被缓存");
        return null;
    }
}
