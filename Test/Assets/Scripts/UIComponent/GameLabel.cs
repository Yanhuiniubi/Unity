using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLabel : GameUIComponent
{
    private TextMeshProUGUI _label;
    protected override void OnInit()
    {
        base.OnInit();
        _label = GetComponent<TextMeshProUGUI>();
    }
    public string Text
    {
        get
        {
            return _label.text;
        }
        set
        {
            _label.text = value;
        }
    }
}
