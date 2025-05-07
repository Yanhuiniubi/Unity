using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 打开背包来源
/// </summary>
public enum eOpenBagFrom
{
    Dustbin_Kehuishou = 0,
    Dustbin_Youhai = 1,
    Dustbin_Chuyu = 2,
    Dustbin_Qita = 3,
    BagKey = 4,
}
[UIBind(UIDef.UI_UIBAG)]
public class UIBag : UIBagBase
{
    private UIContainer<UIBagItem> _container;
    private UIContainer<UIShopTab> _tabGrid;
    private eOpenBagFrom _openFrom;
    public override void OnInit()
    {
        base.OnInit();
        _container = new UIContainer<UIBagItem>(gameObject.transform.Find("Scroll View/Grid").gameObject);
        _tabGrid = new UIContainer<UIShopTab>(gameObject.transform.Find("ScrollViewPage/e_GridPage").gameObject);
        e_CloseBtn.AddClickEvent(CloseUI);
        e_TxtTitle.AddClickPointerHandle((data) => Debug.Log($"{e_TxtTitle.Text}"));
    }
    public override void OnShow(object param)
    {
        base.OnShow(param);
        BagEvent.OnItemChanged += SetItems;
        _openFrom = (eOpenBagFrom)param;
        switch (_openFrom)
        {
            case eOpenBagFrom.Dustbin_Kehuishou:
                e_TxtTitle.Text = "可回收垃圾".ParseColorText("00CBFF") + "桶";
                break;
            case eOpenBagFrom.Dustbin_Youhai:
                e_TxtTitle.Text = "有害垃圾".ParseColorText("FF0700") + "桶";
                break;
            case eOpenBagFrom.Dustbin_Chuyu:
                e_TxtTitle.Text = "厨余垃圾".ParseColorText("78FF23") + "桶";
                break;
            case eOpenBagFrom.Dustbin_Qita:
                e_TxtTitle.Text = "其他垃圾".ParseColorText("CBFF9E") + "桶";
                break;
            case eOpenBagFrom.BagKey:
                e_TxtTitle.Text = "背包".ParseColorText("FFFFFF");
                break;
            default:
                break;
        }
        SetTab();
    }
    private string[] _titles = { "垃圾", "植被" };
    private void SetTab()
    {
        int tabCount = 1;
        if (_openFrom == eOpenBagFrom.BagKey)
            tabCount = _titles.Length;
        _tabGrid.Ensuresize(tabCount);
        var children = _tabGrid.Children;
        int count = children.Count;
        for (int i = 0; i < count; i++)
        {
            children[i].SetData(i, _titles[i], e_GridPage, SetItems);
        }
    }
    public override void OnHide()
    {
        base.OnHide();
        BagEvent.OnItemChanged -= SetItems;
    }
    /// <summary>
    /// 设置背包道具显示
    /// </summary>
    private void SetItems(int page)
    {
        BagData.Inst.SortItem(page);
        var itemList = BagData.Inst.GetItemListByPage(page);
        if (itemList == null)
            return;
        int count = itemList.Count;
        _container.Ensuresize(count);
        var children = _container.Children;
        for (int i = 0;i < count;i++)
        {
            children[i].SetData(itemList[i].ID, itemList[i].Count, _openFrom);
        }
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
}

public class UIBagItem : UIBagContentBase
{
    private Button _btnUse;
    private string _id;
    private int _count;
    private eOpenBagFrom _openBagFrom;
    private TableItemMain _cfg;
    protected override void OnInit()
    {
        base.OnInit();
        _btnUse = GetUIComponent<Button>();
        _btnUse.onClick.AddListener(OnUseBtnClick);
        e_DescBtn.AddClickEvent(OnDescBtnClick);
    }
    public void SetData(string id, int count, eOpenBagFrom openBagFrom)
    {
        _id = id;
        _count = count;
        _openBagFrom = openBagFrom;
        e_ItemCount.Text = count.ToString();
        _cfg = TableItemMainMod.Get(id);
        e_ItemImg.Sprite = _cfg.IconPath;
        e_ItemName.Text = _cfg.Name;
    }
    private void OnUseBtnClick()
    {
        switch (_openBagFrom)
        {
            case eOpenBagFrom.Dustbin_Kehuishou:
            case eOpenBagFrom.Dustbin_Youhai:
            case eOpenBagFrom.Dustbin_Chuyu:
            case eOpenBagFrom.Dustbin_Qita:
                GarbageUseInfo info = new GarbageUseInfo();
                info.ItemInfo = new ItemInfo(_id, _count);
                info.OpenBagFrom = _openBagFrom;
                UIMod.Inst.ShowUI<UIItemUse>(UIDef.UI_UIITEMUSE, info);
                break;
            case eOpenBagFrom.BagKey:
                if (_cfg.ItemType <= 3)//垃圾
                    return;
                if (_cfg.ItemType == 4)//植被
                {
                    if (TaskData.Inst.Chapter < 3)
                        return;
                    UIMod.Inst.ShowUI<UIPlantUseLogic>(UIDef.UI_PLANTUSE, _cfg);
                }
                break;
            default:
                break;
        }
    }
    private void OnDescBtnClick()
    {
        UIMod.Inst.ShowUI<UIItemDesc>(UIDef.UI_UIITEMDESC, _cfg);
    }
}
