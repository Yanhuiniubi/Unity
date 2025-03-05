using BehaviorDesigner.Runtime;
using Palmmedia.ReportGenerator.Core.Logging;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

[UIBind(UIDef.UI_NPCQUESTION)]
public class UINPCQuestionLogic : UILogicBase
{
    private Button _closeBtn;
    private Button _OkBtn;
    private TextMeshProUGUI _content;
    private UIContainer<Option> _options;
    private bool _inAsk;
    public override void OnHide()
    {
        base.OnHide();
        AIChatEvent.OnResponseEnd -= OnQuestionGenerated;
        var bt = _logger?.GetComponent<BehaviorTree>();
        bt?.EnableBehavior();
    }

    public override void OnInit()
    {
        base.OnInit();
        _closeBtn = GetUIComponentInchildren<Button>("Bg_transparent/Bg/CloseBtn");
        _OkBtn = GetUIComponentInchildren<Button>("OKBtn");
        _content = GetUIComponentInchildren<TextMeshProUGUI>("Bg_transparent/Bg/Content");
        _options = new UIContainer<Option>(gameObject.transform.Find("ScrollViewPage/GridPage").gameObject);
        _closeBtn.onClick.AddListener(() => UIMod.Inst.HideUI());
        _OkBtn.onClick.AddListener(OnOKBtnClick);
    }
    private GameObject _logger;
    public override void OnShow(object param)
    {
        base.OnShow(param);
        AIChatEvent.OnResponseEnd += OnQuestionGenerated;
        _logger = param as GameObject;
        _inAsk = false;
        _OkBtn.gameObject.SetActive(false);
        AIAssistant.Inst.SendQuestion(
                    new List<Content>
                        {
                            new Content() { role = "system", content = "��Ļ�������֪ʶ�൱�ḻ����Ҫ�����һЩ����������صĵ�ѡ��" },
                            new Content() { role = "user", content = "���������һ�����ڻ�������֮���" +
            "ѡ���⣬��������Ļ���Ҫ�У���ϣ�����ܾ�����������ֻ��Ҫ������ɣ�ABCD�ĸ�ѡ��Լ���ȷ��,���磺 ��ɣ���ľ�ڻ��������е�������ʲô��\r\nA. �ṩ����\r\nB. ���ն�����̼\r\nC. ��ֹˮ����ʧ\r\nD. ���϶��ǣ��𰸣�D" },
                            new Content() { role = "assistant", content = AIChatData.Inst.Question },
                        }, "���������һ�����ڻ�������֮���" +
            "ѡ���⣬��������Ļ���Ҫ�У���ϣ�����ܾ�����������ֻ��Ҫ������ɣ�ABCD�ĸ�ѡ��Լ���ȷ��,���磺 ��ɣ���ľ�ڻ��������е�������ʲô��\r\nA. �ṩ����\r\nB. ���ն�����̼\r\nC. ��ֹˮ����ʧ\r\nD. ���϶��ǣ��𰸣�D", 0.99f);
        var bt = _logger?.GetComponent<BehaviorTree>();
        bt?.DisableBehavior(true);
    }
    private void OnOKBtnClick()
    {
        if (!_inAsk)
        {
            _content.text = _question.Substring(0, _question.IndexOf("��") - 1);
            _options.Ensuresize(4);
            var children = _options.Children;
            int count = children.Count;
            for (int i = 0; i < count; i++)
            {
                children[i].SetData(i);
            }
            _inAsk = true;
        }
        else
        {
            string result = "";
            var options = _options.Children;
            foreach (var item in options)
            {
                if (item.IsSelected)
                {
                    result = item.Content;
                    break;
                }
            }
            if (string.IsNullOrEmpty(result))
                return;
            for (int i = _question.Length - 1; i >= 0; i--)
            {
                if (char.IsLetter(_question[i]))
                {
                    UIMod.Inst.HideUI();
                    if (result.Equals(_question[i].ToString()))//�ش���ȷ
                    {
                        TaskData.Inst.CheckTask("Stop");
                        GameObject.Destroy(_logger);
                    }
                    else//�ش����
                    {
                        UIMod.Inst.ShowUI<UITaskFailure>(UIDef.UI_TASKFAILURE, "�ش������ֹ����ʧ��");
                    }
                    break;
                }
            }
        }
    }
    private string _question;
    private void OnQuestionGenerated()
    {
        _question = AIChatData.Inst.Question;
        _OkBtn.gameObject.SetActive(true);
    }
}

public class Option : UITemplateBase
{
    private Toggle _toggle;
    private TextMeshProUGUI _content;
    public bool IsSelected => _toggle.isOn;
    public string Content => _content.text;
    public override void OnInit()
    {
        base.OnInit();
        _toggle = GetUIComponentInchildren<Toggle>("Toggle");
        _content = GetUIComponentInchildren<TextMeshProUGUI>("Toggle/Background/Text (TMP)");
    }
    public void SetData(int number)
    {
        _content.text = ((char)(number + 'A')).ToString();
    }
}
