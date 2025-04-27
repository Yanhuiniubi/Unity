using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIShopBase : UILogicBase
{
    protected Button e_CloseBtn;//UI-Shop/e_CloseBtn
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn = GetUIComponentInchildren<Button>("e_CloseBtn");
    }
}
public class UIShopContentBase : UITemplateBase
{
    protected Image e_ItemImg;//UI-Shop/Scroll View/Grid/Template/e_ItemImg
    protected TextMeshProUGUI e_ItemName;//UI-Shop/Scroll View/Grid/Template/e_ItemName
    protected Button e_DescBtn;//UI-Shop/Scroll View/Grid/Template/e_DescBtn
    protected TextMeshProUGUI e_ItemCount;//UI-Shop/Scroll View/Grid/Template/HorizontalLayout/e_ItemCount
    public override void OnInit()
    {
        base.OnInit();
        e_ItemImg = GetUIComponentInchildren<Image>("e_ItemImg");
        e_ItemName = GetUIComponentInchildren<TextMeshProUGUI>("e_ItemName");
        e_DescBtn = GetUIComponentInchildren<Button>("e_DescBtn");
        e_ItemCount = GetUIComponentInchildren<TextMeshProUGUI>("HorizontalLayout/e_ItemCount");
    }
}
public class UIShopContentBase1 : UITemplateBase
{
    protected Toggle e_Toggle;//UI-Shop/ScrollViewPage/GridPage/Template/e_Toggle
    protected TextMeshProUGUI e_TxtPageName;//UI-Shop/ScrollViewPage/GridPage/Template/e_Toggle/e_TxtPageName
    public override void OnInit()
    {
        base.OnInit();
        e_Toggle = GetUIComponentInchildren<Toggle>("e_Toggle");
        e_TxtPageName = GetUIComponentInchildren<TextMeshProUGUI>("e_Toggle/e_TxtPageName");
    }
}