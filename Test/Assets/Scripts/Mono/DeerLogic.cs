using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI3DInfo
{
    public Vector3 BasePos;
    public float Height;
    public TableAnimal Cfg;
}

public class DeerLogic : MonoBehaviour
{
    const string NAME = "Deer";
    const string UIPATH = UIDef.UI_INTRODUTION;
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
                UI3DInfo info = new UI3DInfo();
                info.BasePos = gameObject.transform.position;
                info.Height = _collider.height;
                info.Cfg = TableAnimalMod.Get(NAME);
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
