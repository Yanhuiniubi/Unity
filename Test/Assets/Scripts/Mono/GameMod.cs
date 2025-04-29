using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGameState
{
    Normal,
    OpenUI
}
public enum eOpenGameStyle
{
    NewGame,
    LoadData
}

public class GameMod : MonoBehaviour
{
    private static GameMod inst;
    public static GameMod Inst => inst;
    public Transform UIRoot;
    public Transform UI3DRoot;
    public Vector3 PlayerPosition => gameObject.transform.position;
    public GameObject PlayerObj => gameObject;
    public Transform GarbageRoundBox;
    public Transform GarbageRoot;
    public Transform DustbinRoot;
    public Transform UITipsRoot;
    public Transform RoundRoot;
    public LayerMask _groundMask;
    private CapsuleCollider _capsuleCollider;

    public Transform LoggerRoot;
    public Camera UICamera;
    public float PlayerHeight
    {
        get
        {
            if (_capsuleCollider == null)
                _capsuleCollider = transform.Find("common_people_male_1").GetComponent<CapsuleCollider>();
            return _capsuleCollider.height;
        }
    }
    private eGameState _gameState;
    /// <summary>
    /// 游戏状态
    /// </summary>
    public eGameState GameState => _gameState;
    public static eOpenGameStyle eOpenGameStyle;
    private void Awake()
    { 
        inst = this;
        _gameState = eGameState.Normal;
        DustbinData.Inst.InitData();
    }
    private void Start()
    {
        UIMod.Inst.ShowUI<UIMain>(UIDef.UI_MAIN, changeGameState: false);
        if (eOpenGameStyle == eOpenGameStyle.NewGame)
        {
            var arr = TableItemMainMod.Array;
            foreach (var item in arr)
            {
                BagData.Inst.AddItem(item, 3);
            }
            TaskData.Inst.InitCurTask();
        }
        else
        {
            StoreDataMod.Inst.LoadData();
            UIMod.Inst.ShowUI<UIStoreData>(UIDef.UI_StoreData, "正在读取档案，请稍等。。");
        }
    }
    /// <summary>
    /// 设置游戏状态
    /// </summary>
    /// <param name="state"></param>
    public void SetGameState(eGameState state)
    {
        _gameState = state;
        if (state == eGameState.Normal)
            Cursor.lockState = CursorLockMode.Locked;
        else if (state == eGameState.OpenUI)
            Cursor.lockState = CursorLockMode.None;
    }
}
