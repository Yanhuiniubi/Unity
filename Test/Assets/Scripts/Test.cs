using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TableAnimal cfg = TableAnimalMod.Get(1);
        Debug.Log(cfg.Name);
        TableAnimal cfg1 = TableAnimalMod.Get(2);
        Debug.Log(cfg1.Name);
    }

    
}
