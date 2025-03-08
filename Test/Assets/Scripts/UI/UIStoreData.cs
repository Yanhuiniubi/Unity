using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UIBind(UIDef.UI_StoreData)]
public class UIStoreData : UILogicBase
{
    private TextMeshProUGUI _desc;

    public override void OnInit()
    {
        base.OnInit();
        _desc = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/TxtDesc");
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        _desc.text = param.ToString();
        GameMod.Inst.StartCoroutine(CloseUI());
    }
    IEnumerator CloseUI()
    {
        CuttingMod.Inst.PauseAllLoggers();
        yield return new WaitForSeconds(3.5f);
        UIMod.Inst.HideUI();
        CuttingMod.Inst.UnPauseAllLoggers();
    }
}
