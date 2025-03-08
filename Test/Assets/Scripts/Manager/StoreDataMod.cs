using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDataMod
{
    public static readonly StoreDataMod Inst = new StoreDataMod();
    private StoreDataMod()
    {
        
    }
    public void StoreData()
    {
        UIMod.Inst.ShowUI<UIStoreData>(UIDef.UI_StoreData, "���ڴ洢���������Եȡ���");
        BagData.Inst.OnStoreData();
        TaskData.Inst.OnStoreData();
        AIChatData.Inst.OnStoreData();
        PlayerData.Inst.OnStoreData();
    }
    public void LoadData()
    {
        BagData.Inst.OnReadData();
        TaskData.Inst.OnReadData();
        AIChatData.Inst.OnReadData();
        PlayerData.Inst.OnReadData();
    }
}
