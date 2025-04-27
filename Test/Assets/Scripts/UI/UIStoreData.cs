using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UIBind(UIDef.UI_StoreData)]
public class UIStoreData : UIStoreDataBase
{

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        e_TxtDesc.text = param.ToString();
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
