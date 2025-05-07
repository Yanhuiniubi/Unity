using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIAIChatBase : UILogicBase
{
    protected TextMeshProUGUI e_TxtTitle;//UI-AIChat/ImgTitle/e_TxtTitle
    protected ScrollRect e_ScrollView;//UI-AIChat/e_Scroll View
    protected Button e_CloseBtn;//UI-AIChat/e_CloseBtn
    protected TMP_InputField e_InputField;//UI-AIChat/Input/e_InputField
    protected Button e_SendBtn;//UI-AIChat/e_SendBtn
    protected Image e_WaitBg;//UI-AIChat/e_WaitBg
    public override void OnInit()
    {
        base.OnInit();
        e_TxtTitle = GetUIComponent<TextMeshProUGUI>("ImgTitle/e_TxtTitle");
        e_ScrollView = GetUIComponent<ScrollRect>("e_Scroll View");
        e_CloseBtn = GetUIComponent<Button>("e_CloseBtn");
        e_InputField = GetUIComponent<TMP_InputField>("Input/e_InputField");
        e_SendBtn = GetUIComponent<Button>("e_SendBtn");
        e_WaitBg = GetUIComponent<Image>("e_WaitBg");
    }
}
public class UIAIChatContentBase : UITemplateBase
{
    protected Image e_Icon;//UI-AIChat/e_Scroll View/Grid/Template/ChatRootLeft/MaxWidth/e_Icon
    protected Image e_ImgBG;//UI-AIChat/e_Scroll View/Grid/Template/ChatRootLeft/MaxWidth/Content/e_ImgBG
    protected TextMeshProUGUI e_TxtContent;//UI-AIChat/e_Scroll View/Grid/Template/ChatRootLeft/MaxWidth/Content/e_ImgBG/e_TxtContent
    public override void OnInit()
    {
        base.OnInit();
        e_Icon = GetUIComponent<Image>("ChatRootLeft/MaxWidth/e_Icon");
        e_ImgBG = GetUIComponent<Image>("ChatRootLeft/MaxWidth/Content/e_ImgBG");
        e_TxtContent = GetUIComponent<TextMeshProUGUI>("ChatRootLeft/MaxWidth/Content/e_ImgBG/e_TxtContent");
    }
}