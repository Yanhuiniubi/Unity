using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResData
{
    public static readonly ResData Inst = new ResData();
    private Dictionary<string, object> _resDic;
    private ResData()
    {
        _resDic = new Dictionary<string, object>();
    }
    public T GetResByPath<T>(string path) where T : Object
    {
        if (_resDic.TryGetValue(path,out var res))
        {
            return res as T;
        }
        else
        {
            res = Resources.Load<T>(path);
            _resDic.Add(path, res);
        }
        return res as T;
    }
}
