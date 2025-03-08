using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// �������ɹ�����  
/// </summary>
public class GarbageGenerateManager
{
    public static GarbageGenerateManager Inst = new GarbageGenerateManager();
    private GarbageGenerateManager()
    {

    }
    public void GenerateGarbageFromTask(List<Vector3> garbagePos = null)
    {
        var data = TaskData.Inst.CurTask;
        var count = data.Count - TaskData.Inst.Process;
        var groundObject = GameMod.Inst.GarbageRoundBox.gameObject;
        MeshRenderer meshRenderer = groundObject.GetComponent<MeshRenderer>();
        if (garbagePos == null || garbagePos.Count == 0)
        {
            if (meshRenderer != null)
            {
                Bounds bounds = meshRenderer.bounds;
                Vector3 min = bounds.min;
                Vector3 max = bounds.max;
                LayerMask _layerMask = GameMod.Inst._groundMask;
                for (int i = 0; i < count; i++)
                {

                    float x = Random.Range(min.x, max.x);
                    float z = Random.Range(min.z, max.z);
                    Vector3 origin = new Vector3(x, bounds.max.y + 1, z); // �ڵ�����ߵ��Ϸ���������
                    RaycastHit hit;

                    if (Physics.Raycast(origin, Vector3.down, out hit, maxDistance: 1000, layerMask: _layerMask))
                    {
                        Vector3 spawnPosition = hit.point;
                        GarbageData.Inst.GenerateGarbage(data.Param1, spawnPosition);
                    }
                }
            }
            else
            {
                Debug.LogError("�����������Ҳ��� MeshRenderer �����");
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                GarbageData.Inst.GenerateGarbage(data.Param1, garbagePos[i]);
            }
        }
    }
}
