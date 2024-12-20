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
    public static Action OnItemChanged;
}
public class BagData
{
    public static readonly BagData Inst = new BagData();
    private List<ItemInfo> _itemList;
    private Dictionary<string, ItemInfo> _itemDic;
    private bool _isDirt;
    public List<ItemInfo> ItemList => _itemList;
    private BagData()
    {
        _itemList = new List<ItemInfo>();
        _itemDic = new Dictionary<string, ItemInfo>();
    }
    /// <summary>
    /// 添加垃圾
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="count"></param>
    public void AddItem(string ID,int count)
    {
        if (_itemDic.ContainsKey(ID))
            _itemDic[ID].Count += count;
        else
        {
            ItemInfo info = new ItemInfo();
            info.ID = ID;
            info.Count = count;
            _itemDic.Add(ID, info);
            _itemList.Add(info);
        }    
        _isDirt = true;
        BagEvent.OnItemChanged?.Invoke();
    }
    /// <summary>
    /// 使用垃圾
    /// </summary>
    /// <param name="dustbinType"></param>
    /// <param name="itemID"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool UseItem(int dustbinType,string itemID, int count)
    {
        if (_itemDic.ContainsKey(itemID) && _itemDic[itemID].Count >= count)
        {
            _itemDic[itemID].Count -= count;
            if (_itemDic[itemID].Count == 0)
            {
                _itemDic.Remove(itemID);
                for (int i = 0;i < _itemList.Count;i++)
                {
                    if (_itemList[i].ID.Equals(itemID))
                    {
                        _itemList.RemoveAt(i);
                        break;
                    }
                }
            }
            DoUseItem(dustbinType,TableItemGarbageMod.Get(itemID).Type,count);
            _isDirt = true;
            BagEvent.OnItemChanged?.Invoke();

        }
        return false;
    }
    private void DoUseItem(int dustbinType,int itemType,int count)
    {
        if (dustbinType == itemType)
            PlayerData.Inst.AddCoinsByItemCount(count);
        else
            PlayerData.Inst.DeleteCoinsByItemCount(count);

    }
    /// <summary>
    /// 脏标记时排序
    /// </summary>
    public void SortItem()
    {
        if (_isDirt)
        {
            _itemList.Sort((x, y) => y.Count.CompareTo(x.Count));
            _isDirt = false;
        }
    }
    public void SaveData()
    {
        ItemInfoList itemListData = new ItemInfoList { itemList = _itemList };
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

            _itemList = itemListData.itemList;
            _itemDic.Clear();
            foreach (var item in _itemList)
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
