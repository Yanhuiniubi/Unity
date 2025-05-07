using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[UIBind(UIDef.UI_TASKFAILURE)]
public class UITaskFailure : UITaskFailureBase
{
    public override void OnHide()
    {
        base.OnHide();
    }
    public override void OnShow(object param)
    {
        base.OnShow(param);
        if (param != null)
            e_TxtDesc.Text = param.ToString();
    }
    public override void OnInit()
    {
        base.OnInit();
        e_BtnSure.AddClickEvent(HideUI);
    }
    private void HideUI()
    {
        UIMod.Inst.HideUI();
    }
    
}
