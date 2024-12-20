using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[UIBind(UIDef.UI_MAIN)]
public class UIMain : UILogicBase
{
    private TextMeshProUGUI _coinsCount;
    public override void OnHide()
    {
        base.OnHide();
        PlayerEvent.OnCoinsChanged -= SetCoins;
    }

    public override void OnInit()
    {
        base.OnInit();
        _coinsCount = GetUIComponentInchildren<TextMeshProUGUI>("PlayerInfo/Kuang/CoinIcon/Bg/CoinsCount");
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        PlayerEvent.OnCoinsChanged += SetCoins;
        SetCoins();
    }
    private void SetCoins()
    {
        _coinsCount.text = PlayerData.Inst.Coins.ToString();
    }
}
