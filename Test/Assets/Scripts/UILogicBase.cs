using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogicBase
{
    public GameObject root;
    public virtual void OnShow<T>(T param)
    {
        
    }
    public virtual void OnHide()
    {

    }
    protected U GetUIComponent<U>(string path) where U : Component
    {
        return root.transform.Find(path).GetComponent<U>();
    }
}
