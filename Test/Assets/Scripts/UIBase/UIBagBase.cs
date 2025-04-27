using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIBagBase : UILogicBase
{
    protected TextMeshProUGUI e_TxtTitle;//UI-Bag/ImgTitle/e_TxtTitle
    protected Button e_CloseBtn;//UI-Bag/e_CloseBtn
    protected ToggleGroup e_GridPage;//UI-Bag/ScrollViewPage/e_GridPage
    public override void OnInit()
    {
        base.OnInit();
        e_TxtTitle = GetUIComponentInchildren<TextMeshProUGUI>("ImgTitle/e_TxtTitle");
        e_CloseBtn = GetUIComponentInchildren<Button>("e_CloseBtn");
        e_GridPage = GetUIComponentInchildren<ToggleGroup>("ScrollViewPage/e_GridPage");
    }
}
public class UIBagContentBase : UITemplateBase
{
    protected Image e_ItemImg;//UI-Bag/Scroll View/Grid/Template/e_ItemImg
    protected TextMeshProUGUI e_ItemCount;//UI-Bag/Scroll View/Grid/Template/e_ItemCount
    protected TextMeshProUGUI e_ItemName;//UI-Bag/Scroll View/Grid/Template/e_ItemName
    protected Button e_DescBtn;//UI-Bag/Scroll View/Grid/Template/e_DescBtn
    public override void OnInit()
    {
        base.OnInit();
        e_ItemImg = GetUIComponentInchildren<Image>("e_ItemImg");
        e_ItemCount = GetUIComponentInchildren<TextMeshProUGUI>("e_ItemCount");
        e_ItemName = GetUIComponentInchildren<TextMeshProUGUI>("e_ItemName");
        e_DescBtn = GetUIComponentInchildren<Button>("e_DescBtn");
    }
}
public class UIBagContentBase1 : UITemplateBase
{
    protected Toggle e_Toggle;//UI-Bag/ScrollViewPage/e_GridPage/Template/e_Toggle
    protected TextMeshProUGUI e_TxtPageName;//UI-Bag/ScrollViewPage/e_GridPage/Template/e_Toggle/e_TxtPageName
    public override void OnInit()
    {
        base.OnInit();
        e_Toggle = GetUIComponentInchildren<Toggle>("e_Toggle");
        e_TxtPageName = GetUIComponentInchildren<TextMeshProUGUI>("e_Toggle/e_TxtPageName");
    }
}