using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIItemUseBase : UILogicBase
{
    protected Button e_CloseBtn;//UI-ItemUse/BgTransparent/Bg/e_CloseBtn
    protected Button e_ConfirmBtn;//UI-ItemUse/BgTransparent/Bg/e_ConfirmBtn
    protected TextMeshProUGUI e_TxtName;//UI-ItemUse/BgTransparent/Bg/e_TxtName
    protected TextMeshProUGUI e_TxtUse;//UI-ItemUse/BgTransparent/Bg/e_TxtUse
    protected Image e_ItemImg;//UI-ItemUse/BgTransparent/Bg/Kuang/e_ItemImg
    protected Slider e_Slider;//UI-ItemUse/BgTransparent/Bg/e_Slider
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn = GetUIComponentInchildren<Button>("BgTransparent/Bg/e_CloseBtn");
        e_ConfirmBtn = GetUIComponentInchildren<Button>("BgTransparent/Bg/e_ConfirmBtn");
        e_TxtName = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/e_TxtName");
        e_TxtUse = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/e_TxtUse");
        e_ItemImg = GetUIComponentInchildren<Image>("BgTransparent/Bg/Kuang/e_ItemImg");
        e_Slider = GetUIComponentInchildren<Slider>("BgTransparent/Bg/e_Slider");
    }
}