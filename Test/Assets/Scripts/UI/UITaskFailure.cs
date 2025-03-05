using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[UIBind(UIDef.UI_TASKFAILURE)]
public class UITaskFailure : UILogicBase
{
    private Button _btnSure;
    private TextMeshProUGUI _content;
    public override void OnHide()
    {
        base.OnHide();
    }
    public override void OnShow(object param)
    {
        base.OnShow(param);
        if (param != null)
            _content.text = param.ToString();
    }
    public override void OnInit()
    {
        base.OnInit();
        _btnSure = GetUIComponentInchildren<Button>("BgTransparent/Bg/BtnSure");
        _content = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/TxtDesc");
        _btnSure.onClick.AddListener(HideUI);
    }
    private void HideUI()
    {
        UIMod.Inst.HideUI();
    }
    
}
