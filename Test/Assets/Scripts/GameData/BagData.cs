using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 道具信息
/// </summary>
[System.Serializable]
public class ItemInfo
{
    public string ID;
    public int Count;
    public ItemInfo() { }
    public ItemInfo(string id, int count)
    {
        ID = id;
        Count = count;
    }
}

[System.Serializable]
public class ItemInfoList
{
    public List<ItemInfo> itemList;
}

public static class BagEvent
{
    /// <summary>
    /// 垃圾数量变化
    /// </summary>
    public static Action OnGarbageItemChanged;
}
public class BagData
{
    public static readonly BagData Inst = new BagData();
    private List<ItemInfo> _itemGarbageList;
    private Dictionary<string, ItemInfo> _itemDic;
    private bool _isDirt;
    public List<ItemInfo> ItemList => _itemGarbageList;
    private BagData()
    {
        _itemGarbageList = new List<ItemInfo>();
        _itemDic = new Dictionary<string, ItemInfo>();
    }
    /// <summary>
    /// 添加垃圾
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="count"></param>
    public void AddItem(TableItemMain cfg,int count)
    {
        if (cfg.ItemType >= 0 && cfg.ItemType <= 3)//垃圾
        {
            if (_itemDic.ContainsKey(cfg.ID))
                _itemDic[cfg.ID].Count += count;
            else
            {
                ItemInfo info = new ItemInfo();
                info.ID = cfg.ID;
                info.Count = count;
                _itemDic.Add(cfg.ID, info);
                _itemGarbageList.Add(info);
            }
            _isDirt = true;
            BagEvent.OnGarbageItemChanged?.Invoke();
        }
        
    }
    public void UseItem(TableItemMain cfg, int count)
    {

    }
    /// <summary>
    /// 使用垃圾
    /// </summary>
    /// <param name="dustbinType"></param>
    /// <param name="itemID"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool UseGarbage(TableItemMain cfg, int count, int dustbinType)
    {
        string itemID = cfg.ID;
        int coinsChange = 0;
        if (_itemDic.ContainsKey(itemID) && _itemDic[itemID].Count >= count)
        {
            _itemDic[itemID].Count -= count;
            if (_itemDic[itemID].Count == 0)
            {
                _itemDic.Remove(itemID);
                for (int i = 0;i < _itemGarbageList.Count;i++)
                {
                    if (_itemGarbageList[i].ID.Equals(itemID))
                    {
                        _itemGarbageList.RemoveAt(i);
                        break;
                    }
                }
            }
            coinsChange = DoUseGarbage(dustbinType, cfg.ItemType,count);
            UIMod.Inst.ShowTipsUI<UICoinChanged>(UIDef.UI_COINCHANGED, coinsChange);
            _isDirt = true;
            BagEvent.OnGarbageItemChanged?.Invoke();
        }
        return coinsChange > 0;
    }
    private int DoUseGarbage(int dustbinType,int itemType,int count)
    {
        if (dustbinType == itemType)
            return PlayerData.Inst.AddCoinsByItemCount(count);
        else
            return PlayerData.Inst.DeleteCoinsByItemCount(count);

    }
    /// <summary>
    /// 脏标记时排序
    /// </summary>
    public void SortItem()
    {
        if (_isDirt)
        {
            _itemGarbageList.Sort((x, y) => y.Count.CompareTo(x.Count));
            _isDirt = false;
        }
    }
    public void SaveData()
    {
        ItemInfoList itemListData = new ItemInfoList { itemList = _itemGarbageList };
        string json = JsonUtility.ToJson(itemListData, true);

        // 使用 Application.persistentDataPath 以确保跨平台兼容性
        string filePath = Path.Combine(Application.persistentDataPath, "savedItems.json");
        File.WriteAllText(filePath, json);
    }

    public void LoadData()
    {
        // 使用 Application.persistentDataPath 以确保跨平台兼容性
        string filePath = Path.Combine(Application.persistentDataPath, "savedItems.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ItemInfoList itemListData = JsonUtility.FromJson<ItemInfoList>(json);

            _itemGarbageList = itemListData.itemList;
            _itemDic.Clear();
            foreach (var item in _itemGarbageList)
            {
                _itemDic.Add(item.ID, item);
            }
        }
        else
        {
            Debug.LogError("No save file found!");
        }
    }
}
