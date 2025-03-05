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
                            new Content() { role = "system", content = "你的环境保护知识相当丰富，需要你设计一些环境保护相关的单选题" },
                            new Content() { role = "user", content = "请随机给出一个关于环境保护之类的" +
            "选择题，其他多余的话不要有，我希望你能尽量发挥想象，只需要给出题干，ABCD四个选项、以及正确答案,例如： 题干：树木在环境保护中的作用是什么？\r\nA. 提供氧气\r\nB. 吸收二氧化碳\r\nC. 防止水土流失\r\nD. 以上都是，答案：D" },
                            new Content() { role = "assistant", content = AIChatData.Inst.Question },
                        }, "请随机给出一个关于环境保护之类的" +
            "选择题，其他多余的话不要有，我希望你能尽量发挥想象，只需要给出题干，ABCD四个选项、以及正确答案,例如： 题干：树木在环境保护中的作用是什么？\r\nA. 提供氧气\r\nB. 吸收二氧化碳\r\nC. 防止水土流失\r\nD. 以上都是，答案：D", 0.99f);
        var bt = _logger?.GetComponent<BehaviorTree>();
        bt?.DisableBehavior(true);
    }
    private void OnOKBtnClick()
    {
        if (!_inAsk)
        {
            _content.text = _question.Substring(0, _question.IndexOf("答案") - 1);
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
