using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PlayerEvent
{
    public static Action OnCoinsChanged;
}

public class PlayerData
{
    public static readonly PlayerData Inst = new PlayerData();
    private int _coins;
    public int Coins => _coins;
    private PlayerData()
    {

    }
    public void AddCoinsByItemCount(int count)
    {
        _coins += count * 5;
        PlayerEvent.OnCoinsChanged?.Invoke();
    }
    public bool UseCoinsByItemCount(int count)
    {
        if (count * 5 > _coins)
            return false;
        _coins -= count * 5;
        PlayerEvent.OnCoinsChanged?.Invoke();
        return true;
    }
    public void DeleteCoinsByItemCount(int count)
    {
        _coins = Math.Max(0, _coins - count * 2);
        PlayerEvent.OnCoinsChanged?.Invoke();
    }
}
