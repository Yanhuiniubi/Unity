using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 3DUI的基类
/// </summary>
public class UI3DLogicBase
{
    public string resPath;
    public string name;
    public GameObject gameObject;
    private Coroutine co;
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void OnInit()
    {

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

    protected T MakeUIComponent<T>(string path) where T : GameUIComponent, new()
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
            UIMod.Inst.Delete3DUI(resPath + name);
            GameObject.Destroy(gameObject);
        }
    }
}
