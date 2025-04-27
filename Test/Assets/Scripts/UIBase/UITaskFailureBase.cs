using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UITaskFailureBase : UILogicBase
{
    protected TextMeshProUGUI e_TxtDesc;//UI-TaskFailure/BgTransparent/Bg/e_TxtDesc
    protected Button e_BtnSure;//UI-TaskFailure/BgTransparent/Bg/e_BtnSure
    public override void OnInit()
    {
        base.OnInit();
        e_TxtDesc = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/e_TxtDesc");
        e_BtnSure = GetUIComponentInchildren<Button>("BgTransparent/Bg/e_BtnSure");
    }
}