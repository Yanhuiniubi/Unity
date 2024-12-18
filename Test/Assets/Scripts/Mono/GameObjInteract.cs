using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjInteract : MonoBehaviour
{
    public Transform RayStartPos;
    public int RayDistance = 3;
    public LayerMask LayerMask;

    void Update()
    {
        CheckInteractObj();
        CheckGenerateGarbage();
        CheckOpenBag();
    }
    private void CheckGenerateGarbage()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            eGarbageType type = (eGarbageType)Random.Range(0, 4);
            GarbageData.Inst.GenerateGarbage(type);
        }
    }
    private void CheckInteractObj()
    {
        Ray ray = new Ray(RayStartPos.position, RayStartPos.forward);

        RaycastHit hit;

        // 如果射线与任何游戏对象相交
        if (Physics.Raycast(ray, out hit, RayDistance, LayerMask))
        {
            Debug.Log("射线检测到: " + hit.collider.gameObject.name);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GarbageData.Inst.DeleteGarbage(hit.collider.gameObject);
            }
        }
    }
    private void CheckOpenBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!UIMod.Inst.IsActiveUI(UIDef.UI_UIBAG))
                UIMod.Inst.ShowUI<UIBag>(UIDef.UI_UIBAG);
        }
    }
}
