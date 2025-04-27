using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIMainBase : UILogicBase
{
    protected TextMeshProUGUI e_CoinsCount;//UI-Main/PlayerInfo/Kuang/CoinIcon/Bg/e_CoinsCount
    protected TextMeshProUGUI e_TxtGarbageCount;//UI-Main/PlayerInfo/Kuang/BgGarbageCount/e_TxtGarbageCount
    protected TextMeshProUGUI e_TaskDesc;//UI-Main/TaskArea/Bg/e_TaskDesc
    public override void OnInit()
    {
        base.OnInit();
        e_CoinsCount = GetUIComponentInchildren<TextMeshProUGUI>("PlayerInfo/Kuang/CoinIcon/Bg/e_CoinsCount");
        e_TxtGarbageCount = GetUIComponentInchildren<TextMeshProUGUI>("PlayerInfo/Kuang/BgGarbageCount/e_TxtGarbageCount");
        e_TaskDesc = GetUIComponentInchildren<TextMeshProUGUI>("TaskArea/Bg/e_TaskDesc");
    }
}