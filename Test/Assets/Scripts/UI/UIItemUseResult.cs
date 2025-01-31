using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class GarbageUseResultInfo
{
    public bool IsSuccess;
    public int DustbinType;
    public TableItemMain Cfg;
}
public class ShopItemBuyResultInfo
{
    public bool IsSuccess;
    public int Count;
    public TableItemMain Cfg;
}

[UIBind(UIDef.UI_ITEMUSERESULT)]
public class UIItemUseResult : UILogicBase
{
    private Image _bg;
    private Image _itemIcon;
    private TextMeshProUGUI _txtResult;
    private TextMeshProUGUI _txtDesc;
    private Button _btnSure;

    private GarbageUseResultInfo _garbageInfo;
    private ShopItemBuyResultInfo _shopItemInfo;
    public override void OnHide()
    {
        base.OnHide();
        _garbageInfo = null;
        _shopItemInfo = null;
    }

    public override void OnInit()
    {
        base.OnInit();
        _bg = GetUIComponentInchildren<Image>("BgTransparent/Bg");
        _itemIcon = GetUIComponentInchildren<Image>("BgTransparent/Bg/ItemIcon");
        _txtResult = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/TxtResult");
        _txtDesc = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/TxtDesc");
        _btnSure = GetUIComponentInchildren<Button>("BgTransparent/Bg/BtnSure");
        _btnSure.onClick.AddListener(HideUI);
    }
    private TableItemMain _item;
    public override void OnShow(object param)
    {
        base.OnShow(param);
        if (param is GarbageUseResultInfo)
            _garbageInfo = param as GarbageUseResultInfo;
        else if (param is ShopItemBuyResultInfo)
            _shopItemInfo = param as ShopItemBuyResultInfo;
        else
            _item = param as TableItemMain;
        SetView();
    }
    private void SetView()
    {
        if (_garbageInfo != null)
        {
            string garbage = "";
            switch ((eGarbageType)_garbageInfo.DustbinType)
            {
                case eGarbageType.Kehuishou:
                    garbage = "可回收垃圾！".ParseColorText("FFFFFF");
                    break;
                case eGarbageType.Youhai:
                    garbage = "有害垃圾！".ParseColorText("FFFFFF");
                    break;
                case eGarbageType.Chuyu:
                    garbage = "厨余垃圾！".ParseColorText("FFFFFF");
                    break;
                case eGarbageType.Qita:
                    garbage = "其他垃圾！".ParseColorText("FFFFFF");
                    break;
                default:
                    break;
            }
            if (_garbageInfo.IsSuccess)
            {
                _txtResult.text = "恭喜！垃圾分类正确".ParseColorText("FF0059");
                _txtDesc.text = $"{_garbageInfo.Cfg.Name.ParseColorText("FF0059")}属于{garbage}";
                _bg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("28button_green.png");
            }
            else
            {
                _txtResult.text = "哦不！垃圾分类错误".ParseColorText("FFFFFF");
                _txtDesc.text = $"{_garbageInfo.Cfg.Name.ParseColorText("FFFFFF")}不属于{garbage}";
                _bg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("30button_red.png");
            }
            _itemIcon.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(_garbageInfo.Cfg.IconPath);
        }
        else if (_shopItemInfo != null)
        {
            if (_shopItemInfo.IsSuccess)
            {
                _txtResult.text = "恭喜！购买成功".ParseColorText("FF0059");
                _txtDesc.text = $"{_shopItemInfo.Cfg.Name.ParseColorText("FF0059")} * {_shopItemInfo.Count}";
                _bg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("28button_green.png");
            }
            else
            {
                _txtResult.text = "哦不！购买失败".ParseColorText("FFFFFF");
                _txtDesc.text = $"资金不足以购买{_shopItemInfo.Cfg.Name} * {_shopItemInfo.Count}";
                _bg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("30button_red.png");
            }
            _itemIcon.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(_shopItemInfo.Cfg.IconPath);
        }
        else//种植
        {
            _txtResult.text = "恭喜！种植成功".ParseColorText("FF0059");
            _txtDesc.text = $"{_item.Name.ParseColorText("FF0059")} * {1}";
            _bg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("28button_green.png");
            _itemIcon.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(_item.IconPath);
        }
    }
    private void HideUI()
    {
        UIMod.Inst.HideUI();
    }
}
