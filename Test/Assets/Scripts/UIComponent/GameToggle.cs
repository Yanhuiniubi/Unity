using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameToggle : GameUIComponent
{
    private Toggle _toggle;
    protected override void OnInit()
    {
        base.OnInit();
        _toggle = GetUIComponent<Toggle>();
    }
    public void AddValueChangeEvent(UnityAction<bool> OnValueChange)
    {
        _toggle.onValueChanged.AddListener(OnValueChange);
    }
    public bool IsOn
    {
        get
        {
            return _toggle.isOn;
        }
        set
        {
            _toggle.isOn = value;
        }
    }
    public ToggleGroup Group
    {
        get => _toggle.group;
        set
        {
            _toggle.group = value;
        }
    }
}