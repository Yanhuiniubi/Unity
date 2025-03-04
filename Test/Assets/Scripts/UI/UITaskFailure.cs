using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIBind(UIDef.UI_TASKFAILURE)]
public class UITaskFailure : UILogicBase
{
    private Button _btnSure;
    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnInit()
    {
        base.OnInit();
        _btnSure = GetUIComponentInchildren<Button>("BgTransparent/Bg/BtnSure");
        _btnSure.onClick.AddListener(HideUI);
    }
    private void HideUI()
    {
        UIMod.Inst.HideUI();
    }
    
}
