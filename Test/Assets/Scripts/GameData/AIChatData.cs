using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public static class AIChatEvent
{
    /// <summary>
    /// ai回复 内容--是否是更新
    /// </summary>
    public static Action<bool> OnResponse;
    public static Action OnResponseEnd;
    public static Action<bool> OnRequest;
}
[Serializable]
public class ChatInfo
{
    public bool IsSelf;
    public string Content;
}
[Serializable]
public class AIDataStore
{
    public List<ChatInfo> ChatInfos;
    public List<Content> Contents;
}
public class AIChatData
{
    public static readonly AIChatData Inst = new AIChatData();
    private List<ChatInfo> _chatInfos;
    public List<ChatInfo> ChatInfos => _chatInfos;
    private AIChatData()
    {
        _chatInfos = new List<ChatInfo>();
    }
    public List<Content> Questions = new List<Content>() { new Content()
        {
            role = "system", content = "你的环境保护知识相当丰富，需要你设计一些环境保护相关的单选题"
        } 
    };
    public void OnStoreData()
    {
        AIDataStore infos = new AIDataStore();
        infos.ChatInfos = new List<ChatInfo>();
        infos.Contents = new List<Content>();
        for (int i = 0;i < _chatInfos.Count; i++)
        {
            infos.ChatInfos.Add(_chatInfos[i]);
        }
        for (int i = 0; i < Questions.Count; i++)
        {
            infos.Contents.Add(Questions[i]);
        }
        string json = JsonUtility.ToJson(infos, true);
        string path = Path.Combine(Application.streamingAssetsPath, "AIChatData.json");
        File.WriteAllText(path, json);
    }
    public void OnReadData()
    {
        // 获取文件路径
        string path = Path.Combine(Application.streamingAssetsPath, "AIChatData.json");
        // 读取文件内容
        string json = "";
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
        else
        {
            Debug.LogError("File not found: " + path);
            return;
        }
        // 反序列化JSON数据
        AIDataStore data = JsonUtility.FromJson<AIDataStore>(json);
        _chatInfos = data.ChatInfos;
        Questions = data.Contents;
    }
    public void RecvData(bool IsSelf,string Content,bool isUpdate)
    {
        if (UIMod.Inst.IsActiveUI(UIDef.UI_AICHAT))
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
        else if (UIMod.Inst.IsActiveUI(UIDef.UI_NPCQUESTION))
        {
            if (!IsSelf)
            {
                if (isUpdate)
                {
                    int count = Questions.Count;
                    Questions[count - 1].content = Content;
                }
                else
                {
                    Content info = new Content();
                    info.role = "assistant";
                    info.content = Content;
                    Questions.Add(info);
                }
            }
        }
    }
    public void AddQuestion(string question)
    {
        Content info = new Content();
        info.role = "user";
        info.content = question;
        Questions.Add(info);
    }
}
