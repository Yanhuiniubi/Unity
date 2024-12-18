using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDesc : UILogicBase
{
    private TextMeshProUGUI _txtTitle;
    private TextMeshProUGUI _txtDesc;
    private Button _closeBtn;

    private TableItemGarbage _cfg;
    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnInit()
    {
        base.OnInit();
        _txtTitle = GetUIComponentInchildren<TextMeshProUGUI>("ImgTitle/TxtTitle");
        _txtDesc = GetUIComponentInchildren<TextMeshProUGUI>("TxtDesc");
        _closeBtn = GetUIComponentInchildren<Button>("CloseBtn");
        _closeBtn.onClick.AddListener(CloseUI);
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        _cfg = param as TableItemGarbage;
        if (_cfg == null)
        {
            Debug.LogError("UIItemDesc OnShow 传入参数 不为TableItemGarbage");
            return;
        }
        _txtTitle.text = _cfg.Name;
        _txtDesc.text = _cfg.Desc;
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
}
