using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameButton : GameUIComponent
{
    private Button _btn;
    protected override void OnInit()
    {
        base.OnInit();
        _btn = GetComponent<Button>();
    }
    public void AddClickEvent(UnityAction OnClick)
    {
        _btn.onClick.AddListener(OnClick);
    }
}
