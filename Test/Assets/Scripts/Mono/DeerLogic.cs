using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerUIInfo
{
    public Vector3 BasePos;
    public float Height;
}

public class DeerLogic : MonoBehaviour
{
    const string NAME = "Deer";
    const string UIPATH = "Prefab/IntroDution";
    const string KEY = UIPATH + NAME;
    private CapsuleCollider _collider; 
    private void Start()
    {
        _collider = transform.Find("deer_001").gameObject.GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position,GameMod.Inst.PlayerPosition) <= 5)
        {
            if (!UIMod.Inst.IsActiveUI3D(KEY))
            {
                DeerUIInfo info = new DeerUIInfo();
                info.BasePos = gameObject.transform.position;
                info.Height = _collider.height;
                UIMod.Inst.Show3DUI<UIIntroDutionLogic>(UIPATH, NAME, info);
            }
        }
        else
        {
            if (UIMod.Inst.IsActiveUI3D(KEY))
            {
                UIMod.Inst.HideUI(KEY);
            }
        }
    }
}
