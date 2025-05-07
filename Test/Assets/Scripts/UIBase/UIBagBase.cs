using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIBagBase : UILogicBase
{
    protected GameLabel e_TxtTitle;//UI-Bag/ImgTitle/e_TxtTitle
    protected GameButton e_CloseBtn;//UI-Bag/e_CloseBtn
    protected ToggleGroup e_GridPage;//UI-Bag/ScrollViewPage/e_GridPage
    public override void OnInit()
    {
        base.OnInit();
        e_TxtTitle = MakeUIComponent<GameLabel>("ImgTitle/e_TxtTitle");
        e_CloseBtn = MakeUIComponent<GameButton>("e_CloseBtn");
        e_GridPage = GetUIComponent<ToggleGroup>("ScrollViewPage/e_GridPage");
    }
}
public class UIBagContentBase : GameUIComponent
{
    protected GameImage e_ItemImg;//UI-Bag/Scroll View/Grid/Template/e_ItemImg
    protected GameLabel e_ItemCount;//UI-Bag/Scroll View/Grid/Template/e_ItemCount
    protected GameLabel e_ItemName;//UI-Bag/Scroll View/Grid/Template/e_ItemName
    protected GameButton e_DescBtn;//UI-Bag/Scroll View/Grid/Template/e_DescBtn
    protected override void OnInit()
    {
        base.OnInit();
        e_ItemImg = MakeUIComponent<GameImage>("e_ItemImg");
        e_ItemCount = MakeUIComponent<GameLabel>("e_ItemCount");
        e_ItemName = MakeUIComponent<GameLabel>("e_ItemName");
        e_DescBtn = MakeUIComponent<GameButton>("e_DescBtn");
    }
}
public class UIBagContentBase1 : GameUIComponent
{
    protected GameToggle e_Toggle;//UI-Bag/ScrollViewPage/e_GridPage/Template/e_Toggle
    protected GameLabel e_TxtPageName;//UI-Bag/ScrollViewPage/e_GridPage/Template/e_Toggle/e_TxtPageName
    protected override void OnInit()
    {
        base.OnInit();
        e_Toggle = MakeUIComponent<GameToggle>("e_Toggle");
        e_TxtPageName = MakeUIComponent<GameLabel>("e_Toggle/e_TxtPageName");
    }
}