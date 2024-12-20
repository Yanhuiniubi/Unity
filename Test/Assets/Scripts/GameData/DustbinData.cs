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
    /// ��ʼ������
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
    /// ������Ϸ�����ȡ����Ͱ���ñ�����
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public TableDustbin GetDustbinCfgByObj(GameObject obj)
    {
        if (_dic.TryGetValue(obj, out var val))
        {
            return val;
        }
        Debug.LogError("DustbinData GetDustbinCfgByObj �����objδ������");
        return null;
    }
}
