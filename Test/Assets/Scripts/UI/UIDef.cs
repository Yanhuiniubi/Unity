using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class UIBindAttribute : Attribute
{
    public UIBindAttribute(string uiname)
    {

    }
}
public class UIDef
{
    public const string UI_INTRODUTION = "IntroDution.prefab";
    public const string UI_BAGITEM = "BagItem.prefab";
    public const string UI_UIBAG = "UI-Bag.prefab";
    public const string UI_UIITEMDESC = "UI-ItemDesc.prefab";
    public const string UI_UIITEMUSE = "UI-ItemUse.prefab";
    public const string UI_MAIN = "UI-Main.prefab";
    public const string UI_ITEMUSERESULT = "UI-ItemUseResult.prefab";
    public const string UI_COINCHANGED = "UI-CoinChanged.prefab";
    public const string UI_SHOP = "UI-Shop.prefab";
    public const string UI_LOADING = "UI-Loading.prefab";
    public const string UI_GAMEINTRODUTION = "UI-GameIntrodution.prefab";
    public const string UI_AICHAT = "UI-AIChat.prefab";
    public const string UI_PLANTUSE = "UI-PlantUse.prefab";
    public const string UI_TASKFAILURE = "UI-TaskFailure.prefab";
    public const string UI_NPCQUESTION = "UI-NPCQuestion.prefab";
    public const string UI_StoreData = "UI-StoreData.prefab";
    public const string UI_Guide = "UI-Guide.prefab";
    public const string UI_AICHATTEST = "UI-AIChatTest.prefab";
}
