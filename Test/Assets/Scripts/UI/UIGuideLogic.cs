using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIBind(UIDef.UI_Guide)]
public class UIGuideLogic : UIGuideBase
{
    public override void OnInit()
    {
        base.OnInit();
        e_CloseBtn.onClick.AddListener(() => UIMod.Inst.HideUI());
    }
}
