using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMod : MonoBehaviour
{
    private static GameMod inst;
    public static GameMod Inst => inst;
    public Transform UIRoot;
    public Transform UI3DRoot;
    public Vector3 PlayerPosition => gameObject.transform.position;
    private void Awake()
    {
        inst = this;
    }

    void Update()
    {
        
    }
}
