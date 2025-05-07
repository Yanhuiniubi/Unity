using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UITaskFailureBase : UILogicBase
{
    protected GameLabel e_TxtDesc;//UI-TaskFailure/BgTransparent/Bg/e_TxtDesc
    protected GameButton e_BtnSure;//UI-TaskFailure/BgTransparent/Bg/e_BtnSure
    public override void OnInit()
    {
        base.OnInit();
        e_TxtDesc = MakeUIComponent<GameLabel>("BgTransparent/Bg/e_TxtDesc");
        e_BtnSure = MakeUIComponent<GameButton>("BgTransparent/Bg/e_BtnSure");
    }
}