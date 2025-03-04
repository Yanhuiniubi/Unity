using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BehaviorDesigner.Runtime;

public class TreeInfo
{
    public GameObject Obj;
    public bool IsTarget;

    public TreeInfo(GameObject obj, bool isTarget)
    {
        Obj = obj;
        IsTarget = isTarget;
    }
}
public class CuttingMod
{
    public static readonly CuttingMod Inst = new CuttingMod();
    private HashSet<GameObject> loggers = new HashSet<GameObject>();
    private HashSet<TreeInfo> _trees = new HashSet<TreeInfo>();
    private CuttingMod()
    {

    }
    public Transform GetTargetTree(Vector3 originPos)
    {
        float minDistance = float.MaxValue;
        Transform target = null;
        foreach (var t in _trees) 
        {
            if (t.IsTarget)
                continue;
            float distance = Vector3.Distance(originPos, t.Obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                t.IsTarget = true;
                target = t.Obj.transform;
            }
        }
        return target;
    }
    public void DeleteTree(GameObject tree)
    {
        TreeInfo info = null;
        foreach (var item in _trees)
        {
            if (tree == item.Obj)
            {
                info = item;
                break;
            }
        }
        _trees.Remove(info);
    }
    public void GenerateLogger()
    {
        ResData.Inst.GetResByAddress("Logger.prefab", (GameObject logger) =>
        {
            var trees = GameObject.FindGameObjectsWithTag("Plant");
            if (trees != null && trees.Length != 0)
                foreach (var t in trees) { _trees.Add(new TreeInfo(t,false)); }
            GenerateLoggerFromTask(logger);
        });
    }
    private void GenerateLoggerFromTask(GameObject logger)
    {
        var data = TaskData.Inst.CurTask;
        var count = data.Count;
        var groundObject = GameMod.Inst.GarbageRoundBox.gameObject;
        MeshRenderer meshRenderer = groundObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Bounds bounds = meshRenderer.bounds;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            LayerMask _layerMask = GameMod.Inst._groundMask;

            for (int i = 0; i < count; i++)
            {
                float x = UnityEngine.Random.Range(min.x, max.x);
                float z = UnityEngine.Random.Range(min.z, max.z);
                Vector3 origin = new Vector3(x, bounds.max.y + 1, z); // 在地面最高点上方发射射线
                RaycastHit hit;

                if (Physics.Raycast(origin, Vector3.down, out hit, maxDistance: 1000, layerMask: _layerMask))
                {
                    Vector3 spawnPosition = hit.point;
                    GameObject obj = GameObject.Instantiate<GameObject>(logger, GameMod.Inst.LoggerRoot);
                    obj.transform.position = spawnPosition + new Vector3(0f, 2f, 0f);
                    loggers.Add(obj);
                    var bt = obj.GetComponent<BehaviorTree>();
                    bt.SendEvent("StartCutting");
                }
            }
        }
        else
        {
            Debug.LogError("地面物体上找不到 MeshRenderer 组件！");
        }
    }
}
