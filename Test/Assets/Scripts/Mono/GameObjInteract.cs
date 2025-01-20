using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eInteractionType
{
    None,
    PickUpGarbage,
    OpenBagFromDustbin,
    OpenShop,
}
public class GameObjInteract : MonoBehaviour
{
    private const string KEY_GARBAGE = UIDef.UI_INTRODUTION + "Garbage";
    private const string KEY_DUSTBIN = UIDef.UI_INTRODUTION + "Dustbin";
    private const string KEY_SHOP = UIDef.UI_INTRODUTION + "Shop";
    public Transform RayStartPos;
    public int RayDistance = 3;
    public LayerMask LayerMaskGarbage;//垃圾的layer
    public LayerMask LayerDustbin;//垃圾桶的layer
    public LayerMask LayerShopNpc;//商店npc的layer
    void Update()
    {
        if (GameMod.Inst.GameState == eGameState.OpenUI)
            return;
        CheckInteractObj();
        CheckOpenBag();
    }
    private string _cacheLastGarbageName;
    private bool _cacheHideUI;
    /// <summary>
    /// 射线检测可交互的物体
    /// </summary>
    private void CheckInteractObj()
    {
        Ray ray = new Ray(RayStartPos.position, RayStartPos.forward);

        RaycastHit hit; 
        LayerMask interact = LayerMaskGarbage | LayerDustbin | LayerShopNpc;
        // 如果射线与任何游戏对象相交
        if (Physics.Raycast(ray, out hit, RayDistance, interact))
        {
            eInteractionType interactionType = eInteractionType.None;
            GameObject obj = hit.collider.gameObject;
            if (obj.layer == 3)//垃圾
            {
                var cfg = GarbageData.Inst.GetGarbageCfgByObj(obj);
                if (!UIMod.Inst.IsActiveUI3D(KEY_GARBAGE) || !cfg.Name.Equals(_cacheLastGarbageName))
                {
                    UIMod.Inst.HideUI(KEY_GARBAGE);
                    UI3DInfo info = new UI3DInfo();
                    info.BasePos = obj.transform.position;
                    info.Height = GameMod.Inst.PlayerHeight;
                    info.Desc = cfg.Name;
                    _cacheLastGarbageName = info.Desc;
                    UIMod.Inst.Show3DUI<UIIntroDutionLogic>(UIDef.UI_INTRODUTION, "Garbage", info);
                }
                interactionType = eInteractionType.PickUpGarbage;
            }
            else if (obj.layer == 6)//垃圾桶
            {
                if (!UIMod.Inst.IsActiveUI3D(KEY_DUSTBIN))
                {
                    UI3DInfo info = new UI3DInfo();
                    info.BasePos = obj.transform.position;
                    info.Height = GameMod.Inst.PlayerHeight;
                    info.Desc = DustbinData.Inst.GetDustbinCfgByObj(obj).Desc;
                    UIMod.Inst.Show3DUI<UIIntroDutionLogic>(UIDef.UI_INTRODUTION, "Dustbin", info);
                }
                interactionType = eInteractionType.OpenBagFromDustbin;
            }
            else if (obj.layer == 8)//商店npc
            {
                if (!UIMod.Inst.IsActiveUI3D(KEY_SHOP))
                {
                    UI3DInfo info = new UI3DInfo();
                    info.BasePos = obj.transform.position;
                    info.Height = (hit.collider as BoxCollider).size.y;
                    info.Desc = "我的商店能让你口袋掏空！";
                    UIMod.Inst.Show3DUI<UIIntroDutionLogic>(UIDef.UI_INTRODUTION, "Shop", info);
                }
                interactionType = eInteractionType.OpenShop;
            }
            _cacheHideUI = true;
            if (Input.GetKeyDown(KeyCode.F))
            {
                switch (interactionType)
                {
                    case eInteractionType.PickUpGarbage:
                        {
                            GarbageData.Inst.DeleteGarbage(obj);
                        }
                        break;
                    case eInteractionType.OpenBagFromDustbin:
                        {
                            UIMod.Inst.ShowUI<UIBag>(UIDef.UI_UIBAG, (eOpenBagFrom)
                                DustbinData.Inst.GetDustbinCfgByObj(obj).Type);
                        }
                        break;
                    case eInteractionType.OpenShop:
                        {
                            UIMod.Inst.ShowUI<UIShop>(UIDef.UI_SHOP);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else if (_cacheHideUI)
        {
            UIMod.Inst.HideUI(KEY_DUSTBIN);
            UIMod.Inst.HideUI(KEY_GARBAGE);
            UIMod.Inst.HideUI(KEY_SHOP);
            _cacheHideUI = false;
        }
    }
    /// <summary>
    /// 打开背包
    /// </summary>
    private void CheckOpenBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!UIMod.Inst.IsActiveUI(UIDef.UI_UIBAG))
                UIMod.Inst.ShowUI<UIBag>(UIDef.UI_UIBAG, eOpenBagFrom.BagKey);
        }
    }
}
