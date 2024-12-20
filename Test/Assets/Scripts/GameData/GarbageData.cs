using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
/// <summary>
/// ��������
/// </summary>
public enum eGarbageType
{
    Kehuishou = 0,
    Youhai = 1,
    Chuyu = 2,
    Qita = 3
}
/// <summary>
/// ��������
/// </summary>
public class GarbageInfo
{
    public GameObject GameObject;
    public TableItemGarbage Cfg;
}
/// <summary>
/// �������ݹ���
/// </summary>
public class GarbageData
{
    public static readonly GarbageData Inst = new GarbageData();
    private Dictionary<GameObject, GarbageInfo> _garDic_Obj;
    private Dictionary<eGarbageType, List<TableItemGarbage>> _garDic_Type;
    private Dictionary<string, Queue<GarbageInfo>> _poor;//�����
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
    /// ����������Ϣ���������
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GarbageInfo InitNewGarbageInfo(string id)
    {
        GarbageInfo info = new GarbageInfo();
        var cfg = TableItemGarbageMod.Get(id);
        info.Cfg = cfg;
        GameObject obj = GameObject.Instantiate<GameObject>(ResData.Inst.GetResByPath<GameObject>(cfg.PrefabPath),
            GameMod.Inst.GarbageRoot);
        info.GameObject = obj;
        obj.transform.position = new Vector3(0,5,0);
        return info;
    }
    /// <summary>
    /// �Ӷ�����л�ȡһ������
    /// </summary>
    /// <param name="type"></param>
    private GarbageInfo GetGarbageFromPoor(eGarbageType type)
    {
        var list = _garDic_Type[type];
        int index = Random.Range(0, list.Count);
        string id = list[index].ID;
        if (_poor.TryGetValue(id, out Queue<GarbageInfo> queue))
        {
            if (queue.Count > 0)
                return queue.Dequeue();
            else
                return InitNewGarbageInfo(id);
        }
        else
        {
            _poor[id] = new Queue<GarbageInfo>();
            return InitNewGarbageInfo(id);
        }
    }
    /// <summary>
    /// ���������������
    /// </summary>
    /// <param name="type"></param>
    private void ReturnGarbageToPoor(string id, GarbageInfo info)
    {
        var queue = _poor[id];
        queue.Enqueue(info);
    }
    /// <summary>
    /// ��������������������
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    public void GenerateGarbage(eGarbageType type)
    {
        GarbageInfo info = GetGarbageFromPoor(type);
        _garDic_Obj.Add(info.GameObject, info);
        info.GameObject.SetActive(true);
    }
    /// <summary>
    /// ��������Obj��������
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
            Debug.LogError($"_garDic_Obj ������{obj.name}");
        }
    }
    /// <summary>
    /// �������������ȡ��Ӧ�����ñ�����
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public TableItemGarbage GetGarbageCfgByObj(GameObject obj)
    {
        if (_garDic_Obj.TryGetValue(obj,out var val))
        {
            return val.Cfg;
        }
        Debug.LogError("GarbageData GetGarbageCfgByObj �����objδ������");
        return null;
    }
}
