using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUseResultInfo
{
    public bool IsSuccess;
    public int DustbinType;
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

    private ItemUseResultInfo _info;
    public override void OnHide()
    {
        base.OnHide();
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
        _info = param as ItemUseResultInfo;
        if (_info == null)
        {
            Debug.LogError("UIItemUseResult OnShow param isnot ItemUseResultInfo");
            return;
        }
        SetView();
    }
    private void SetView()
    {
        string garbage = "";
        switch ((eGarbageType)_info.DustbinType)
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
        if (_info.IsSuccess)
        {
            _txtResult.text = "��ϲ������������ȷ".ParseColorText("FF0059");
            _txtDesc.text = $"{_info.Cfg.Name.ParseColorText("FF0059")}����{garbage}";
            _bg.sprite = ResData.Inst.GetResByPath<Sprite>("Icon/28button_green");
        }
        else
        {
            _txtResult.text = "Ŷ���������������".ParseColorText("FFFFFF");
            _txtDesc.text = $"{_info.Cfg.Name.ParseColorText("FFFFFF")}������{garbage}";
            _bg.sprite = ResData.Inst.GetResByPath<Sprite>("Icon/30button_red");
        }
        _itemIcon.sprite = ResData.Inst.GetResByPath<Sprite>(_info.Cfg.IconPath);
    }
    private void HideUI()
    {
        UIMod.Inst.HideUI();
    }
}
