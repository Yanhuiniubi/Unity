using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameUIComponent
{
    public GameObject gameObject;
    public RectTransform rectTransform;
    public string Name => gameObject.name;
    public bool Visible
    {
        get
        {
            return gameObject.activeSelf;
        }
        set
        {
            gameObject.SetActive(value);
        }
    }
    public void Init(string path,GameObject root)
    {
        gameObject = root.transform.Find(path).gameObject;
        rectTransform = gameObject.GetComponent<RectTransform>();
        OnInit();
    }
    public void Init(GameObject go)
    {
        gameObject = go;
        rectTransform = gameObject.GetComponent<RectTransform>();
        OnInit();
    }
    protected virtual void OnInit()
    {

    }
    public T GetUIComponent<T>() where T : Component
    {
        T com = gameObject.GetComponent<T>();
        return com;
    }
    protected T MakeUIComponent<T>(string path) where T : GameUIComponent, new()
    {
        T com = new T();
        com.Init(path, gameObject);
        return com;
    }
    public void AddClickPointerHandle(Action<PointerEventData> OnClick)
    {
        UIClickPointer handle = UIClickPointer.Get(gameObject);
        handle.OnClick = OnClick;
    }
}
