using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIItemUseResultBase : UILogicBase
{
    protected GameImage e_Bg;//UI-ItemUseResult/BgTransparent/e_Bg
    protected GameImage e_ItemIcon;//UI-ItemUseResult/BgTransparent/e_Bg/e_ItemIcon
    protected GameLabel e_TxtResult;//UI-ItemUseResult/BgTransparent/e_Bg/e_TxtResult
    protected GameLabel e_TxtDesc;//UI-ItemUseResult/BgTransparent/e_Bg/e_TxtDesc
    protected GameButton e_BtnSure;//UI-ItemUseResult/BgTransparent/e_Bg/e_BtnSure
    public override void OnInit()
    {
        base.OnInit();
        e_Bg = MakeUIComponent<GameImage>("BgTransparent/e_Bg");
        e_ItemIcon = MakeUIComponent<GameImage>("BgTransparent/e_Bg/e_ItemIcon");
        e_TxtResult = MakeUIComponent<GameLabel>("BgTransparent/e_Bg/e_TxtResult");
        e_TxtDesc = MakeUIComponent<GameLabel>("BgTransparent/e_Bg/e_TxtDesc");
        e_BtnSure = MakeUIComponent<GameButton>("BgTransparent/e_Bg/e_BtnSure");
    }
}