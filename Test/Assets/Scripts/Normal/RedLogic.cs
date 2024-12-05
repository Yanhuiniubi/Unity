using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedLogic : UILogicBase
{
    Image img;
    public override void OnInit()
    {
        base.OnInit();
        img = GetUIComponent<Image>("Red");
    }
    public override void OnHide()
    {
        base.OnHide();
        img.color = Color.green;
    }
}
