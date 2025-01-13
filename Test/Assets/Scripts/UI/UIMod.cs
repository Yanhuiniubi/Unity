using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMod
{
    public static readonly UIMod Inst = new UIMod();
    private Stack<UILogicBase> _uiPanelStack = new Stack<UILogicBase>();
    private Dictionary<string, UILogicBase> cacheUIDic_hide = new Dictionary<string, UILogicBase>();
    private Dictionary<string, UILogicBase> cacheUIDic_show = new Dictionary<string, UILogicBase>();
    /// <summary>
    /// 常规显示UI，由Stack管理，后展示的先隐藏
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="param"></param>
    /// <param name="parent"></param>
    /// <param name="changeGameState"></param>
    public void ShowUI<T>(string path, object param = null, Transform parent = null, bool changeGameState = true) where T : UILogicBase, new()
    {
        if (cacheUIDic_show.ContainsKey(path))
        {
            Debug.LogError("请勿重复show同一个UI");
            return;
        }
        if (changeGameState)
            GameMod.Inst.SetGameState(eGameState.OpenUI);
        if (cacheUIDic_hide.ContainsKey(path))
        {
            UILogicBase cacheUI = cacheUIDic_hide[path];
            cacheUI.gameObject.SetActive(true);
            cacheUI.OnShow(param);
            _uiPanelStack.Push(cacheUI);
            cacheUIDic_show.Add(path, cacheUI);
            return;
        }
        T uibase = new T();
        GameObject res = ResData.Inst.GetResByAddressPermanent<GameObject>(path);
        GameObject root = GameObject.Instantiate<GameObject>(res, parent == null ? GameMod.Inst.UIRoot : parent);
        uibase.gameObject = root;
        uibase.resPath = path;
        _uiPanelStack.Push(uibase);

        uibase.OnInit();
        uibase.OnShow(param);
        cacheUIDic_show.Add(path, uibase);
    }
    /// <summary>
    /// 隐藏stack顶部的UI，与showui配对使用
    /// </summary>
    public void HideUI()
    {
        if (_uiPanelStack.Count > 0)
        {
            UILogicBase uiBase = _uiPanelStack.Pop();
            uiBase.HideThisPanel();
            cacheUIDic_hide.Add(uiBase.resPath, uiBase);
            cacheUIDic_show.Remove(uiBase.resPath);
        }
        if (_uiPanelStack.Count == 1)
            GameMod.Inst.SetGameState(eGameState.Normal);
    }
    /// <summary>
    /// 删除UI缓存
    /// </summary>
    /// <param name="path"></param>
    public void DeleteUI(string path)
    {
        if (cacheUIDic_hide.ContainsKey(path))
        {
            cacheUIDic_hide.Remove(path);
        }
    }
    private Dictionary<string, UI3DLogicBase> cache3DUIDic_show = new Dictionary<string, UI3DLogicBase>();
    private Dictionary<string, UI3DLogicBase> cache3DUIDic_hide = new Dictionary<string, UI3DLogicBase>();
    /// <summary>
    /// 展示3DUI，name是一个特殊的key，用于标识是否是同一个UI，可同时存在多个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="param"></param>
    /// <param name="parent"></param>
    public void Show3DUI<T>(string path, string name, object param = null, Transform parent = null) where T : UI3DLogicBase, new()
    {
        string key = path + name;
        if (cache3DUIDic_show.ContainsKey(key))
        {
            Debug.LogError("请勿重复show同一个UI");
            return;
        }
        if (cache3DUIDic_hide.ContainsKey(key))
        {
            UI3DLogicBase cacheUI = cache3DUIDic_hide[key];
            cacheUI.gameObject.SetActive(true);
            cacheUI.OnShow(param);
            cache3DUIDic_hide.Remove(key);
            cache3DUIDic_show.Add(key, cacheUI);
            return;
        }
        T uibase = new T();

        GameObject res = ResData.Inst.GetResByAddressPermanent<GameObject>(path);
        GameObject root = GameObject.Instantiate<GameObject>(res, parent == null ? GameMod.Inst.UI3DRoot : parent);
        uibase.gameObject = root;
        uibase.resPath = path;
        uibase.name = name;
        uibase.OnInit();
        uibase.OnShow(param);
        cache3DUIDic_show.Add(key, uibase);
    }
    /// <summary>
    /// 展示tips类UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="param"></param>
    /// <param name="parent"></param>
    public void ShowTipsUI<T>(string path, object param = null, Transform parent = null) where T : UILogicBase, new()
    {
        if (cacheUIDic_show.ContainsKey(path))
        {
            UILogicBase cacheUI = cacheUIDic_show[path];
            cacheUI.gameObject.SetActive(false);
            cacheUI.OnShow(param);
            cacheUI.gameObject.SetActive(true);
            return;
        }
        if (cacheUIDic_hide.ContainsKey(path))
        {
            UILogicBase cacheUI = cacheUIDic_hide[path];
            cacheUI.gameObject.SetActive(true);
            cacheUI.OnShow(param);
            cacheUIDic_show.Add(path, cacheUI);
            cacheUIDic_hide.Remove(path);
            return;
        }

        T uibase = new T();
        GameObject res = ResData.Inst.GetResByAddressPermanent<GameObject>(path);
        GameObject root = GameObject.Instantiate<GameObject>(res, parent == null ? GameMod.Inst.UITipsRoot : parent);
        uibase.gameObject = root;
        uibase.resPath = path;

        uibase.OnInit();
        uibase.OnShow(param);
        cacheUIDic_show.Add(path, uibase);
    }
    public void HideTips(string path)
    {
        UILogicBase uiBase = cacheUIDic_show[path];
        uiBase.HideThisPanel();
        cacheUIDic_hide.Add(uiBase.resPath, uiBase);
        cacheUIDic_show.Remove(uiBase.resPath);
    }
    /// <summary>
    /// 与show3Dui配对使用
    /// </summary>
    /// <param name="key"></param>
    public void HideUI(string key)
    {
        if (cache3DUIDic_show.ContainsKey(key))
        {
            UI3DLogicBase uiBase = cache3DUIDic_show[key];
            uiBase.HideThisPanel();
            cache3DUIDic_show.Remove(key);
            cache3DUIDic_hide.Add(key, uiBase);
        }
    }
    /// <summary>
    /// 删除3dui缓存
    /// </summary>
    /// <param name="key"></param>
    public void Delete3DUI(string key)
    {
        if (cache3DUIDic_hide.ContainsKey(key))
        {
            cache3DUIDic_hide.Remove(key);
        }
    }
    /// <summary>
    /// 3dUI是否正在显示
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsActiveUI3D(string key)
    {
        return cache3DUIDic_show.ContainsKey(key);
    }
    /// <summary>
    /// UI是否正在显示
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool IsActiveUI(string path)
    {
        return cacheUIDic_show.ContainsKey(path);
    }
}
