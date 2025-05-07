using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIItemUseResultBase : UILogicBase
{
    protected Image e_Bg;//UI-ItemUseResult/BgTransparent/e_Bg
    protected Image e_ItemIcon;//UI-ItemUseResult/BgTransparent/e_Bg/e_ItemIcon
    protected TextMeshProUGUI e_TxtResult;//UI-ItemUseResult/BgTransparent/e_Bg/e_TxtResult
    protected TextMeshProUGUI e_TxtDesc;//UI-ItemUseResult/BgTransparent/e_Bg/e_TxtDesc
    protected Button e_BtnSure;//UI-ItemUseResult/BgTransparent/e_Bg/e_BtnSure
    public override void OnInit()
    {
        base.OnInit();
        e_Bg = GetUIComponent<Image>("BgTransparent/e_Bg");
        e_ItemIcon = GetUIComponent<Image>("BgTransparent/e_Bg/e_ItemIcon");
        e_TxtResult = GetUIComponent<TextMeshProUGUI>("BgTransparent/e_Bg/e_TxtResult");
        e_TxtDesc = GetUIComponent<TextMeshProUGUI>("BgTransparent/e_Bg/e_TxtDesc");
        e_BtnSure = GetUIComponent<Button>("BgTransparent/e_Bg/e_BtnSure");
    }
}