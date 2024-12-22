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
    /// <summary>
    /// ���ݵ����������ӽ��
    /// </summary>
    /// <param name="count"></param>
    public int AddCoinsByItemCount(int count)
    {
        _coins += count * 5;
        PlayerEvent.OnCoinsChanged?.Invoke();
        return count * 5;
    }
    /// <summary>
    /// �������ʹ��󣬿۳����
    /// </summary>
    /// <param name="count"></param>
    public int DeleteCoinsByItemCount(int count)
    {
        _coins = Math.Max(0, _coins - count * 2);
        PlayerEvent.OnCoinsChanged?.Invoke();
        return -count * 2;
    }
}
