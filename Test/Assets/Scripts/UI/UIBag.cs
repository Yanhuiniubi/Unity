using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum eOpenBagFrom
{
    BagKey,
    Dustbin_Kehuishou,
    Dustbin_Youhai,
    Dustbin_Chuyu,
    Dustbin_Qita,
}

public class UIBag : UILogicBase
{
    private TextMeshProUGUI _title;
    private UIContainer<UIBagItem> _container;
    private Button _closeBtn;
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
            children[i].SetData(itemList[i].ID, itemList[i].Count);
        }
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
}
