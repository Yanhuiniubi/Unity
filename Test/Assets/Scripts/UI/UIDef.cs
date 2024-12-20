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
    public const string UI_INTRODUTION = "Prefab/IntroDution";
    public const string UI_BAGITEM = "Prefab/BagItem";
    public const string UI_UIBAG = "Prefab/UI-Bag";
    public const string UI_UIITEMDESC = "Prefab/UI-ItemDesc";
    public const string UI_UIITEMUSE = "Prefab/UI-ItemUse";
    public const string UI_MAIN = "Prefab/UI-Main";
}
