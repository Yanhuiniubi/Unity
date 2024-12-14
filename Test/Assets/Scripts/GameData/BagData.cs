using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    public string ID;
    public int Count;
}

[System.Serializable]
public class ItemInfoList
{
    public List<ItemInfo> itemList;
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
    }
    public bool UseItem(string ID, int count)
    {
        if (_itemDic.ContainsKey(ID) && _itemDic[ID].Count >= count)
        {
            _itemDic[ID].Count -= count;
            DoUseItem(ID, count);
            _isDirt = true;
        }
        return false;
    }
    private void DoUseItem(string ID, int count)
    {

    }
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
