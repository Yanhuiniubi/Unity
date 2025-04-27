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
public class UIItemUse : UIItemUseBase
{
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
        e_CloseBtn.onClick.AddListener(CloseUI);
        e_ConfirmBtn.onClick.AddListener(OnConfirmBtnClick);
        e_Slider.onValueChanged.AddListener(OnCountChanged);
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
            e_TxtName.text = _garbageCfg.Name;
            e_Slider.minValue = 0;
            e_Slider.maxValue = _garbage.ItemInfo.Count;
            e_Slider.value = 0;
            e_ItemImg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(_garbageCfg.IconPath);
        }
        else if (_shopItem != null)
        {
            _shopItemCfg = TableItemMainMod.Get(_shopItem.ItemID);
            e_TxtName.text = _shopItemCfg.Name;
            e_Slider.minValue = 1;
            e_Slider.maxValue = 10;
            e_Slider.value = 1;
            e_ItemImg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(_shopItemCfg.IconPath);
            string color = "";
            if (_shopItem.Price > PlayerData.Inst.Coins)
                color = "FF1515";
            else
                color = "3CFF14";
            e_TxtUse.text = $"购买1个" + $"（${_shopItem.Price}）".ParseColorText(color);
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
            info.IsSuccess = BagData.Inst.UseItem(TableItemMainMod.Get(_garbage.ItemInfo.ID), (int)e_Slider.value, info.DustbinType);
            CloseUI();
            UIMod.Inst.ShowUI<UIItemUseResult>(UIDef.UI_ITEMUSERESULT, info);
            if (data != null)
                TaskData.Inst.CheckTask("Recycle", new RecycleGarbageInfo((int)e_Slider.value, info.IsSuccess));
        }
        else if (_shopItem != null)
        {
            ShopItemBuyResultInfo info = new ShopItemBuyResultInfo();
            info.Cfg = _shopItemCfg;
            info.Count = (int)e_Slider.value;
            info.IsSuccess = ShopData.Inst.ReqBuyShopItem(_shopItem, (int)e_Slider.value);
            CloseUI();
            UIMod.Inst.ShowUI<UIItemUseResult>(UIDef.UI_ITEMUSERESULT, info);
        }
    }
    private void OnCountChanged(float val)
    {
        if (_garbage != null)
        {
            e_TxtUse.text = $"投入{val}个";
            if (val == 0)
                e_ConfirmBtn.gameObject.SetActive(false);
            else
                e_ConfirmBtn.gameObject.SetActive(true);
        }
        else if (_shopItem != null)
        {
            string color = "";
            int needCoin = _shopItem.Price * (int)val;
            if (needCoin > PlayerData.Inst.Coins)
                color = "FF1515";
            else
                color = "3CFF14";
            e_TxtUse.text = $"购买{val}个" + $"（${_shopItem.Price * val}）".ParseColorText(color);
        }
    }
}
