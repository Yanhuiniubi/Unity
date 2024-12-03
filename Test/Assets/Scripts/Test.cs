using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TableRole cfg = TableRoleMod.Get(1);
        Debug.Log(cfg.Name);
    }

    
}
