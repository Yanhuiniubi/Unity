using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIMainBase : UILogicBase
{
    protected GameLabel e_CoinsCount;//UI-Main/PlayerInfo/Kuang/CoinIcon/Bg/e_CoinsCount
    protected GameLabel e_TxtGarbageCount;//UI-Main/PlayerInfo/Kuang/BgGarbageCount/e_TxtGarbageCount
    protected GameLabel e_TaskDesc;//UI-Main/TaskArea/Bg/e_TaskDesc
    public override void OnInit()
    {
        base.OnInit();
        e_CoinsCount = MakeUIComponent<GameLabel>("PlayerInfo/Kuang/CoinIcon/Bg/e_CoinsCount");
        e_TxtGarbageCount = MakeUIComponent<GameLabel>("PlayerInfo/Kuang/BgGarbageCount/e_TxtGarbageCount");
        e_TaskDesc = MakeUIComponent<GameLabel>("TaskArea/Bg/e_TaskDesc");
    }
}