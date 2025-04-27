using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIStoreDataBase : UILogicBase
{
    protected TextMeshProUGUI e_TxtDesc;//UI-StoreData/BgTransparent/Bg/e_TxtDesc
    public override void OnInit()
    {
        base.OnInit();
        e_TxtDesc = GetUIComponentInchildren<TextMeshProUGUI>("BgTransparent/Bg/e_TxtDesc");
    }
}