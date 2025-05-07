using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UINPCQuestionBase : UILogicBase
{
    protected GameButton e_CloseBtn;//UI-NPCQuestion/Bg_transparent/Bg/e_CloseBtn
    protected GameLabel e_Content;//UI-NPCQuestion/Bg_transparent/Bg/e_Content
    protected GameButton e_OKBtn;//UI-NPCQuestion/e_OKBtn
    protected ToggleGroup e_ToggleGroup;//UI-NPCQuestion/e_ToggleGroup
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn = MakeUIComponent<GameButton>("Bg_transparent/Bg/e_CloseBtn");
        e_Content = MakeUIComponent<GameLabel>("Bg_transparent/Bg/e_Content");
        e_OKBtn = MakeUIComponent<GameButton>("e_OKBtn");
        e_ToggleGroup = GetUIComponent<ToggleGroup>("e_ToggleGroup");
    }
}
public class UINPCQuestionContentBase : GameUIComponent
{
    protected GameToggle e_Toggle;//UI-NPCQuestion/ScrollViewPage/GridPage/Template/e_Toggle
    protected GameLabel e_Text;//UI-NPCQuestion/ScrollViewPage/GridPage/Template/e_Toggle/Background/e_Text
    protected override void OnInit()
    {
        base.OnInit();
        e_Toggle = MakeUIComponent<GameToggle>("e_Toggle");
        e_Text = MakeUIComponent<GameLabel>("e_Toggle/Background/e_Text");
    }
}