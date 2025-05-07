using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 常规UI的基类
/// </summary>
public class UILogicBase
{
    public string resPath;
    public GameObject gameObject;
    public RectTransform rectTransform;
    protected Canvas canvas;
    private Coroutine co;
    /// <summary>
    /// 初始化
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
    /// UI每次显示
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
    /// UI每次隐藏
    /// </summary>
    public virtual void OnHide()
    {

    }
    /// <summary>
    /// UI销毁
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
