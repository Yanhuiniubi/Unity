using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有Tempplate的基类
/// </summary>
//public class UITemplateBase
//{
//    public GameObject gameObject;
//    /// <summary>
//    /// 初始化
//    /// </summary>
//    public virtual void OnInit()
//    {

//    }
//    protected T GetUIComponent<T>(string path = "") where T : Component
//    {
//        if (string.IsNullOrEmpty(path))
//            return gameObject.GetComponent<T>();
//        return gameObject.transform.Find(path).GetComponent<T>();
//    }

//    protected T MakeUIComponent<T>(string path) where T : GameUIComponent, new()
//    {
//        T com = new T();
//        com.Init(path, gameObject);
//        return com;
//    }
//}
