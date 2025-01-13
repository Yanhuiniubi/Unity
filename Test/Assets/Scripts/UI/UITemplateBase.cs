using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有Tempplate的基类
/// </summary>
public class UITemplateBase
{
    public GameObject gameObject;
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void OnInit()
    {

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
