using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����Tempplate�Ļ���
/// </summary>
public class UITemplateBase
{
    public GameObject gameObject;
    /// <summary>
    /// ��ʼ��
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
