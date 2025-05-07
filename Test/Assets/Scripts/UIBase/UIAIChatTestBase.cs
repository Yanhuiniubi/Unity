using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIAIChatTestBase : UILogicBase
{
    protected GameLabel e_TxtTitle;//UI-AIChatTest/ImgTitle/e_TxtTitle
    protected ScrollRect e_ScrollView;//UI-AIChatTest/e_Scroll View
    protected GameButton e_CloseBtn;//UI-AIChatTest/e_CloseBtn
    protected TMP_InputField e_InputField;//UI-AIChatTest/Input/e_InputField
    protected GameButton e_SendBtn;//UI-AIChatTest/e_SendBtn
    protected GameImage e_WaitBg;//UI-AIChatTest/e_WaitBg
    public override void OnInit()
    {
        base.OnInit();
        e_TxtTitle = MakeUIComponent<GameLabel>("ImgTitle/e_TxtTitle");
        e_ScrollView = GetUIComponent<ScrollRect>("e_Scroll View");
        e_CloseBtn = MakeUIComponent<GameButton>("e_CloseBtn");
        e_InputField = GetUIComponent<TMP_InputField>("Input/e_InputField");
        e_SendBtn = MakeUIComponent<GameButton>("e_SendBtn");
        e_WaitBg = MakeUIComponent<GameImage>("e_WaitBg");
    }
}
public class UIAIChatTestContentBase : GameUIComponent
{
    protected GameImage e_Icon;//UI-AIChatTest/e_Scroll View/Grid/Template/ChatRootLeft/MaxWidth/e_Icon
    protected GameImage e_ImgBG;//UI-AIChatTest/e_Scroll View/Grid/Template/ChatRootLeft/MaxWidth/Content/e_ImgBG
    protected GameLabel e_TxtContent;//UI-AIChatTest/e_Scroll View/Grid/Template/ChatRootLeft/MaxWidth/Content/e_ImgBG/e_TxtContent
    protected override void OnInit()
    {
        base.OnInit();
        e_Icon = MakeUIComponent<GameImage>("ChatRootLeft/MaxWidth/e_Icon");
        e_ImgBG = MakeUIComponent<GameImage>("ChatRootLeft/MaxWidth/Content/e_ImgBG");
        e_TxtContent = MakeUIComponent<GameLabel>("ChatRootLeft/MaxWidth/Content/e_ImgBG/e_TxtContent");
    }
}