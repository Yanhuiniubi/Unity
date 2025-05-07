using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[UIBind(UIDef.UI_COINCHANGED)]
public class UICoinChanged : UILogicBase
{
    private TextMeshProUGUI _txtCoinChange;
    private Animation _ani;
    private Coroutine co;
    public override void OnHide()
    {
        base.OnHide();
        GameMod.Inst.StopCoroutine(co);
    }

    public override void OnInit()
    {
        base.OnInit();
        _txtCoinChange = GetUIComponent<TextMeshProUGUI>("TxtCoinChange");
        _ani = GetUIComponent<Animation>();
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        int value = (int)param;
        if (value > 0)
            _txtCoinChange.text = $"+{value}".ParseColorText("02F100");
        else
            _txtCoinChange.text = $"{value}".ParseColorText("F12500");
        co = GameMod.Inst.StartCoroutine(WaitAnimationEnd());
    }
    IEnumerator WaitAnimationEnd()
    {
        while (_ani.isPlaying)
        {
            yield return null;
        }
        UIMod.Inst.HideTips(UIDef.UI_COINCHANGED);
    }
}
