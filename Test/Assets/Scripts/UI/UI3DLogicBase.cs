using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected T GetUIComponentInchildren<T>(string path) where T : Component
    {
        return gameObject.transform.Find(path).GetComponent<T>();
    }
    protected T GetUIComponent<T>() where T : Component
    {
        return gameObject.GetComponent<T>();
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
