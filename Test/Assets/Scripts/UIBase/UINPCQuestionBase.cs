using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UINPCQuestionBase : UILogicBase
{
    protected Button e_CloseBtn;//UI-NPCQuestion/Bg_transparent/Bg/e_CloseBtn
    protected TextMeshProUGUI e_Content;//UI-NPCQuestion/Bg_transparent/Bg/e_Content
    protected Button e_OKBtn;//UI-NPCQuestion/e_OKBtn
    protected ToggleGroup e_ToggleGroup;//UI-NPCQuestion/e_ToggleGroup
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn = GetUIComponent<Button>("Bg_transparent/Bg/e_CloseBtn");
        e_Content = GetUIComponent<TextMeshProUGUI>("Bg_transparent/Bg/e_Content");
        e_OKBtn = GetUIComponent<Button>("e_OKBtn");
        e_ToggleGroup = GetUIComponent<ToggleGroup>("e_ToggleGroup");
    }
}
public class UINPCQuestionContentBase : UITemplateBase
{
    protected Toggle e_Toggle;//UI-NPCQuestion/ScrollViewPage/GridPage/Template/e_Toggle
    protected TextMeshProUGUI e_Text;//UI-NPCQuestion/ScrollViewPage/GridPage/Template/e_Toggle/Background/e_Text
    public override void OnInit()
    {
        base.OnInit();
        e_Toggle = GetUIComponent<Toggle>("e_Toggle");
        e_Text = GetUIComponent<TextMeshProUGUI>("e_Toggle/Background/e_Text");
    }
}