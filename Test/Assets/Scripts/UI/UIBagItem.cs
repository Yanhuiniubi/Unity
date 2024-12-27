using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBagItem : UITemplateBase
{
    private Button _btnUse;
    private Image _icon;
    private TextMeshProUGUI _itemCount;
    private TextMeshProUGUI _itemName;
    private Button _descBtn;
    private string _id;
    private int _count;
    private bool _canUseItem;
    private eOpenBagFrom _openBagFrom;
    private TableItemMain _cfg;
    public override void OnInit()
    {
        base.OnInit();
        _btnUse = GetUIComponent<Button>();
        _descBtn = GetUIComponentInchildren<Button>("DescBtn");
        _icon = GetUIComponentInchildren<Image>("ItemImg");
        _itemCount = GetUIComponentInchildren<TextMeshProUGUI>("ItemCount");
        _itemName = GetUIComponentInchildren<TextMeshProUGUI>("ItemName");
        _btnUse.onClick.AddListener(OnUseBtnClick);
        _descBtn.onClick.AddListener(OnDescBtnClick);
    }
    public void SetData(string id,int count,bool canUseItem, eOpenBagFrom openBagFrom)
    {
        _id = id;
        _count = count;
        _canUseItem = canUseItem;
        _openBagFrom = openBagFrom;
        _itemCount.text = count.ToString();
        _cfg = TableItemMainMod.Get(id);
        _icon.sprite = ResData.Inst.GetResByPath<Sprite>(_cfg.IconPath);
        _itemName.text = _cfg.Name;
    }
    private void OnUseBtnClick()
    {
        if (_canUseItem)
        {
            ItemUseInfo info = new ItemUseInfo();
            info.ItemInfo = new ItemInfo(_id, _count);
            info.OpenBagFrom = _openBagFrom;
            UIMod.Inst.ShowUI<UIItemUse>(UIDef.UI_UIITEMUSE, info);
        }
    }
    private void OnDescBtnClick()
    {
        UIMod.Inst.ShowUI<UIItemDesc>(UIDef.UI_UIITEMDESC, _cfg);
    }
}
