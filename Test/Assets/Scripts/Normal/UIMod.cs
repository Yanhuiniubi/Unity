using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            cacheUI.root.SetActive(true);
            cacheUI.OnShow(param);
            _uiPanelQueue.Push(cacheUI);
            return;
        }

        GameObject res = Resources.Load<GameObject>(path);
        GameObject root = GameObject.Instantiate<GameObject>(res, parent == null ? GameMod.Inst.UIRoot : parent);
        T uibase = new T();
        uibase.root = root;
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
    private Dictionary<string, UILogicBase> cache3DUIDic_show = new Dictionary<string, UILogicBase>();
    private Dictionary<string, UILogicBase> cache3DUIDic_hide = new Dictionary<string, UILogicBase>();
    public void Show3DUI<T>(string path,bool isRepeat,object param = null, Transform parent = null) where T : UILogicBase, new()
    {
        if (cache3DUIDic_show.ContainsKey(path))
        {
            if (!isRepeat)
            {
                Debug.LogError("请勿重复show同一个UI");
                return;
            }
        }
        if (cache3DUIDic_hide.ContainsKey(path))
        {
            UILogicBase cacheUI = cache3DUIDic_hide[path];
            cacheUI.root.SetActive(true);
            cacheUI.OnShow(param);
            cache3DUIDic_hide.Remove(path);
            cache3DUIDic_show.Add(path, cacheUI);
            return;
        }

        GameObject res = Resources.Load<GameObject>(path);
        GameObject root = GameObject.Instantiate<GameObject>(res, parent == null ? GameMod.Inst.UI3DRoot : parent);
        T uibase = new T();
        uibase.root = root;
        uibase.resPath = path;
        uibase.OnInit();
        uibase.OnShow(param);
        cache3DUIDic_show.Add(path, uibase);
    }
    public void HideUI(string path)
    {
        if (cache3DUIDic_show.ContainsKey(path))
        {
            UILogicBase uiBase = cache3DUIDic_show[path];
            uiBase.HideThisPanel();
            cache3DUIDic_show.Remove(path);
            cache3DUIDic_hide.Add(path, uiBase);
        }
    }
}
