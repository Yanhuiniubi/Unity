using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[UIBind(UIDef.UI_MAIN)]
public class UIMain : UILogicBase
{
    private TextMeshProUGUI _coinsCount;
    private TextMeshProUGUI _garbageCount;
    private TextMeshProUGUI _taskDesc;
    private Animation _taskAni;
    public override void OnHide()
    {
        base.OnHide();
        PlayerEvent.OnCoinsChanged -= SetCoins;
        PlayerEvent.OnGarbageCntChanged -= SetGarbageCount;
        TaskEvent.OnTaskStateUpdate -= OnTaskStateUpdate;
        TaskEvent.OnTaskFinish -= OnTaskFinish;
        TaskEvent.OnTaskFailure -= OnTaskFailure;
    }

    public override void OnInit()
    {
        base.OnInit();
        _coinsCount = GetUIComponentInchildren<TextMeshProUGUI>("PlayerInfo/Kuang/CoinIcon/Bg/CoinsCount");
        _garbageCount = GetUIComponentInchildren<TextMeshProUGUI>("PlayerInfo/Kuang/BgGarbageCount/TxtGarbageCount");
        _taskDesc = GetUIComponentInchildren<TextMeshProUGUI>("TaskArea/Bg/TaskDesc");
        _taskAni = GetUIComponentInchildren<Animation>("TaskArea/Bg");
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        PlayerEvent.OnCoinsChanged += SetCoins;
        PlayerEvent.OnGarbageCntChanged += SetGarbageCount;
        TaskEvent.OnTaskStateUpdate += OnTaskStateUpdate;
        TaskEvent.OnTaskFinish += OnTaskFinish;
        TaskEvent.OnTaskFailure += OnTaskFailure;
        SetCoins();
        SetGarbageCount();
    }
    private void SetCoins()
    {
        _coinsCount.text = PlayerData.Inst.Coins.ToString();
    }
    private void SetGarbageCount()
    {
        _garbageCount.text = $"剩余垃圾数量：{GarbageData.Inst.GarbageCount.ToString().ParseColorText("FD17F5")}";
    }
    private void OnTaskStateUpdate()
    {
        var data = TaskData.Inst.CurTask;
        if (data == null)
        {
            _taskDesc.text = "已完成所有任务！";
        }
        else
        {
            eTaskType type = (eTaskType)data.TaskType;
            switch (type)
            {
                case eTaskType.PickUpGarbage:
                case eTaskType.Plant:
                    TableItemMain item = TableItemMainMod.Get(data.Param1);
                    _taskDesc.text = data.Desc.Replace("{0}", item.Name);
                    break;
                case eTaskType.StopCutting:
                    break;
            }
            _taskDesc.text += $"({TaskData.Inst.Process.ToString().ParseColorText("000000")}/" +
                $"{data.Count})";
        }
    }
    private void OnTaskFinish()
    {
        OnTaskStateUpdate();
        _taskAni.Play();
    }
    private void OnTaskFailure()
    {

    }
}
