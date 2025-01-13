using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����Tempplate�Ļ���
/// </summary>
public class UITemplateBase
{
    public GameObject gameObject;
    public HashSet<string> ResAddresses = new HashSet<string>();
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public virtual void OnInit()
    {

    }
    /// <summary>
    /// ��Դ�ͷ�
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
