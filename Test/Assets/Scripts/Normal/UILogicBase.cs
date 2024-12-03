using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogicBase
{
    public string resPath;
    public GameObject root;
    private Coroutine co;
    /// <summary>
    /// ��ʼ�����
    /// </summary>
    public virtual void InitComponent()
    {

    }
    /// <summary>
    /// UIÿ����ʾ
    /// </summary>
    /// <param name="param"></param>
    public virtual void OnShow(object param)
    {
        if (co != null)
        {
            GameMod.Inst.StopCoroutine(co);
            co = null;
        }
        UIMod.Inst.DeleteUI(resPath);
    }
    /// <summary>
    /// UIÿ������
    /// </summary>
    public virtual void OnHide()
    {

    }
    protected T GetUIComponentInchildren<T>(string path) where T : Component
    {
        return root.transform.Find(path).GetComponent<T>();
    }
    protected T GetUIComponent<T>(string path) where T : Component
    {
        return root.GetComponent<T>();
    }

    public void HideThisPanel()
    {
        co = GameMod.Inst.StartCoroutine(DelayDeleteObj());
        root.SetActive(false);
        OnHide();
    }
    IEnumerator DelayDeleteObj()
    {
        yield return new WaitForSeconds(3f);
        if (!root.activeSelf)
        {
            UIMod.Inst.DeleteUI(resPath);
            GameObject.Destroy(root);
        }
    }
}
