using BehaviorDesigner.Runtime.Tasks;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAIChatLogic : UIAIChatBase
{
    private UITileLoop<ChatTemplate> _chatContent;
    private List<ChatInfo> _chatInfos;
    RectTransform _rect;
    public override void OnInit()
    {
        base.OnInit();
        _rect = GetUIComponentInchildren<RectTransform>("e_Scroll View/Grid");
        _chatContent = new UITileLoop<ChatTemplate>(gameObject.transform.Find("e_Scroll View/Grid").gameObject, e_ScrollView);
        e_CloseBtn.onClick.AddListener(() =>
        {
            if (!_AIResponsing) 
                UIMod.Inst.HideUI();
        });
        e_SendBtn.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(e_InputField.text) && !_AIResponsing)
            {
                AIAssistant.Inst.SendQuestion(
                    new List<Content>
                        {
                            new Content() { role = "user", content = e_InputField.text },
                             // new Content() { role = "assistant", content = "....." }, // AI的历史回答结果，这里省略了具体内容，可以根据需要添加更多历史对话信息和最新问题的内容。
                        }, e_InputField.text);
            }
        });
        _chatInfos = AIChatData.Inst.ChatInfos;

        _chatContent.OnUpdateItem = null;
        _chatContent.OnUpdateItem += OnUpdateItem;
    }

    private void OnUpdateItem(int index, ChatTemplate template)
    {
        template.SetData(_chatInfos[index]);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
    }
    public override void OnShow(object param)
    {
        base.OnShow(param);
        AIChatEvent.OnRequest += AddOrUpdateNewMessage;
        AIChatEvent.OnResponse += AddOrUpdateNewMessage;
        AIChatEvent.OnResponseEnd += SetWaitPanel;
        SetChatView();
    }
    private void SetChatView()
    {
        var chatInfos = AIChatData.Inst.ChatInfos;
        int cnt = chatInfos.Count;
        _chatContent.Ensuresize(cnt);
    }
    private bool _AIResponsing;
    private void AddOrUpdateNewMessage(bool isSelf)
    {
        if (isSelf)
        {
            _AIResponsing = true;
            e_InputField.text = string.Empty;
            e_WaitBg.gameObject.SetActive(true);
        }
        var chatInfos = AIChatData.Inst.ChatInfos;
        int cnt = chatInfos.Count;
        _chatContent.Ensuresize(cnt);
    }
    private void SetWaitPanel()
    {
        e_WaitBg.gameObject.SetActive(false);
        _AIResponsing = false;
    }
    public override void OnHide()
    {
        base.OnHide();
        AIChatEvent.OnRequest -= AddOrUpdateNewMessage;
        AIChatEvent.OnResponse -= AddOrUpdateNewMessage;
        AIChatEvent.OnResponseEnd -= SetWaitPanel;
        e_InputField.text = string.Empty;
    }
}
public class ChatTemplate : UIAIChatContentBase
{
    public override void OnInit()
    {
        base.OnInit();
    }
    public void SetData(ChatInfo info)
    {
        if (info.IsSelf)
        {
            e_Icon.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("PLayer.png");
            e_ImgBG.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("Button Green.png");
        }
        else
        {
            e_Icon.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("Robot.png");
            e_ImgBG.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("Button Blue.png");
        }
        e_TxtContent.text = info.Content;
    }
}
