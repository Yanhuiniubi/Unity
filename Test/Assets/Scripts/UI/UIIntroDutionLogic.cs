using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntroDutionLogic : UI3DLogicBase
{
    private DeerUIInfo _info;
    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        _info = param as DeerUIInfo;
        root.transform.position = new Vector3(_info.BasePos.x, _info.BasePos.y + _info.Height, _info.BasePos.z);
    }
}
