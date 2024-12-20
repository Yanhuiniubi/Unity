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
    Dustbin_Kehuishou,
    Dustbin_Youhai,
    Dustbin_Chuyu,
    Dustbin_Qita,
    BagKey,
}
[UIBind(UIDef.UI_UIBAG)]
public class UIBag : UILogicBase
{
    private TextMeshProUGUI _title;
    private UIContainer<UIBagItem> _container;
    private Button _closeBtn;
    private eOpenBagFrom _openFrom;
    public override void OnInit()
    {
        base.OnInit();
        _title = GetUIComponentInchildren<TextMeshProUGUI>("ImgTitle/TxtTitle");
        _container = new UIContainer<UIBagItem>(gameObject.transform.Find("Scroll View/Grid").gameObject);
        _closeBtn = GetUIComponentInchildren<Button>("CloseBtn");
        _closeBtn.onClick.AddListener(CloseUI);
    }
    public override void OnShow(object param)
    {
        base.OnShow(param);
        BagEvent.OnItemChanged += SetItems;
        Cursor.lockState = CursorLockMode.None;
        _openFrom = (eOpenBagFrom)param;
        switch (_openFrom)
        {
            case eOpenBagFrom.Dustbin_Kehuishou:
                _title.text = "可回收垃圾".ParseColorText("00CBFF") + "桶";
                break;
            case eOpenBagFrom.Dustbin_Youhai:
                _title.text = "有害垃圾".ParseColorText("FF0700") + "桶";
                break;
            case eOpenBagFrom.Dustbin_Chuyu:
                _title.text = "厨余垃圾".ParseColorText("78FF23") + "桶";
                break;
            case eOpenBagFrom.Dustbin_Qita:
                _title.text = "其他垃圾".ParseColorText("CBFF9E") + "桶";
                break;
            case eOpenBagFrom.BagKey:
                _title.text = "背包".ParseColorText("FFFFFF");
                break;
            default:
                break;
        }
        SetItems();
    }
    public override void OnHide()
    {
        base.OnHide();
        BagEvent.OnItemChanged -= SetItems;
        Cursor.lockState = CursorLockMode.Locked;
    }
    /// <summary>
    /// 设置背包道具显示
    /// </summary>
    private void SetItems()
    {
        BagData.Inst.SortItem();
        var itemList = BagData.Inst.ItemList;
        int count = itemList.Count;
        _container.Ensuresize(count);
        var children = _container.Children;
        for (int i = 0;i < count;i++)
        {
            children[i].SetData(itemList[i].ID, itemList[i].Count,_openFrom != eOpenBagFrom.BagKey, _openFrom);
        }
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
}
