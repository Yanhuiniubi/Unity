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
        e_CloseBtn.onClick.AddListener(() => {
            if (e_OKBtn.gameObject.activeSelf)
                UIMod.Inst.HideUI();
            });
        e_OKBtn.onClick.AddListener(OnOKBtnClick);
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
        e_Content.text = "你好啊小使者，如果你能回答出来我的问题，我就不砍树！不过我得先思考思考~";
        AIChatData.Inst.AddQuestion("请随机给出一个关于环境保护之类的" +
            "选择题，其他多余的话不要有，我希望你能尽量发挥想象，只需要给出题干，ABCD四个选项、以及正确答案,例如： 题干：树木在环境保护中的作用是什么？\r\nA. 提供氧气\r\nB. 吸收二氧化碳\r\nC. 防止水土流失\r\nD. 以上都是，答案：D");
        AIAssistant.Inst.SendQuestion(AIChatData.Inst.Questions, "请随机给出一个关于环境保护之类的" +
            "选择题，其他多余的话不要有，我希望你能尽量发挥想象，只需要给出题干，ABCD四个选项、以及正确答案,例如： 题干：树木在环境保护中的作用是什么？\r\nA. 提供氧气\r\nB. 吸收二氧化碳\r\nC. 防止水土流失\r\nD. 以上都是，答案：D"
            ,1f);
        OnLoggerPause?.Invoke(true, _logger.name);
    }
    private void OnOKBtnClick()
    {
        string _question = AIChatData.Inst.Questions[AIChatData.Inst.Questions.Count - 1].content;
        if (!_inAsk)
        {
            e_Content.text = _question.Substring(0, _question.IndexOf("答案") - 1);
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
                    if (result.Equals(_question[i].ToString()))//回答正确
                    {
                        TaskData.Inst.CheckTask("Stop");
                        CuttingMod.Inst.DeleteLoggers(_logger);
                        GameObject.Destroy(_logger);
                    }
                    else//回答错误
                    {
                        UIMod.Inst.ShowUI<UITaskFailure>(UIDef.UI_TASKFAILURE, "回答错误，阻止砍树失败");
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
    public bool IsSelected => e_Toggle.isOn;
    public string Content => e_Text.text;
    public override void OnInit()
    {
        base.OnInit();
    }
    public void SetData(int number)
    {
        e_Text.text = ((char)(number + 'A')).ToString();
    }
}
