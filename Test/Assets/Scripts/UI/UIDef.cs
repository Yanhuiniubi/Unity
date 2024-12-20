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
    public const string UI_INTRODUTION = "Prefab/UI/IntroDution";
    public const string UI_BAGITEM = "Prefab/UI/BagItem";
    public const string UI_UIBAG = "Prefab/UI/UI-Bag";
    public const string UI_UIITEMDESC = "Prefab/UI/UI-ItemDesc";
    public const string UI_UIITEMUSE = "Prefab/UI/UI-ItemUse";
    public const string UI_MAIN = "Prefab/UI/UI-Main";
    public const string UI_ITEMUSERESULT = "Prefab/UI/UI-ItemUseResult";
}
