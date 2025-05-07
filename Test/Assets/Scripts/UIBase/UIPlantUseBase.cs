using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIPlantUseBase : UILogicBase
{
    protected Button e_CloseBtn;//UI-PlantUse/BgTransparent/Bg/e_CloseBtn
    protected Button e_ConfirmBtn;//UI-PlantUse/BgTransparent/Bg/e_ConfirmBtn
    protected TextMeshProUGUI e_TxtName;//UI-PlantUse/BgTransparent/Bg/e_TxtName
    protected TextMeshProUGUI e_TxtUse;//UI-PlantUse/BgTransparent/Bg/e_TxtUse
    protected Image e_ItemImg;//UI-PlantUse/BgTransparent/Bg/Kuang/e_ItemImg
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn = GetUIComponent<Button>("BgTransparent/Bg/e_CloseBtn");
        e_ConfirmBtn = GetUIComponent<Button>("BgTransparent/Bg/e_ConfirmBtn");
        e_TxtName = GetUIComponent<TextMeshProUGUI>("BgTransparent/Bg/e_TxtName");
        e_TxtUse = GetUIComponent<TextMeshProUGUI>("BgTransparent/Bg/e_TxtUse");
        e_ItemImg = GetUIComponent<Image>("BgTransparent/Bg/Kuang/e_ItemImg");
    }
}