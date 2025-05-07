using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[UIBind(UIDef.UI_SHOP)]
public class UIShop : UIShopBase
{
    private UIContainer<UIShopItem> _shopGrid;
    private UIContainer<UIShopTab> _tabGrid;
    private ToggleGroup _toggleGroup;
    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnInit()
    {
        base.OnInit();
        _shopGrid = new UIContainer<UIShopItem>(gameObject.transform.Find("Scroll View/Grid").gameObject);
        _tabGrid = new UIContainer<UIShopTab>(gameObject.transform.Find("ScrollViewPage/GridPage").gameObject);
        _toggleGroup = GetUIComponent<ToggleGroup>("ScrollViewPage/GridPage");
        e_CloseBtn.AddClickEvent(CloseUI);
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        SetTab();
    }
    private string[] _titles = { "Ö²±»" };
    private void SetTab()
    {
        _tabGrid.Ensuresize(1);
        var children = _tabGrid.Children;
        int count = children.Count;
        for (int i = 0;i < count;i++)
        {
            children[i].SetData(i, _titles[i], _toggleGroup, SetShopItem);
        }
    }
    private void SetShopItem(int page)
    {
        var itemList = ShopData.Inst.GetShopItemListByPage(page);
        int count = itemList.Count;
        _shopGrid.Ensuresize(count);
        var children = _shopGrid.Children;
        for (int i = 0;i < count;i++)
        {
            children[i].SetData(itemList[i]);
        }
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
}
public class UIShopItem : UIShopContentBase
{
    private Button _btnBuy;

    private TableItemShop _shopItemCfg;
    private TableItemMain _itemCfg;
    public override void OnInit()
    {
        base.OnInit();
        _btnBuy = GetUIComponent<Button>();;
        _btnBuy.onClick.AddListener(OnBuyBtnClick);
        e_DescBtn.AddClickEvent(OnDescBtnClick);
    }
    public void SetData(TableItemShop cfg)
    {
        _shopItemCfg = cfg;
        _itemCfg = TableItemMainMod.Get(_shopItemCfg.ItemID);
        e_ItemCount.Text = cfg.Price.ToString();
        e_ItemImg.Sprite = _itemCfg.IconPath;
        e_ItemName.Text = _itemCfg.Name;
    }
    private void OnBuyBtnClick()
    {
        UIMod.Inst.ShowUI<UIItemUse>(UIDef.UI_UIITEMUSE, _shopItemCfg);
    }
    private void OnDescBtnClick()   
    {
        UIMod.Inst.ShowUI<UIItemDesc>(UIDef.UI_UIITEMDESC, TableItemMainMod.Get(_shopItemCfg.ItemID));
    }
}
public class UIShopTab : UIShopContentBase1
{
    private Image _bg;
    private int _page;

    private Sprite _selected;
    private Sprite _unSelected;
    private Action<int> OnSelectedTab;
    public override void OnInit()
    {
        base.OnInit();
        _bg = GetUIComponent<Image>();
        e_Toggle.AddValueChangeEvent(OnValueChanged);
    }

    public void SetData(int page,string title,ToggleGroup toggleGroup, Action<int> OnSelectedTab)
    {
        _page = page;
        e_TxtPageName.Text = title;
        this.OnSelectedTab = OnSelectedTab;
        e_Toggle.Group = toggleGroup;
        if (page == 0)
        {
            if (e_Toggle.IsOn)
                OnSelectedTab?.Invoke(_page);
            else
                e_Toggle.IsOn = true;
            if (_selected == null)
                _selected = ResData.Inst.GetResByAddressPermanent<Sprite>("Selected.png");
            _bg.sprite = _selected;
        }
    }
    private void OnValueChanged(bool isOn)
    {
        if (_selected == null)
            _selected = ResData.Inst.GetResByAddressPermanent<Sprite>("Selected.png");
        if (_unSelected == null)
            _unSelected = ResData.Inst.GetResByAddressPermanent<Sprite>("UnSelected.png");
        _bg.sprite = isOn ? _selected : _unSelected;
        if (isOn)
            OnSelectedTab?.Invoke(_page);
    }
}

