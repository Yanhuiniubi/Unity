using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum eTaskType
{
    PickUpGarbage = 1,//捡垃圾
    RecycleGarbage = 2,//回收垃圾
    Plant = 3,//种植
    StopCutting = 4,//停止砍树
}
public class TaskEvent
{
    public static Action OnTaskStateUpdate;
    public static Action OnTaskFinish;
    public static Action OnTaskFailure;
}

[Serializable]
public class TaskStoreData
{
    public int TaskID;
    public int Process;
    public List<Vector3> Pos;
    public List<string> CuttingTrees;
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
    public void OnStoreData()
    {
        TaskStoreData info = new TaskStoreData();
        info.TaskID = _curTask != null ? _curTask.ID : int.MaxValue;
        info.Process = _process;
        if (_curTask != null)
        {
            if ((eTaskType)_curTask.TaskType == eTaskType.PickUpGarbage)
                info.Pos = GarbageData.Inst.GetAllGarbagePos();
            else if ((eTaskType)_curTask.TaskType == eTaskType.StopCutting)
            {
                info.Pos = CuttingMod.Inst.GetAllLoggerPos();
                info.CuttingTrees = CuttingMod.Inst.GetAllCuttingTrees();
            }
        }
        string json = JsonUtility.ToJson(info, true);
        string path = Path.Combine(Application.streamingAssetsPath, "TaskData.json");
        File.WriteAllText(path, json);
    }
    public void OnReadData()
    {
        // 获取文件路径
        string path = Path.Combine(Application.streamingAssetsPath, "TaskData.json");
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
        TaskStoreData data = JsonUtility.FromJson<TaskStoreData>(json);
        _process = data.Process;
        if (data.CuttingTrees != null)
        {
            int count = data.CuttingTrees.Count;
            for (int i = 0;i < count;i++)
            {
                var obj = GameObject.Find(data.CuttingTrees[i]);
                GameObject.DestroyImmediate(obj);
            }
        }
        InitCurTask(data.TaskID, data.Pos);
    }
    public void InitCurTask(int id = 17, List<Vector3> pos = null)
    {
        _curTask = TableMainTaskMod.Get(id);
        if (_curTask != null)
        {
            Chapter = _curTask.TaskType;
            eTaskType type = (eTaskType)_curTask.TaskType;
            if (type == eTaskType.PickUpGarbage)
                GarbageGenerateManager.Inst.GenerateGarbageFromTask(pos);
            else if (type == eTaskType.StopCutting)
                CuttingMod.Inst.GenerateLogger(pos);
        }
        TaskEvent.OnTaskStateUpdate?.Invoke();
    }
    public void RefreshCurTask(int id)
    {
        _process = 0;
        _curTask = TableMainTaskMod.Get(id);
        if (_curTask != null)
        {
            Chapter = _curTask.TaskType;
            eTaskType type = (eTaskType)_curTask.TaskType;
            if (type == eTaskType.PickUpGarbage)
                GarbageGenerateManager.Inst.GenerateGarbageFromTask();
            else if (type == eTaskType.StopCutting)
                CuttingMod.Inst.GenerateLogger(null);
        }
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
