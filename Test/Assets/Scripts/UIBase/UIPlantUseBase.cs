using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIPlantUseBase : UILogicBase
{
    protected GameButton e_CloseBtn;//UI-PlantUse/BgTransparent/Bg/e_CloseBtn
    protected GameButton e_ConfirmBtn;//UI-PlantUse/BgTransparent/Bg/e_ConfirmBtn
    protected GameLabel e_TxtName;//UI-PlantUse/BgTransparent/Bg/e_TxtName
    protected GameLabel e_TxtUse;//UI-PlantUse/BgTransparent/Bg/e_TxtUse
    protected GameImage e_ItemImg;//UI-PlantUse/BgTransparent/Bg/Kuang/e_ItemImg
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn = MakeUIComponent<GameButton>("BgTransparent/Bg/e_CloseBtn");
        e_ConfirmBtn = MakeUIComponent<GameButton>("BgTransparent/Bg/e_ConfirmBtn");
        e_TxtName = MakeUIComponent<GameLabel>("BgTransparent/Bg/e_TxtName");
        e_TxtUse = MakeUIComponent<GameLabel>("BgTransparent/Bg/e_TxtUse");
        e_ItemImg = MakeUIComponent<GameImage>("BgTransparent/Bg/Kuang/e_ItemImg");
    }
}