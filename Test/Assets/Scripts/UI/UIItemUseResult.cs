using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public TableItemShop Cfg;
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

    public override void OnShow(object param)
    {
        base.OnShow(param);
        if (param is GarbageUseResultInfo)
            _garbageInfo = param as GarbageUseResultInfo;
        else if (param is ShopItemBuyResultInfo)
            _shopItemInfo = param as ShopItemBuyResultInfo;
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
                    garbage = "�ɻ���������".ParseColorText("FFFFFF");
                    break;
                case eGarbageType.Youhai:
                    garbage = "�к�������".ParseColorText("FFFFFF");
                    break;
                case eGarbageType.Chuyu:
                    garbage = "����������".ParseColorText("FFFFFF");
                    break;
                case eGarbageType.Qita:
                    garbage = "����������".ParseColorText("FFFFFF");
                    break;
                default:
                    break;
            }
            if (_garbageInfo.IsSuccess)
            {
                _txtResult.text = "��ϲ������������ȷ".ParseColorText("FF0059");
                _txtDesc.text = $"{_garbageInfo.Cfg.Name.ParseColorText("FF0059")}����{garbage}";
                _bg.sprite = ResData.Inst.GetResByPath<Sprite>("Icon/28button_green");
            }
            else
            {
                _txtResult.text = "Ŷ���������������".ParseColorText("FFFFFF");
                _txtDesc.text = $"{_garbageInfo.Cfg.Name.ParseColorText("FFFFFF")}������{garbage}";
                _bg.sprite = ResData.Inst.GetResByPath<Sprite>("Icon/30button_red");
            }
            _itemIcon.sprite = ResData.Inst.GetResByPath<Sprite>(_garbageInfo.Cfg.IconPath);
        }
        else if (_shopItemInfo != null)
        {
            if (_shopItemInfo.IsSuccess)
            {
                _txtResult.text = "��ϲ������ɹ�".ParseColorText("FF0059");
                _txtDesc.text = $"{_shopItemInfo.Cfg.Name.ParseColorText("FF0059")} * {_shopItemInfo.Count}";
                _bg.sprite = ResData.Inst.GetResByPath<Sprite>("Icon/28button_green");

            }
            else
            {
                _txtResult.text = "Ŷ��������ʧ��".ParseColorText("FFFFFF");
                _txtDesc.text = $"�ʽ����Թ���{_shopItemInfo.Cfg.Name} * {_shopItemInfo.Count}";
                _bg.sprite = ResData.Inst.GetResByPath<Sprite>("Icon/30button_red");
            }
            _itemIcon.sprite = ResData.Inst.GetResByPath<Sprite>(_shopItemInfo.Cfg.IconPath);
        }
    }
    private void HideUI()
    {
        UIMod.Inst.HideUI();
    }
}
