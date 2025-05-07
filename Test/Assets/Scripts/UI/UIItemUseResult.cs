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
    public TableItemMain Cfg;
}

[UIBind(UIDef.UI_ITEMUSERESULT)]
public class UIItemUseResult : UIItemUseResultBase
{
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
        e_BtnSure.AddClickEvent(HideUI);
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
                e_TxtResult.Text = "恭喜！垃圾分类正确".ParseColorText("FF0059");
                e_TxtDesc.Text = $"{_garbageInfo.Cfg.Name.ParseColorText("FF0059")}属于{garbage}";
                e_Bg.Sprite = "28button_green.png";
            }
            else
            {
                e_TxtResult.Text = "哦不！垃圾分类错误".ParseColorText("FFFFFF");
                e_TxtDesc.Text = $"{_garbageInfo.Cfg.Name.ParseColorText("FFFFFF")}不属于{garbage}";
                e_Bg.Sprite = "30button_red.png";
            }
            e_ItemIcon.Sprite = _garbageInfo.Cfg.IconPath;
        }
        else if (_shopItemInfo != null)
        {
            if (_shopItemInfo.IsSuccess)
            {
                e_TxtResult.Text = "恭喜！购买成功".ParseColorText("FF0059");
                e_TxtDesc.Text = $"{_shopItemInfo.Cfg.Name.ParseColorText("FF0059")} * {_shopItemInfo.Count}";
                e_Bg.Sprite = "28button_green.png";
            }
            else
            {
                e_TxtResult.Text = "哦不！购买失败".ParseColorText("FFFFFF");
                e_TxtDesc.Text = $"资金不足以购买{_shopItemInfo.Cfg.Name} * {_shopItemInfo.Count}";
                e_Bg.Sprite = "30button_red.png";
            }
            e_ItemIcon.Sprite = _shopItemInfo.Cfg.IconPath;
        }
        else//种植
        {
            e_TxtResult.Text = "恭喜！种植成功".ParseColorText("FF0059");
            e_TxtDesc.Text = $"{_item.Name.ParseColorText("FF0059")} * {1}";
            e_Bg.Sprite = "28button_green.png";
            e_ItemIcon.Sprite = _item.IconPath;
        }
    }
    private void HideUI()
    {
        UIMod.Inst.HideUI();
    }
}
