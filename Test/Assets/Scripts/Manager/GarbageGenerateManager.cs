using System.Collections;
using System.Collections.Generic;
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
    public void GenerateGarbage(int count)
    {
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

                float x = Random.Range(min.x, max.x);
                float z = Random.Range(min.z, max.z);                                                           
                Vector3 origin = new Vector3(x, bounds.max.y + 1, z); // �ڵ�����ߵ��Ϸ���������
                RaycastHit hit;

                if (Physics.Raycast(origin, Vector3.down, out hit,maxDistance:1000,layerMask: _layerMask))
                {
                    Vector3 spawnPosition = hit.point;
                    eGarbageType type = (eGarbageType)Random.Range(0, 4);
                    GarbageData.Inst.GenerateGarbage(type, spawnPosition);
                }
            }
        }
        else
        {
            Debug.LogError("�����������Ҳ��� MeshRenderer �����");
        }
    }
}
