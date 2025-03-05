using BehaviorDesigner.Runtime.Tasks;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAIChatLogic : UILogicBase
{
    private Button _closeBtn;
    private Button _requestBtn;
    private TMP_InputField _InputField;
    private UIContainer<ChatTemplate> _chatContent;
    private Image _wait;

    RectTransform _rect;
    public override void OnInit()
    {
        base.OnInit();
        _rect = GetUIComponentInchildren<RectTransform>("Scroll View/Grid");
        _closeBtn = GetUIComponentInchildren<Button>("CloseBtn");
        _requestBtn = GetUIComponentInchildren<Button>("SendBtn");
        _InputField = GetUIComponentInchildren<TMP_InputField>("Input/InputField");
        _wait = GetUIComponentInchildren<Image>("WaitBg");
        _chatContent = new UIContainer<ChatTemplate>(gameObject.transform.Find("Scroll View/Grid").gameObject);
        _closeBtn.onClick.AddListener(() =>
        {
            if (!_AIResponsing) 
                UIMod.Inst.HideUI();
        });
        _requestBtn.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(_InputField.text) && !_AIResponsing)
            {
                AIAssistant.Inst.SendQuestion(
                    new List<Content>
                        {
                            new Content() { role = "user", content = _InputField.text },
                             // new Content() { role = "assistant", content = "....." }, // AI的历史回答结果，这里省略了具体内容，可以根据需要添加更多历史对话信息和最新问题的内容。
                        }, _InputField.text);
            }
        });
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
        var children = _chatContent.Children;
        for (int i = 0;i < cnt;i++)
        {
            children[i].SetData(chatInfos[i]);
        }
    }
    private bool _AIResponsing;
    private void AddOrUpdateNewMessage(bool isSelf)
    {
        if (isSelf)
        {
            _AIResponsing = true;
            _InputField.text = string.Empty;
            _wait.gameObject.SetActive(true);
        }
        var chatInfos = AIChatData.Inst.ChatInfos;
        int cnt = chatInfos.Count;
        _chatContent.Ensuresize(cnt);
        var children = _chatContent.Children;
        children[cnt - 1].SetData(chatInfos[cnt - 1]);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
    }
    private void SetWaitPanel()
    {
        _wait.gameObject.SetActive(false);
        _AIResponsing = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
    }
    public override void OnHide()
    {
        base.OnHide();
        AIChatEvent.OnRequest -= AddOrUpdateNewMessage;
        AIChatEvent.OnResponse -= AddOrUpdateNewMessage;
        AIChatEvent.OnResponseEnd -= SetWaitPanel;
        _InputField.text = string.Empty;
    }
}
public class ChatTemplate : UITemplateBase
{
    private Image _icon;
    private Image _bg;
    private TextMeshProUGUI _content;
    public override void OnInit()
    {
        base.OnInit();
        _icon = GetUIComponentInchildren<Image>("ChatRootLeft/MaxWidth/Icon");
        _content = GetUIComponentInchildren<TextMeshProUGUI>("ChatRootLeft/MaxWidth/Content/" +
            "ImgBG/TxtContent");
        _bg = GetUIComponentInchildren<Image>("ChatRootLeft/MaxWidth/Content/" +
            "ImgBG");
    }
    public void SetData(ChatInfo info)
    {
        if (info.IsSelf)
        {
            _icon.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("PLayer.png");
            _bg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("Button Green.png");
        }
        else
        {
            _icon.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("Robot.png");
            _bg.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>("Button Blue.png");
        }
        _content.text = info.Content;
    }
}
