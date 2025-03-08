using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;
using System.IO;

public static class PlayerEvent
{
    public static Action OnCoinsChanged;
    public static Action OnGarbageCntChanged;
}
[Serializable]
public class PlayerStoreInfo
{
    public int Coins;
    public Vector3 Position;
}

public class PlayerData
{
    public static readonly PlayerData Inst = new PlayerData();
    private int _coins = 2000;
    public int Coins => _coins;
    private PlayerData()
    {
        
    }
    public void OnStoreData()
    {
        PlayerStoreInfo infos = new PlayerStoreInfo();
        infos.Coins = _coins;
        infos.Position = GameMod.Inst.PlayerPosition;
        string json = JsonUtility.ToJson(infos, true);
        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.json");
        File.WriteAllText(path, json);
    }
    public void OnReadData()
    {
        // 获取文件路径
        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.json");
        // 读取文件内容
        string json = "";
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
        else
        {
            Debug.LogError("File not found: " + path);
            return;
        }
        // 反序列化JSON数据
        PlayerStoreInfo data = JsonUtility.FromJson<PlayerStoreInfo>(json);
        _coins = data.Coins;
        GameMod.Inst.gameObject.transform.position = data.Position;
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
