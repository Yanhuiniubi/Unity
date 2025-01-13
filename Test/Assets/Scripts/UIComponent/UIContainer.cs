using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class UIContainer<T> where T : UITemplateBase, new()
{
    private List<T> _children = new List<T>();
    public List<T> Children => _children;
    public GameObject gameObject;
    public UIContainer(GameObject obj)
    {
        gameObject = obj;
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
        var curChildCount = parent.childCount - 1;
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
                GameObject go = GameObject.Instantiate<GameObject>(temp.gameObject, parent.transform);
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
