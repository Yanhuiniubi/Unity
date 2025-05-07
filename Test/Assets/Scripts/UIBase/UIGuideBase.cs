using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIGuideBase : UILogicBase
{
    protected GameButton e_CloseBtn;//UI-Guide/e_CloseBtn
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn = MakeUIComponent<GameButton>("e_CloseBtn");
    }
}