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
    private RectOffset Offset;
    public int ChildCount { get; private set; }
    public UITileLoop(GameObject grid, ScrollRect rect)
    {
        gameObject = grid;
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
        LayoutElement layout = temp.GetComponent<LayoutElement>();
        if (layout == null)
        {
            Debug.LogError("Template 上无 LayoutElement组件");
            return;
        }
        itemHeight += layout.preferredHeight;

        // 初始化 Item 池
        canVisibleMaxCount = Mathf.CeilToInt(scrollRect.viewport.rect.height / itemHeight) + 1;
        // 监听滚动事件
        scrollRect.onValueChanged.AddListener(OnScroll);
        
        firstVisibleIndex = 0;
        lastVisibleIndex = canVisibleMaxCount - 1;

        Offset = content.GetComponent<LayoutGroup>().padding;
    }

    void OnScroll(Vector2 scrollPos)
    {
        // 计算当前应该显示的 Item 范围
        float contentY = content.anchoredPosition.y;
        int firstIndex = Mathf.FloorToInt(contentY / itemHeight);
        int lastIndex = firstIndex + canVisibleMaxCount - 1;

        if (firstIndex < 0 || lastIndex >= ChildCount || firstVisibleIndex == firstIndex) return;

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
        SetOffset(firstIndex, lastIndex);
    }
    public void Ensuresize(int count)
    {
        float contentY = content.anchoredPosition.y;

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
        for (int i = firstVisibleIndex; i < count && i <= lastVisibleIndex; i++)
        {
            OnUpdateItem?.Invoke(i, _children[i]);
        }
        SetOffset(firstVisibleIndex, lastVisibleIndex);
    }
    private void SetOffset(int startIndex,int endIndex)
    {
        int diffTop = Mathf.FloorToInt(startIndex * itemHeight);
        int diffBottom = Mathf.FloorToInt((ChildCount - 1 - endIndex) * itemHeight);
        Offset.top = diffTop;
        Offset.bottom = diffBottom >= 0 ? diffBottom : 0;
    }
}
