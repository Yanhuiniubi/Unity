using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMod
{
    public readonly UIMod Inst;
    private Queue<UILogicBase> _uiPanelQueue;
    private UIMod()
    {
        Inst = this;
    }
    public void ShowUI<T, U>(string name,U param) where T : UILogicBase , new()
    {
        GameObject root = Resources.Load<GameObject>(name);
        T uibase = new T();
        uibase.root = root;
        uibase.OnShow<U>(param);
        _uiPanelQueue.Enqueue(uibase);
    }
    public void HideUI()
    {
        if (_uiPanelQueue.Count > 0)
        {
            UILogicBase uiBase = _uiPanelQueue.Dequeue();
            uiBase.OnHide();
        }
    }
}
