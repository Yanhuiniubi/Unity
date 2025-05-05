using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UITileLoop<T> where T : UITemplateBase, new()
{
    public Action<int, T> OnUpdateItem;

    private GameObject gameObject;

    private ScrollRect scrollRect;
    private RectTransform content;
    private int canVisibleMaxCount;
    private float itemHeight;
    private List<T> _children;
    private int firstVisibleIndex;
    private int lastVisibleIndex;

    public int ChildCount { get; private set; }
    public UITileLoop(GameObject obj, ScrollRect rect)
    {
        gameObject = obj;
        content = gameObject.GetComponent<RectTransform>();
        scrollRect = rect;
        _children = new List<T>();
        Init();
    }
    private void Init()
    {
        Transform parent = gameObject.transform;
        RectTransform temp = parent.Find("Template") as RectTransform;
        if (temp == null)
        {
            Debug.LogError($"GameObject {parent.name} Dont have Template");
            return;
        }
        itemHeight = 230f;
        // 初始化 Item 池
        canVisibleMaxCount = Mathf.CeilToInt(scrollRect.viewport.rect.height / itemHeight) + 1;
        // 监听滚动事件
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    void OnScroll(Vector2 scrollPos)
    {
        // 计算当前应该显示的 Item 范围
        float contentY = content.anchoredPosition.y;
        int firstIndex = Mathf.FloorToInt(contentY / itemHeight);
        int lastIndex = firstIndex + canVisibleMaxCount - 1;

        if (firstIndex < 0 || lastIndex >= ChildCount) return;
        if (firstVisibleIndex < firstIndex)
        {
            Transform firstChild = content.GetChild(1);
            firstChild.SetAsLastSibling();
            if (lastIndex >= _children.Count)
            {
                GameObject go = firstChild.gameObject;
                go.SetActive(true);
                T template = new T();
                template.gameObject = go;
                template.OnInit();
                _children.Add(template);
            }
            OnUpdateItem?.Invoke(lastIndex, _children[lastIndex]);
        }
        else if (firstVisibleIndex > firstIndex)
        {
            Transform lastChild = content.GetChild(content.childCount - 1);
            lastChild.SetSiblingIndex(1);
            OnUpdateItem?.Invoke(firstIndex, _children[firstIndex]);
        }

        firstVisibleIndex = firstIndex;
        lastVisibleIndex = lastIndex;
    }
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
        if (curChildCount < count)
        {
            while (curChildCount < count && curChildCount < canVisibleMaxCount)
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
        else if (curChildCount > count)
        {
            while (curChildCount > count && curChildCount > canVisibleMaxCount)
            {
                GameObject.Destroy(parent.GetChild(curChildCount).gameObject);
                curChildCount--;
                _children.RemoveAt(_children.Count - 1);
            }
        }
        temp.gameObject.SetActive(false);
        ChildCount = count;
        for (int i = 0;i < count && i < canVisibleMaxCount;i++)
        {
            OnUpdateItem?.Invoke(i, _children[i]);
        }
    }
}
