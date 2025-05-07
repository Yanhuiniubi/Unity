using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameImage : GameUIComponent
{
    private Image _image;
    protected override void OnInit()
    {
        base.OnInit();
        _image = GetUIComponent<Image>();
    }
    public string Sprite
    {
        get
        {
            return _image.sprite.name;
        }
        set
        {
            _image.sprite = ResData.Inst.GetResByAddressPermanent<Sprite>(value);
        }
    }
}
