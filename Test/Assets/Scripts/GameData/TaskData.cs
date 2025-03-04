using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTaskType
{
    PickUpGarbage = 1,//¼ñÀ¬»ø
    RecycleGarbage = 2,//»ØÊÕÀ¬»ø
    Plant = 3,//ÖÖÖ²
    StopCutting = 4,//Í£Ö¹¿³Ê÷
}
public class TaskEvent
{
    public static Action OnTaskStateUpdate;
    public static Action OnTaskFinish;
    public static Action OnTaskFailure;
}
public class TaskData
{
    public static readonly TaskData Inst = new TaskData();
    private TaskData()
    {

    }
    private TableMainTask _curTask;
    public TableMainTask CurTask => _curTask;
    private int _process;
    public int Process => _process;
    public int Chapter { private set; get; }
    public void RefreshCurTask(int id = 16)
    {
        _process = 0;
        _curTask = TableMainTaskMod.Get(id);
        Chapter = _curTask.TaskType;
        eTaskType type = (eTaskType)_curTask.TaskType;
        if (type == eTaskType.PickUpGarbage)
            GarbageGenerateManager.Inst.GenerateGarbageFromTask();
        else if (type == eTaskType.StopCutting)
            CuttingMod.Inst.GenerateLogger();
        TaskEvent.OnTaskStateUpdate?.Invoke();
    }
    public void CheckTask(string param,object param1 = null)
    {
        if (param.Equals(_curTask.Param1))
        {
            DoTask(param1);
        }
    }
    private void DoTask(object param)
    {
        eTaskType type = (eTaskType)_curTask.TaskType;
        switch (type)
        {
            case eTaskType.StopCutting:
            case eTaskType.PickUpGarbage:
            case eTaskType.Plant:
                if (++_process == _curTask.Count)
                {
                    RefreshCurTask(_curTask.ID + 1);
                    TaskEvent.OnTaskFinish?.Invoke();
                }
                else
                    TaskEvent.OnTaskStateUpdate?.Invoke();
                break;
            case eTaskType.RecycleGarbage:
                RecycleGarbageInfo info = param as RecycleGarbageInfo;
                bool result = info.Result;
                if (result)
                {
                    _process += info.Count;
                    if (_process >= _curTask.Count)
                    {
                        RefreshCurTask(_curTask.ID + 1);
                        TaskEvent.OnTaskFinish?.Invoke();
                    }
                    else
                        TaskEvent.OnTaskStateUpdate?.Invoke();
                }
                else
                {
                    _process = 0;
                    TaskEvent.OnTaskFailure?.Invoke();
                }
                break;
        }
    }
}
