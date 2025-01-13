using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有Tempplate的基类
/// </summary>
public class UITemplateBase
{
    public GameObject gameObject;
    public HashSet<string> ResAddresses = new HashSet<string>();
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void OnInit()
    {

    }
    /// <summary>
    /// 资源释放
    /// </summary>
    public virtual void Dispose()
    {
        ResData.Inst.ReleasAsset(ResAddresses);
    }
    protected T GetUIComponentInchildren<T>(string path) where T : Component
    {
        return gameObject.transform.Find(path).GetComponent<T>();
    }
    protected T GetUIComponent<T>() where T : Component
    {
        return gameObject.GetComponent<T>();
    }
}
