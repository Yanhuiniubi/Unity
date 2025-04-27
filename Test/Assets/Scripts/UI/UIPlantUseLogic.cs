using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UIBind(UIDef.UI_PLANTUSE)]
public class UIPlantUseLogic : UIPlantUseBase
{
    private TableItemMain _item;
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn.onClick.AddListener(CloseUI);
        e_ConfirmBtn.onClick.AddListener(OnConfirmBtnClick);
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        _item = param as TableItemMain;
        SetView();
    }
    private void SetView()
    {
        e_TxtName.text = _item.Name;
        e_ItemImg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(_item.IconPath);
        e_TxtUse.text = $"是否将{_item.Name.ParseColorText("000000")}种植在此？";
    }
    public override void OnHide()
    {
        base.OnHide();
    }
    private void CloseUI()
    {
        UIMod.Inst.HideUI();
    }
    private void OnConfirmBtnClick()
    {
        if (TaskData.Inst.Chapter < 3)
            return;
        var data = TaskData.Inst.CurTask;
        if (data != null)
            TaskData.Inst.CheckTask(_item.ID);
        BagData.Inst.UseItem(_item, 1);
    }
}
 