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
                e_TxtResult.Text = "��ϲ������������ȷ".ParseColorText("FF0059");
                e_TxtDesc.Text = $"{_garbageInfo.Cfg.Name.ParseColorText("FF0059")}����{garbage}";
                e_Bg.Sprite = "28button_green.png";
            }
            else
            {
                e_TxtResult.Text = "Ŷ���������������".ParseColorText("FFFFFF");
                e_TxtDesc.Text = $"{_garbageInfo.Cfg.Name.ParseColorText("FFFFFF")}������{garbage}";
                e_Bg.Sprite = "30button_red.png";
            }
            e_ItemIcon.Sprite = _garbageInfo.Cfg.IconPath;
        }
        else if (_shopItemInfo != null)
        {
            if (_shopItemInfo.IsSuccess)
            {
                e_TxtResult.Text = "��ϲ������ɹ�".ParseColorText("FF0059");
                e_TxtDesc.Text = $"{_shopItemInfo.Cfg.Name.ParseColorText("FF0059")} * {_shopItemInfo.Count}";
                e_Bg.Sprite = "28button_green.png";
            }
            else
            {
                e_TxtResult.Text = "Ŷ��������ʧ��".ParseColorText("FFFFFF");
                e_TxtDesc.Text = $"�ʽ����Թ���{_shopItemInfo.Cfg.Name} * {_shopItemInfo.Count}";
                e_Bg.Sprite = "30button_red.png";
            }
            e_ItemIcon.Sprite = _shopItemInfo.Cfg.IconPath;
        }
        else//��ֲ
        {
            e_TxtResult.Text = "��ϲ����ֲ�ɹ�".ParseColorText("FF0059");
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
