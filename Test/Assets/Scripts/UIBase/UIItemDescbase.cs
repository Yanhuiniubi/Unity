using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIItemDescBase : UILogicBase
{
    protected GameLabel e_TxtTitle;//UI-ItemDesc/ImgTitle/e_TxtTitle
    protected GameLabel e_TxtDesc;//UI-ItemDesc/e_TxtDesc
    protected GameButton e_CloseBtn;//UI-ItemDesc/e_CloseBtn
    public override void OnInit()
    {
        base.OnInit();
        e_TxtTitle = MakeUIComponent<GameLabel>("ImgTitle/e_TxtTitle");
        e_TxtDesc = MakeUIComponent<GameLabel>("e_TxtDesc");
        e_CloseBtn = MakeUIComponent<GameButton>("e_CloseBtn");
    }
}