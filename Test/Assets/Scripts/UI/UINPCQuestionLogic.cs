using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UIBind(UIDef.UI_NPCQUESTION)]
public class UINPCQuestionLogic : UINPCQuestionBase
{
    public static Action<bool, string> OnLoggerPause;
    private UIContainer<Option> _options;
    private bool _inAsk;
    public override void OnHide()
    {
        base.OnHide();
        AIChatEvent.OnResponseEnd -= OnQuestionGenerated;
        OnLoggerPause?.Invoke(false, _logger.name);
    }

    public override void OnInit()
    {
        base.OnInit();
        _options = new UIContainer<Option>(gameObject.transform.Find("ScrollViewPage/GridPage").gameObject);
        e_CloseBtn.AddClickEvent(() => {
            if (e_OKBtn.gameObject.activeSelf)
                UIMod.Inst.HideUI();
            });
        e_OKBtn.AddClickEvent(OnOKBtnClick);
    }
    private GameObject _logger;
    public override void OnShow(object param)
    {
        base.OnShow(param);
        AIChatEvent.OnResponseEnd += OnQuestionGenerated;
        _logger = param as GameObject;
        _options.Ensuresize(0);
        _inAsk = false;
        e_OKBtn.gameObject.SetActive(false);
        e_Content.Text = "��ð�Сʹ�ߣ�������ܻش�����ҵ����⣬�ҾͲ������������ҵ���˼��˼��~";
        AIChatData.Inst.AddQuestion("���������һ�����ڻ�������֮���" +
            "ѡ���⣬��������Ļ���Ҫ�У���ϣ�����ܾ�����������ֻ��Ҫ������ɣ�ABCD�ĸ�ѡ��Լ���ȷ��,���磺 ��ɣ���ľ�ڻ��������е�������ʲô��\r\nA. �ṩ����\r\nB. ���ն�����̼\r\nC. ��ֹˮ����ʧ\r\nD. ���϶��ǣ��𰸣�D");
        AIAssistant.Inst.SendQuestion(AIChatData.Inst.Questions, "���������һ�����ڻ�������֮���" +
            "ѡ���⣬��������Ļ���Ҫ�У���ϣ�����ܾ�����������ֻ��Ҫ������ɣ�ABCD�ĸ�ѡ��Լ���ȷ��,���磺 ��ɣ���ľ�ڻ��������е�������ʲô��\r\nA. �ṩ����\r\nB. ���ն�����̼\r\nC. ��ֹˮ����ʧ\r\nD. ���϶��ǣ��𰸣�D"
            ,1f);
        OnLoggerPause?.Invoke(true, _logger.name);
    }
    private void OnOKBtnClick()
    {
        string _question = AIChatData.Inst.Questions[AIChatData.Inst.Questions.Count - 1].content;
        if (!_inAsk)
        {
            e_Content.Text = _question.Substring(0, _question.IndexOf("��") - 1);
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
                        CuttingMod.Inst.DeleteLoggers(_logger);
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
    private void OnQuestionGenerated()
    {
        e_OKBtn.gameObject.SetActive(true);
    }
}

public class Option : UINPCQuestionContentBase
{
    public bool IsSelected => e_Toggle.IsOn;
    public string Content => e_Text.Text;
    protected override void OnInit()
    {
        base.OnInit();
    }
    public void SetData(int number)
    {
        e_Text.Text = ((char)(number + 'A')).ToString();
    }
}
