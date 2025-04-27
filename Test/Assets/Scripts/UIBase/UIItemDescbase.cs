using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIItemDescBase : UILogicBase
{
    protected TextMeshProUGUI e_TxtTitle;//UI-ItemDesc/ImgTitle/e_TxtTitle
    protected TextMeshProUGUI e_TxtDesc;//UI-ItemDesc/e_TxtDesc
    protected Button e_CloseBtn;//UI-ItemDesc/e_CloseBtn
    public override void OnInit()
    {
        base.OnInit();
        e_TxtTitle = GetUIComponentInchildren<TextMeshProUGUI>("ImgTitle/e_TxtTitle");
        e_TxtDesc = GetUIComponentInchildren<TextMeshProUGUI>("e_TxtDesc");
        e_CloseBtn = GetUIComponentInchildren<Button>("e_CloseBtn");
    }
}