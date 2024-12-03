using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMod : MonoBehaviour
{
    private static GameMod inst;
    public static GameMod Inst => inst;
    public Transform UIRoot;
    private void Awake()
    {
        inst = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UIMod.Inst.ShowUI<RedLogic>("Prefab/Red");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIMod.Inst.HideUI();
        }
    }
}
