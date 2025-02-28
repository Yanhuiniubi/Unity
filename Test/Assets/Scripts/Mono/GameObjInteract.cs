using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum eInteractionType
{
    None,
    PickUpGarbage,
    OpenBagFromDustbin,
    OpenShop,
    OpenAIChat,
}
public class GameObjInteract : MonoBehaviour
{
    private const string KEY_GARBAGE = UIDef.UI_INTRODUTION + "Garbage";
    private const string KEY_DUSTBIN = UIDef.UI_INTRODUTION + "Dustbin";
    private const string KEY_SHOP = UIDef.UI_INTRODUTION + "Shop";
    private const string KEY_AI = UIDef.UI_INTRODUTION + "AI";
    public Transform RayStartPos;
    public int RayDistance = 3;
    public LayerMask LayerMaskGarbage;//垃圾的layer
    public LayerMask LayerDustbin;//垃圾桶的layer
    public LayerMask LayerShopNpc;//商店npc的layer
    public LayerMask LayerRobot;//AI机器人的layer
    void Update()
    {
        if (GameMod.Inst.GameState == eGameState.OpenUI)
            return;
        CheckInteractObj();
        CheckOpenBag();
    }
    private LayerMask interact;
    private void Awake()
    {
        interact = LayerMaskGarbage | LayerDustbin |
            LayerShopNpc | LayerRobot;
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
        // 如果射线与任何游戏对象相交
        if (Physics.Raycast(ray, out hit, RayDistance, interact))
        {
            eInteractionType interactionType = eInteractionType.None;
            GameObject obj = hit.collider.gameObject;
            switch (obj.layer)
            {
                case 3://垃圾
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
                    break;
                case 6://垃圾桶
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
                    break;
                case 8://商店Npc
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
                    break;
                case 9://AI助手
                    {
                        if (!UIMod.Inst.IsActiveUI3D(KEY_AI))
                        {
                            UI3DInfo info = new UI3DInfo();
                            info.BasePos = obj.transform.position;
                            info.Height = GameMod.Inst.PlayerHeight;
                            info.Desc = "我可是环境知识大使！";
                            UIMod.Inst.Show3DUI<UIIntroDutionLogic>(UIDef.UI_INTRODUTION, "AI", info);
                        }
                        interactionType = eInteractionType.OpenAIChat;
                    }
                    break;
                default:
                    break;
            }
            
            _cacheHideUI = true;
            if (Input.GetKeyDown(KeyCode.F))
            {
                switch (interactionType)
                {
                    case eInteractionType.PickUpGarbage:
                        {
                            var cfg = GarbageData.Inst.GetGarbageCfgByObj(obj);
                            GarbageData.Inst.DeleteGarbage(obj);
                            TaskData.Inst.CheckTask(cfg.ID);
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
                    case eInteractionType.OpenAIChat:
                        {
                            UIMod.Inst.ShowUI<UIAIChatLogic>(UIDef.UI_AICHAT);
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
            UIMod.Inst.HideUI(KEY_AI);
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
