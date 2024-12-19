using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eInteractionType
{
    None,
    PickUpGarbage,
    OpenBag
}
public class GameObjInteract : MonoBehaviour
{
    private const string KEY_GARBAGE = UIDef.UI_INTRODUTION + "Garbage";
    private const string KEY_DUSTBIN = UIDef.UI_INTRODUTION + "Dustbin";
    public Transform RayStartPos;
    public int RayDistance = 3;
    public LayerMask LayerMaskGarbage;
    public LayerMask LayerDustbin;
    private void Awake()
    {
        InitDustbinData();
    }
    private void InitDustbinData()
    {

    }
    void Update()
    {
        if (GameMod.Inst.GameState == eGameState.OpenUI)
            return;
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
        LayerMask interact = LayerMaskGarbage | LayerDustbin;
        // 如果射线与任何游戏对象相交
        if (Physics.Raycast(ray, out hit, RayDistance, interact))
        {
            eInteractionType interactionType = eInteractionType.None;
            GameObject obj = hit.collider.gameObject;
            if (obj.layer == 3)
            {
                if (!UIMod.Inst.IsActiveUI3D(KEY_GARBAGE))
                {
                    UI3DInfo info = new UI3DInfo();
                    info.BasePos = obj.transform.position;
                    info.Height = GameMod.Inst.PlayerHeight;
                    info.Desc = GarbageData.Inst.GetGarbageCfgByObj(obj).Name;
                    UIMod.Inst.Show3DUI<UIIntroDutionLogic>(UIDef.UI_INTRODUTION, "Garbage", info);
                }
                interactionType = eInteractionType.PickUpGarbage;
            }
            else if (obj.layer == 6)
            {
                if (!UIMod.Inst.IsActiveUI3D(KEY_DUSTBIN))
                {
                    UI3DInfo info = new UI3DInfo();
                    info.BasePos = obj.transform.position;
                    info.Height = GameMod.Inst.PlayerHeight;
                    info.Desc = DustbinData.Inst.GetDustbinCfgByObj(obj).Desc;
                    UIMod.Inst.Show3DUI<UIIntroDutionLogic>(UIDef.UI_INTRODUTION, "Dustbin", info);
                }
                interactionType = eInteractionType.OpenBag;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                switch (interactionType)
                {
                    case eInteractionType.PickUpGarbage:
                        {
                            GarbageData.Inst.DeleteGarbage(obj);
                        }
                        break;
                    case eInteractionType.OpenBag:
                        {
                            UIMod.Inst.ShowUI<UIBag>(UIDef.UI_UIBAG, (eOpenBagFrom)
                                DustbinData.Inst.GetDustbinCfgByObj(obj).Type);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            UIMod.Inst.HideUI(KEY_DUSTBIN);
            UIMod.Inst.HideUI(KEY_GARBAGE);
        }
    }
    private void CheckOpenBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!UIMod.Inst.IsActiveUI(UIDef.UI_UIBAG))
                UIMod.Inst.ShowUI<UIBag>(UIDef.UI_UIBAG, eOpenBagFrom.BagKey);
        }
    }
}
