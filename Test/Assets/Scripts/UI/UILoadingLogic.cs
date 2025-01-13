using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoadingLogic : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _txtLoading;

    [SerializeField]
    private Slider _slider;
    void Start()
    {
        _txtLoading.text = "��Դ���ڼ����У����Եȣ�0/2��";
        ResEvent.OnLoading += OnResLoading;
        ResEvent.OnStartLoad += OnResStartLoad;
        ResEvent.OnLoadFinish += OnResLoadFinish;
        ResData.Inst.LoadPermanentAssetByLabel<GameObject>("PreLoadGameObject", null);
        ResData.Inst.LoadPermanentAssetByLabel<Sprite>("PreLoadSprite", null);
    }
    private void OnResStartLoad(string type,int TotalCnt)
    {
        _slider.minValue = 0;
        _slider.maxValue = TotalCnt;
        _txtLoading.text = $"{type}���ڼ����У����Եȣ�0/{TotalCnt}��";
    }
    private void OnResLoading(string type,int cnt,int TotalCnt)
    {
        _txtLoading.text = $"{type}���ڼ����У����Եȣ�{cnt}/{TotalCnt}��";
        _slider.value = cnt;
    }
    private int _progress;
    private void OnResLoadFinish()
    {
        if (++_progress == 2)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
