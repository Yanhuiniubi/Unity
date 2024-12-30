using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

public static class PlayerEvent
{
    public static Action OnCoinsChanged;
    public static Action OnGarbageCntChanged;
}

public class PlayerData
{
    public static readonly PlayerData Inst = new PlayerData();
    private int _coins;
    public int Coins => _coins;
    private PlayerData()
    {

    }
    /// <summary>
    /// 根据道具数量增加金币
    /// </summary>
    /// <param name="count"></param>
    public int AddCoinsByItemCount(int count)
    {
        _coins += count * 5;
        PlayerEvent.OnCoinsChanged?.Invoke();
        return count * 5;
    }
    /// <summary>
    /// 回收类型错误，扣除金币
    /// </summary>
    /// <param name="count"></param>
    public int DeleteCoinsByItemCount(int count)
    {
        _coins = Math.Max(0, _coins - count * 2);
        PlayerEvent.OnCoinsChanged?.Invoke();
        return -count * 2;
    }
    public bool DeleteCoinsByTotalPrice(int spend)
    {
        if (spend > _coins)
            return false;
        _coins -= spend;
        PlayerEvent.OnCoinsChanged?.Invoke();
        return true;
    }
}
