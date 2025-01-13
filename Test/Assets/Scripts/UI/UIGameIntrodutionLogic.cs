using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameIntrodutionLogic : MonoBehaviour
{
    public Button _btnClose;

    private void Start()
    {
        _btnClose.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
