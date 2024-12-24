using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[UIBind(UIDef.UI_MAIN)]
public class UIMain : UILogicBase
{
    private TextMeshProUGUI _coinsCount;
    private TextMeshProUGUI _garbageCount;
    public override void OnHide()
    {
        base.OnHide();
        PlayerEvent.OnCoinsChanged -= SetCoins;
        PlayerEvent.OnGarbageCntChanged -= SetGarbageCount;
    }

    public override void OnInit()
    {
        base.OnInit();
        _coinsCount = GetUIComponentInchildren<TextMeshProUGUI>("PlayerInfo/Kuang/CoinIcon/Bg/CoinsCount");
        _garbageCount = GetUIComponentInchildren<TextMeshProUGUI>("PlayerInfo/Kuang/BgGarbageCount/TxtGarbageCount");
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        PlayerEvent.OnCoinsChanged += SetCoins;
        PlayerEvent.OnGarbageCntChanged += SetGarbageCount;
        SetCoins();
        SetGarbageCount();
    }
    private void SetCoins()
    {
        _coinsCount.text = PlayerData.Inst.Coins.ToString();
    }
    private void SetGarbageCount()
    {
        _garbageCount.text = $"Ê£ÓàÀ¬»øÊýÁ¿£º{GarbageData.Inst.GarbageCount.ToString().ParseColorText("FD17F5")}";
    }
}
