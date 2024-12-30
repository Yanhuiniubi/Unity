using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopData
{
    public static ShopData Inst = new ShopData();
    private Dictionary<int, List<TableItemShop>> _dic;//page -- shopitemList
    private ShopData()
    {
        _dic = new Dictionary<int, List<TableItemShop>>();
    }
    private void InitData()
    {
        var arr = TableItemShopMod.Array;
        int length = arr.Length;
        for (int i = 0; i < length; i++)
        {
            if (_dic.ContainsKey(arr[i].Page))
                _dic[arr[i].Page].Add(arr[i]);
            else
                _dic[arr[i].Page] = new List<TableItemShop>() { arr[i] };
        }
    }       
    public List<TableItemShop> GetShopItemListByPage(int page)
    {
        if (_dic.Count == 0)
            InitData();
        if (_dic.ContainsKey(page))
            return _dic[page];
        _dic[page] = new List<TableItemShop>();
        return _dic[page];
    }
    public bool ReqBuyShopItem(TableItemShop item,int count)
    {
        bool isSuccess = PlayerData.Inst.DeleteCoinsByTotalPrice(item.Price * count);
        if (isSuccess)
        {
            BagData.Inst.AddItem(TableItemMainMod.Get(item.ItemID), count);
            UIMod.Inst.ShowTipsUI<UICoinChanged>(UIDef.UI_COINCHANGED, -item.Price * count);
        }
        return isSuccess;
    }
}
