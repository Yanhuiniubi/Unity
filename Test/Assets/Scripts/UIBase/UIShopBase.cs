using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIShopBase : UILogicBase
{
    protected GameButton e_CloseBtn;//UI-Shop/e_CloseBtn
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn = MakeUIComponent<GameButton>("e_CloseBtn");
    }
}
public class UIShopContentBase : UITemplateBase
{
    protected GameImage e_ItemImg;//UI-Shop/Scroll View/Grid/Template/e_ItemImg
    protected GameLabel e_ItemName;//UI-Shop/Scroll View/Grid/Template/e_ItemName
    protected GameButton e_DescBtn;//UI-Shop/Scroll View/Grid/Template/e_DescBtn
    protected GameLabel e_ItemCount;//UI-Shop/Scroll View/Grid/Template/HorizontalLayout/e_ItemCount
    public override void OnInit()
    {
        base.OnInit();
        e_ItemImg = MakeUIComponent<GameImage>("e_ItemImg");
        e_ItemName = MakeUIComponent<GameLabel>("e_ItemName");
        e_DescBtn = MakeUIComponent<GameButton>("e_DescBtn");
        e_ItemCount = MakeUIComponent<GameLabel>("HorizontalLayout/e_ItemCount");
    }
}
public class UIShopContentBase1 : UITemplateBase
{
    protected GameToggle e_Toggle;//UI-Shop/ScrollViewPage/GridPage/Template/e_Toggle
    protected GameLabel e_TxtPageName;//UI-Shop/ScrollViewPage/GridPage/Template/e_Toggle/e_TxtPageName
    public override void OnInit()
    {
        base.OnInit();
        e_Toggle = MakeUIComponent<GameToggle>("e_Toggle");
        e_TxtPageName = MakeUIComponent<GameLabel>("e_Toggle/e_TxtPageName");
    }
}