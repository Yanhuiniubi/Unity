using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    public override void OnInit()
    {
        base.OnInit();
        _title = GetUIComponentInchildren<TextMeshProUGUI>("TxtTitle");
        _container = new UIContainer<UIBagItem>(gameObject.transform.Find("Grid").gameObject);
    }
    public override void OnShow(object param)
    {
        base.OnShow(param);

        SetItems();
    }
    public override void OnHide()
    {
        base.OnHide();
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
}
