using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RecycleGarbageInfo
{
    public int Count;
    public bool Result;

    public RecycleGarbageInfo(int count, bool result)
    {
        Count = count;
        Result = result;
    }
}
public class GarbageUseInfo
{
    public ItemInfo ItemInfo;
    public eOpenBagFrom OpenBagFrom;
}
[UIBind(UIDef.UI_UIITEMUSE)]
public class UIItemUse : UILogicBase
{
    private TextMeshProUGUI _txtName;
    private TextMeshProUGUI _txtUse;
    private Button _closeBtn;
    private Button _confirmBtn;
    private Slider _slider;
    private Image _imgItem;

    private GarbageUseInfo _garbage;
    private TableItemShop _shopItem;
    public override void OnHide()
    {
        base.OnHide();
        _garbage = null;
        _shopItem = null;
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
        if (param is GarbageUseInfo garbage)
            _garbage = garbage;
        else if (param is TableItemShop shopItem)
            _shopItem = shopItem;
        SetView();
    }
    private TableItemMain _garbageCfg;
    private TableItemMain _shopItemCfg;
    private void SetView()
    {
        if (_garbage != null)
        {
            _garbageCfg = TableItemMainMod.Get(_garbage.ItemInfo.ID);
            _txtName.text = _garbageCfg.Name;
            _slider.minValue = 0;
            _slider.maxValue = _garbage.ItemInfo.Count;
            _slider.value = 0;
            _imgItem.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(_garbageCfg.IconPath);
        }
        else if (_shopItem != null)
        {
            _shopItemCfg = TableItemMainMod.Get(_shopItem.ItemID);
            _txtName.text = _shopItemCfg.Name;
            _slider.minValue = 1;
            _slider.maxValue = 10;
            _slider.value = 1;
            _imgItem.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(_shopItemCfg.IconPath);
            string color = "";
            if (_shopItem.Price > PlayerData.Inst.Coins)
                color = "FF1515";
            else
                color = "3CFF14";
            _txtUse.text = $"购买1个" + $"（${_shopItem.Price}）".ParseColorText(color);
        }
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
    private void OnConfirmBtnClick()
    {
        if (_garbage != null)
        {
            if (TaskData.Inst.Chapter < 2)
                return;
            var data = TaskData.Inst.CurTask;
            GarbageUseResultInfo info = new GarbageUseResultInfo();
            info.Cfg = _garbageCfg;
            info.DustbinType = (int)_garbage.OpenBagFrom;
            info.IsSuccess = BagData.Inst.UseItem(TableItemMainMod.Get(_garbage.ItemInfo.ID), (int)_slider.value, info.DustbinType);
            CloseUI();
            UIMod.Inst.ShowUI<UIItemUseResult>(UIDef.UI_ITEMUSERESULT, info);
            if (data != null)
                TaskData.Inst.CheckTask("Recycle", new RecycleGarbageInfo((int)_slider.value, info.IsSuccess));
        }
        else if (_shopItem != null)
        {
            ShopItemBuyResultInfo info = new ShopItemBuyResultInfo();
            info.Cfg = _shopItemCfg;
            info.Count = (int)_slider.value;
            info.IsSuccess = ShopData.Inst.ReqBuyShopItem(_shopItem, (int)_slider.value);
            CloseUI();
            UIMod.Inst.ShowUI<UIItemUseResult>(UIDef.UI_ITEMUSERESULT, info);
        }
    }
    private void OnCountChanged(float val)
    {
        if (_garbage != null)
        {
            _txtUse.text = $"投入{val}个";
            if (val == 0)
                _confirmBtn.gameObject.SetActive(false);
            else
                _confirmBtn.gameObject.SetActive(true);
        }
        else if (_shopItem != null)
        {
            string color = "";
            int needCoin = _shopItem.Price * (int)val;
            if (needCoin > PlayerData.Inst.Coins)
                color = "FF1515";
            else
                color = "3CFF14";
            _txtUse.text = $"购买{val}个" + $"（${_shopItem.Price * val}）".ParseColorText(color);
        }
    }
}
