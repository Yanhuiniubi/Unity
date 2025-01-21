using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public static class AIChatEvent
{
    /// <summary>
    /// ai回复 内容--是否是更新
    /// </summary>
    public static Action<bool> OnResponse;
    public static Action OnResponseEnd;
    public static Action<bool> OnRequest;

}

public class ChatInfo
{
    public bool IsSelf;
    public string Content;
}
public class AIChatData
{
    public static AIChatData Inst = new AIChatData();
    private List<ChatInfo> _chatInfos;
    public List<ChatInfo> ChatInfos => _chatInfos;
    private AIChatData()
    {
        _chatInfos = new List<ChatInfo>();
    }
    public void RecvData(bool IsSelf,string Content,bool isUpdate)
    {
        if (isUpdate)
        {
            int count = _chatInfos.Count;
            _chatInfos[count - 1].Content = Content;
        }
        else
        {
            ChatInfo info = new ChatInfo();
            info.IsSelf = IsSelf;
            info.Content = Content;
            _chatInfos.Add(info);
        }
        if (IsSelf)
            AIChatEvent.OnRequest?.Invoke(true);
        else
            AIChatEvent.OnResponse?.Invoke(false);
    }
}
