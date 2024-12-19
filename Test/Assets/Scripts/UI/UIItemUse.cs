using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UIBind(UIDef.UI_UIITEMUSE)]
public class UIItemUse : UILogicBase
{
    private TextMeshProUGUI _txtName;
    private TextMeshProUGUI _txtUse;
    private Button _closeBtn;
    private Button _confirmBtn;
    private Slider _slider;
    private Image _imgItem;

    private ItemInfo _item;
    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnInit()
    {
        base.OnInit();
        _txtName = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/TxtName");
        _txtUse = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/TxtUse");
        _closeBtn = GetUIComponentInchildren<Button>("BgTransparent/Bg/CloseBtn");
        _confirmBtn = GetUIComponentInchildren<Button>("BgTransparent/Bg/ConfirmBtn");
        _slider = GetUIComponentInchildren<Slider>("BgTransparent/Bg/Slider");
        _imgItem = GetUIComponentInchildren<Image>("BgTransparent/Bg/Kuang/ItemImg");
        _closeBtn.onClick.AddListener(CloseUI);
        _confirmBtn.onClick.AddListener(OnConfirmBtnClick);
        _slider.onValueChanged.AddListener(OnCountChanged);
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        _item = param as ItemInfo;
        if (_item == null)
        {
            Debug.LogError("UIItemUse OnShow param is not ItemInfo");
            return;
        }
        SetView();
    }
    private void SetView()
    {
        var cfg = TableItemGarbageMod.Get(_item.ID);
        _txtName.text = cfg.Name;
        _slider.minValue = 0;
        _slider.maxValue = _item.Count;
        _slider.value = 0;
        _imgItem.sprite = ResData.Inst.GetResByPath<Sprite>(cfg.IconPath);
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
    private void OnConfirmBtnClick()
    {

    }
    private void OnCountChanged(float val)
    {
        _txtUse.text = $"Í¶Èë{val}¸ö";
    }
}
