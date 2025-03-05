using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BehaviorDesigner.Runtime;

public class CuttingMod
{
    public static readonly CuttingMod Inst = new CuttingMod();
    private HashSet<GameObject> loggers = new HashSet<GameObject>();
    private Dictionary<GameObject,bool> _trees = new Dictionary<GameObject, bool>();
    private CuttingMod()
    {

    }
    public Transform GetTargetTree(Vector3 originPos)
    {
        float minDistance = float.MaxValue;
        Transform target = null;
        foreach (var t in _trees.Keys) 
        {
            if (_trees[t])
                continue;
            float distance = Vector3.Distance(originPos, t.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = t.transform;
            }
        }
        _trees[target.gameObject] = true;
        return target;
    }
    public void AddTree(GameObject obj)
    {
        _trees[obj] = false;
    }
    public void DeleteTree(GameObject tree)
    {
        _trees.Remove(tree);
    }
    public void GenerateLogger()
    {
        ResData.Inst.GetResByAddress("Logger.prefab", (GameObject logger) =>
        {
            var trees = GameObject.FindGameObjectsWithTag("Plant");
            if (trees != null && trees.Length != 0)
                foreach (var t in trees) { _trees[t] = false; }
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
                }
            }
        }
        else
        {
            Debug.LogError("地面物体上找不到 MeshRenderer 组件！");
        }
    }
}
