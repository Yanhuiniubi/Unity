using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBagItem : UITemplateBase
{
    private Button _btn;
    private Image _icon;
    private TextMeshProUGUI _itemCount;
    private string _id;
    public override void OnInit()
    {
        base.OnInit();
        _btn = GetUIComponent<Button>();
        _icon = GetUIComponentInchildren<Image>("ItemImg");
        _itemCount = GetUIComponentInchildren<TextMeshProUGUI>("ItemCount");
        _btn.onClick.AddListener(OnBtnClick);
    }
    public void SetData(string id,int count)
    {
        _id = id;
    }
    private void OnBtnClick()
    {

    }
}
