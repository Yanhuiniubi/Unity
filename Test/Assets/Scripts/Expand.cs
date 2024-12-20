using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// À©Õ¹·½·¨
/// </summary>
public static class Expand
{
    public static string ParseColorText(this string txt,string color)
    {
        if (string.IsNullOrEmpty(color))
            return txt;
        return string.Format("<color=#{0}>{1}</color>", color, txt);
    }
}
