using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        Cursor.lockState = CursorLockMode.None;
        _openFrom = (eOpenBagFrom)param;
        switch (_openFrom)
        {
            case eOpenBagFrom.Dustbin_Kehuishou:
                _title.text = "¿É»ØÊÕÀ¬»øÍ°";
                break;
            case eOpenBagFrom.Dustbin_Youhai:
                _title.text = "ÓÐº¦À¬»øÍ°";
                break;
            case eOpenBagFrom.Dustbin_Chuyu:
                _title.text = "³øÓàÀ¬»øÍ°";
                break;
            case eOpenBagFrom.Dustbin_Qita:
                _title.text = "ÆäËûÀ¬»øÍ°";
                break;
            case eOpenBagFrom.BagKey:
                _title.text = "±³°ü";
                break;
            default:
                break;
        }
        SetItems();
    }
    public override void OnHide()
    {
        base.OnHide();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void SetItems()
    {
        BagData.Inst.SortItem();
        var itemList = BagData.Inst.ItemList;
        int count = itemList.Count;
        _container.Ensuresize(count);
        var children = _container.Children;
        for (int i = 0;i < count;i++)
        {
            children[i].SetData(itemList[i].ID, itemList[i].Count,_openFrom != eOpenBagFrom.BagKey);
        }
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
}
