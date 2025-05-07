using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����UI�Ļ���
/// </summary>
public class UILogicBase
{
    public string resPath;
    public GameObject gameObject;
    public RectTransform rectTransform;
    protected Canvas canvas;
    private Coroutine co;
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public virtual void OnInit()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvas = gameObject.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = GameMod.Inst.UICamera;
        }
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
            UIMod.Inst.DeleteUI(resPath);
        }
    }
    /// <summary>
    /// UIÿ������
    /// </summary>
    public virtual void OnHide()
    {

    }
    /// <summary>
    /// UI����
    /// </summary>
    public virtual void OnDispose()
    {

    }
    protected T GetUIComponent<T>(string path = "") where T : Component
    {
        if (string.IsNullOrEmpty(path))
            return gameObject.GetComponent<T>();
        return gameObject.transform.Find(path).GetComponent<T>();
    }

    protected T MakeUIComponent<T>(string path) where T : GameUIComponent , new()
    {
        T com = new T();
        com.Init(path, gameObject);
        return com;
    }

    public void HideThisPanel()
    {
        co = GameMod.Inst.StartCoroutine(DelayDeleteObj());
        gameObject.SetActive(false);
        OnHide();
    }
    IEnumerator DelayDeleteObj()
    {
        yield return new WaitForSeconds(5f);
        if (!gameObject.activeSelf)
        {
            OnDispose();
            UIMod.Inst.DeleteUI(resPath);
            GameObject.Destroy(gameObject);
        }
    }
}
