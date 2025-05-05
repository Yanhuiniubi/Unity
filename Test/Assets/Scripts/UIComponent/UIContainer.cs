using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContainer<T> where T : UITemplateBase, new()
{
    private List<T> _children = new List<T>();
    public List<T> Children => _children;
    public int ChildCount => _children.Count;
    public GameObject gameObject;
    public UIContainer(GameObject obj)
    {
        gameObject = obj;
    }
    public T GetChildrenByIndex(int idx)
    {
        if (idx >= _children.Count)
            return _children[_children.Count - 1];
        if (idx < 0)
            return _children[0];
        return _children[idx];
    }
    /// <summary>
    /// ÊµÀý»¯Template
    /// </summary>
    /// <param name="count"></param>
    public void Ensuresize(int count)
    {
        Transform parent = gameObject.transform;
        Transform temp = parent.Find("Template");
        if (temp == null)
        {
            Debug.LogError($"GameObject {parent.name} Dont have Template");
            return;
        }
        var curChildCount = ChildCount;
        if (curChildCount > count)
        {
            while(curChildCount != count)
            {
                GameObject.Destroy(parent.GetChild(curChildCount).gameObject);
                curChildCount--;
                _children.RemoveAt(_children.Count - 1);
            }
        }
        else
        {
            while (curChildCount != count)
            {
                GameObject go = GameObject.Instantiate<GameObject>(temp.gameObject, parent);
                go.SetActive(true);
                T template = new T();
                template.gameObject = go;
                template.OnInit();
                curChildCount++;
                _children.Add(template);
            }
        }
        temp.gameObject.SetActive(false);
    }
}
