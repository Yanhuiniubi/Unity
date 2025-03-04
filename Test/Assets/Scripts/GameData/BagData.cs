using Supercyan.FreeSample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
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
    /// 道具数量变化
    /// </summary>
    public static Action<int> OnItemChanged;
}
public class BagData
{
    public static readonly BagData Inst = new BagData();
    private Dictionary<string, ItemInfo> _itemDic;
    private Dictionary<int, List<ItemInfo>> _pageDic;
    private Dictionary<int, bool> _pageDirt;
    private BagData()
    {
        _pageDic = new Dictionary<int, List<ItemInfo>>();
        _itemDic = new Dictionary<string, ItemInfo>();
        _pageDirt = new Dictionary<int, bool>();
    }
    public List<ItemInfo> GetItemListByPage(int page)
    {
        if (_pageDic.ContainsKey(page))
            return _pageDic[page];
        _pageDic[page] = new List<ItemInfo>();
        return _pageDic[page];
    }
    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="count"></param>
    public void AddItem(TableItemMain cfg, int count)
    {
        if (_itemDic.ContainsKey(cfg.ID))
            _itemDic[cfg.ID].Count += count;
        else
        {
            ItemInfo info = new ItemInfo();
            info.ID = cfg.ID;
            info.Count = count;
            _itemDic.Add(cfg.ID, info);
            if (_pageDic.ContainsKey(cfg.Page))
                _pageDic[cfg.Page].Add(info);
            else
                _pageDic[cfg.Page] = new List<ItemInfo>() { info };
        }
        _pageDirt[cfg.Page] = true;

        BagEvent.OnItemChanged?.Invoke(cfg.Page);
    }
    private Dictionary<TableItemMain,int> _inTaskRecycleCache = new Dictionary<TableItemMain,int>();
    public void HealItem()
    {
        foreach (var item in _inTaskRecycleCache.Keys)
        {
            AddItem(item, _inTaskRecycleCache[item]);
        }
        _inTaskRecycleCache.Clear();
    }
    public bool UseItem(TableItemMain cfg, params object[] param)
    {
        switch (cfg.ItemType)
        {
            case 0:
            case 1:
            case 2:
            case 3://垃圾
                {
                    int count = (int)param[0];
                    if (TaskData.Inst.CurTask.Param1.Equals("Recycle"))
                    {
                        if (_inTaskRecycleCache.ContainsKey(cfg))
                            _inTaskRecycleCache[cfg] += count;
                        else
                            _inTaskRecycleCache[cfg] = count;
                    }
                    return UseGarbage(cfg, count, (int)param[1]);
                }
            case 4://植被
                ResData.Inst.GetResByAddress<GameObject>(cfg.PrefabPath, (obj) =>
                {
                    int count = (int)param[0];
                    DeleteItem(cfg, count);
                    GameObject plant = GameObject.Instantiate<GameObject>(obj, GameMod.Inst.RoundRoot);
                    plant.transform.position = GameMod.Inst.PlayerPosition + GameMod.Inst.PlayerObj.transform.forward * 0.6f;
                    RaycastHit hit;
                    if (Physics.Raycast(plant.transform.position + 10 * Vector3.up, Vector3.down, out hit,100f, GameMod.Inst._groundMask))
                    {
                        // 通过 BoxCollider 的 center 和 size 计算植物底部的位置
                        BoxCollider boxCollider = plant.GetComponentInChildren<BoxCollider>();
                        float colliderHeight = boxCollider.size.y;
                        float colliderCenter = boxCollider.center.y;

                        // 计算植物实际需要调整的位置
                        plant.transform.position = new Vector3(
                            plant.transform.position.x,
                            hit.point.y + colliderHeight / 2 - colliderCenter,
                            plant.transform.position.z
                        );
                    }
                    UIMod.Inst.HideUI();
                    UIMod.Inst.HideUI();
                    Camera ca = Camera.main;
                    Vector3 originPos = ca.transform.position;
                    Vector3 originRot = ca.transform.eulerAngles;
                    GameObject player = GameMod.Inst.PlayerObj;
                    var aniController = player.GetComponent<SimpleSampleCharacterControl>().m_animator;
                    aniController.SetTrigger("Crouch");
                    CameraSport.StartRotate(ca, player.transform,
                        () =>
                        {
                            ca.transform.position = originPos;
                            ca.transform.eulerAngles = originRot;
                            UIMod.Inst.ShowUI<UIItemUseResult>(UIDef.UI_ITEMUSERESULT, cfg);
                        });
                });
                break;
            default:
                break;
        }
        return true;
    }
    private void DeleteItem(TableItemMain cfg, int count)
    {
        string itemID = cfg.ID;
        int page = cfg.Page;
        _itemDic[itemID].Count -= count;
        if (_itemDic[itemID].Count == 0)
        {
            _itemDic.Remove(itemID);
            var list = _pageDic[page];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID.Equals(itemID))
                {
                    list.RemoveAt(i);
                    break;
                }
            }
        }
        _pageDirt[page] = true;
        BagEvent.OnItemChanged?.Invoke(page);
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
                var list = _pageDic[cfg.Page];
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ID.Equals(itemID))
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
            coinsChange = DoUseGarbage(dustbinType, cfg.ItemType, count);
            UIMod.Inst.ShowTipsUI<UICoinChanged>(UIDef.UI_COINCHANGED, coinsChange);
            _pageDirt[cfg.Page] = true;
            BagEvent.OnItemChanged?.Invoke(cfg.Page);
        }
        return coinsChange > 0;
    }
    private int DoUseGarbage(int dustbinType, int itemType, int count)
    {
        if (dustbinType == itemType)
            return PlayerData.Inst.AddCoinsByItemCount(count);
        else
            return PlayerData.Inst.DeleteCoinsByItemCount(count);

    }
    /// <summary>
    /// 脏标记时排序
    /// </summary>
    public void SortItem(int page)
    {
        if (_pageDirt.ContainsKey(page) && _pageDirt[page])
        {
            var list = _pageDic[page];
            list.Sort((x, y) => y.Count.CompareTo(x.Count));
            _pageDirt[page] = false;
        }
    }
}
