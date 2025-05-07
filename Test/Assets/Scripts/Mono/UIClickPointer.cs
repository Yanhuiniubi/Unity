using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickPointer : MonoBehaviour , IPointerClickHandler
{
    public Action<PointerEventData> OnClick;
    public static UIClickPointer Get(GameObject go)
    {
        UIClickPointer handle;
        if (go.TryGetComponent<UIClickPointer>(out handle))
            return handle;
        handle = go.AddComponent<UIClickPointer>();
        return handle;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(eventData);
    }
}
