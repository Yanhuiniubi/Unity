using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UIBind(UIDef.UI_UIITEMDESC)]
public class UIItemDesc : UIItemDescBase
{
    private TableItemMain _cfg;
    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn.AddClickEvent(CloseUI);
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        _cfg = param as TableItemMain;
        if (_cfg == null)
        {
            Debug.LogError("UIItemDesc OnShow 传入参数 不为TableItemGarbage");
            return;
        }
        e_TxtTitle.Text = _cfg.Name;
        e_TxtDesc.Text = _cfg.Desc;
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
}
