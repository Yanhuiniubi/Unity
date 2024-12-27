using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[UIBind(UIDef.UI_SHOP)]
public class UIShop : UILogicBase
{
    private UIContainer<UIShopItem> _shopGrid;
    private UIContainer<UIShopTab> _tabGrid;
    private ToggleGroup _toggleGroup;
    private Button _btnClose;
    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnInit()
    {
        base.OnInit();
        _btnClose = GetUIComponentInchildren<Button>("CloseBtn");
        _shopGrid = new UIContainer<UIShopItem>(gameObject.transform.Find("Scroll View/Grid").gameObject);
        _tabGrid = new UIContainer<UIShopTab>(gameObject.transform.Find("ScrollViewPage/GridPage").gameObject);
        _toggleGroup = GetUIComponentInchildren<ToggleGroup>("ScrollViewPage/GridPage");
        _btnClose.onClick.AddListener(CloseUI);
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        SetTab();
        SetShopItem();
    }
    private string[] _titles = { "Ö²±»" };
    private void SetTab()
    {
        _tabGrid.Ensuresize(1);
        var children = _tabGrid.Children;
        int count = children.Count;
        for (int i = 0;i < count;i++)
        {
            children[i].SetData(i, _titles[i], _toggleGroup);
        }
    }
    private void SetShopItem()
    {

    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
}
public class UIShopItem : UITemplateBase
{
    private Button _btnBuy;
    private Image _icon;
    private TextMeshProUGUI _itemPrice;
    private TextMeshProUGUI _itemName;
    private Button _descBtn;

    private TableItemShop _cfg;
    public override void OnInit()
    {
        base.OnInit();
        _btnBuy = GetUIComponent<Button>();
        _descBtn = GetUIComponentInchildren<Button>("DescBtn");
        _icon = GetUIComponentInchildren<Image>("ItemImg");
        _itemPrice = GetUIComponentInchildren<TextMeshProUGUI>("HorizontalLayout/ItemCount");
        _itemName = GetUIComponentInchildren<TextMeshProUGUI>("ItemName");
        _btnBuy.onClick.AddListener(OnBuyBtnClick);
        _descBtn.onClick.AddListener(OnDescBtnClick);
    }
    public void SetData(TableItemShop cfg)
    {
        _cfg = cfg;
        _itemPrice.text = cfg.Price.ToString();
        _icon.sprite = ResData.Inst.GetResByPath<Sprite>(cfg.IconPath);
        _itemName.text = cfg.Name;
    }
    private void OnBuyBtnClick()
    {
        UIMod.Inst.ShowUI<UIItemUse>(UIDef.UI_UIITEMUSE, _cfg);
    }
    private void OnDescBtnClick()
    {
        UIMod.Inst.ShowUI<UIItemDesc>(UIDef.UI_UIITEMDESC, TableItemMainMod.Get(_cfg.ItemID));
    }
}
public class UIShopTab : UITemplateBase
{
    private TextMeshProUGUI _txtTitle;
    private Toggle _toggle;
    private Image _bg;
    private int _page;

    private Sprite _selected;
    private Sprite _unSelected;
    public override void OnInit()
    {
        base.OnInit();
        _txtTitle = GetUIComponentInchildren<TextMeshProUGUI>("Toggle/TxtPageName");
        _toggle = GetUIComponentInchildren<Toggle>("Toggle");
        _bg = GetUIComponent<Image>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    public void SetData(int page,string title,ToggleGroup toggleGroup)
    {
        _page = page;
        _txtTitle.text = title;
        _toggle.group = toggleGroup;
    }
    private void OnValueChanged(bool isOn)
    {
        if (_selected == null)
            _selected = ResData.Inst.GetResByPath<Sprite>("Icon/Selected");
        if (_unSelected == null)
            _unSelected = ResData.Inst.GetResByPath<Sprite>("Icon/UnSelected");
        _bg.sprite = isOn ? _selected : _unSelected;
    }
}

