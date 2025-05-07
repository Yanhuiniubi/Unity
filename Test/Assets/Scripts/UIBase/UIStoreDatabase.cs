using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIStoreDataBase : UILogicBase
{
    protected GameLabel e_TxtDesc;//UI-StoreData/BgTransparent/Bg/e_TxtDesc
    public override void OnInit()
    {
        base.OnInit();
        e_TxtDesc = MakeUIComponent<GameLabel>("BgTransparent/Bg/e_TxtDesc");
    }
}