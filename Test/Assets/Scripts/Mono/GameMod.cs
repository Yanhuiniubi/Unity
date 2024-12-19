using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGameState
{
    Normal,
    OpenUI
}

public class GameMod : MonoBehaviour
{
    private static GameMod inst;
    public static GameMod Inst => inst;
    public Transform UIRoot;
    public Transform UI3DRoot;
    public Vector3 PlayerPosition => gameObject.transform.position;
    public GameObject PlayerObj => gameObject;
    public Transform GarbageRoot;
    public Transform DustbinRoot;
    private CapsuleCollider _capsuleCollider;
    public float PlayerHeight
    {
        get
        {
            if (_capsuleCollider == null)
                _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            return _capsuleCollider.height;
        }
    }
    private eGameState _gameState;
    public eGameState GameState => _gameState;
    private void Awake()
    {
        inst = this;
        _gameState = eGameState.Normal;
        DustbinData.Inst.InitData();
    }
    public void SetGameState(eGameState state)
    {
        _gameState = state;
    }
}
