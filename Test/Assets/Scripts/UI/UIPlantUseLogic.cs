using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UIBind(UIDef.UI_PLANTUSE)]
public class UIPlantUseLogic : UILogicBase
{
    private TextMeshProUGUI _txtName;
    private TextMeshProUGUI _txtUse;
    private Button _closeBtn;
    private Button _confirmBtn;
    private Image _imgItem;

    private TableItemMain _item;
    public override void OnInit()
    {
        base.OnInit();
        _txtName = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/TxtName");
        _txtUse = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/TxtUse");
        _closeBtn = GetUIComponentInchildren<Button>("BgTransparent/Bg/CloseBtn");
        _confirmBtn = GetUIComponentInchildren<Button>("BgTransparent/Bg/ConfirmBtn");
        _imgItem = GetUIComponentInchildren<Image>("BgTransparent/Bg/Kuang/ItemImg");
        _closeBtn.onClick.AddListener(CloseUI);
        _confirmBtn.onClick.AddListener(OnConfirmBtnClick);
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        _item = param as TableItemMain;
        SetView();
    }
    private void SetView()
    {
        _txtName.text = _item.Name;
        _imgItem.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(_item.IconPath);
        _txtUse.text = $"是否将{_item.Name.ParseColorText("000000")}种植在此？";
    }
    public override void OnHide()
    {
        base.OnHide();
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
    private void OnConfirmBtnClick()
    {
        BagData.Inst.UseItem(_item, 1);
    }
}
 