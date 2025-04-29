using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIGuideBase : UILogicBase
{
    protected Button e_CloseBtn;//UI-Guide/e_CloseBtn
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn = GetUIComponentInchildren<Button>("e_CloseBtn");
    }
}