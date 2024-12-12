using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMod
{
    public static readonly UIMod Inst = new UIMod();
    private Stack<UILogicBase> _uiPanelQueue = new Stack<UILogicBase>();
    private Dictionary<string, UILogicBase> cacheUIDic = new Dictionary<string, UILogicBase>();
    public void ShowUI<T>(string path,object param = null,Transform parent = null) where T : UILogicBase , new()
    {
        if (_uiPanelQueue.Count > 0 && _uiPanelQueue.Peek().resPath.Equals(path))
        {
            Debug.LogError("请勿重复show同一个UI");
            return;
        }
        if (cacheUIDic.ContainsKey(path))
        {
            UILogicBase cacheUI = cacheUIDic[path];
            cacheUI.gameObject.SetActive(true);
            cacheUI.OnShow(param);
            _uiPanelQueue.Push(cacheUI);
            return;
        }

        GameObject res = Resources.Load<GameObject>(path);
        GameObject root = GameObject.Instantiate<GameObject>(res, parent == null ? GameMod.Inst.UIRoot : parent);
        T uibase = new T();
        uibase.gameObject = root;
        uibase.resPath = path;
        uibase.OnInit();
        uibase.OnShow(param);
        _uiPanelQueue.Push(uibase);
    }
    public void HideUI()
    {
        if (_uiPanelQueue.Count > 0)
        {
            UILogicBase uiBase = _uiPanelQueue.Pop();
            uiBase.HideThisPanel();
            cacheUIDic.Add(uiBase.resPath, uiBase);
        }
    }
    public void DeleteUI(string path)
    {
        if (cacheUIDic.ContainsKey(path))
            cacheUIDic.Remove(path);
    }
    private Dictionary<string, UI3DLogicBase> cache3DUIDic_show = new Dictionary<string, UI3DLogicBase>();
    private Dictionary<string, UI3DLogicBase> cache3DUIDic_hide = new Dictionary<string, UI3DLogicBase>();
    public void Show3DUI<T>(string path,string name,object param = null, Transform parent = null) where T : UI3DLogicBase, new()
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

        GameObject res = Resources.Load<GameObject>(path);
        GameObject root = GameObject.Instantiate<GameObject>(res, parent == null ? GameMod.Inst.UI3DRoot : parent);
        T uibase = new T();
        uibase.gameObject = root;
        uibase.resPath = path;
        uibase.name = name;
        uibase.OnInit();
        uibase.OnShow(param);
        cache3DUIDic_show.Add(key, uibase);
    }
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
    public void Delete3DUI(string key)
    {
        if (cache3DUIDic_hide.ContainsKey(key))
            cache3DUIDic_hide.Remove(key);
    }
    public bool IsActiveUI3D(string key)
    {
        return cache3DUIDic_show.ContainsKey(key);
    }
}
