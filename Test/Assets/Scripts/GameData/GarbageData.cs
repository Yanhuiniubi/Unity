using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public TableItemMain Cfg;
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
    public int GarbageCount => _garDic_Obj.Count;
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
    private GarbageInfo InitNewGarbageInfo(string id, Vector3 pos)
    {
        GarbageInfo info = new GarbageInfo();
        var cfg = TableItemMainMod.Get(id);
        info.Cfg = cfg;
        GameObject obj = GameObject.Instantiate<GameObject>(ResData.Inst.GetResByAddressPermanent<GameObject>(cfg.PrefabPath)
           ,GameMod.Inst.GarbageRoot);
        info.GameObject = obj;
        obj.transform.position = new Vector3(pos.x,pos.y + 3,pos.z);
        return info;
    }
    /// <summary>
    /// �Ӷ�����л�ȡһ������
    /// </summary>
    /// <param name="type"></param>
    private GarbageInfo GetGarbageFromPoor(string id, Vector3 pos)
    {
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
    public void GenerateGarbage(eGarbageType type,Vector3 pos)
    {
        var list = _garDic_Type[type];
        int index = Random.Range(0, list.Count);
        string id = list[index].ItemID;
        GarbageInfo info = GetGarbageFromPoor(id, pos);
        _garDic_Obj.Add(info.GameObject, info);
        info.GameObject.SetActive(true);
        PlayerEvent.OnGarbageCntChanged?.Invoke();
    }
    /// <summary>
    /// ��������id��������
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    public void GenerateGarbage(string id, Vector3 pos)
    {
        GarbageInfo info = GetGarbageFromPoor(id, pos);
        _garDic_Obj.Add(info.GameObject, info);
        info.GameObject.SetActive(true);
        PlayerEvent.OnGarbageCntChanged?.Invoke();
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
            BagData.Inst.AddItem(info.Cfg, 1);
            PlayerEvent.OnGarbageCntChanged?.Invoke();
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
    public TableItemMain GetGarbageCfgByObj(GameObject obj)
    {
        if (_garDic_Obj.TryGetValue(obj,out var val))
        {
            return val.Cfg;
        }
        Debug.LogError("GarbageData GetGarbageCfgByObj �����objδ������");
        return null;
    }
}
