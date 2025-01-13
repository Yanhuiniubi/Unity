using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoadingLogic : MonoBehaviour
{
    public TextMeshProUGUI _txtLoading;
    public Slider _slider;
    public Transform _wait;
    public Button _btnTips;
    public Button _btnNewGame;
    public Button _btnLoadFile;
    public Animation _ani;

    public GameObject _tips;
    private void OnResStartLoad(string type,int TotalCnt)
    {
        _slider.minValue = 0;
        _slider.maxValue = TotalCnt;
        _slider.value = 0;
        _txtLoading.text = $"{type}正在加载中，请稍等（0/{TotalCnt}）";
    }
    private void OnResLoading(string type,int cnt,int TotalCnt)
    {
        _txtLoading.text = $"{type}正在加载中，请稍等（{cnt}/{TotalCnt}）";
        _slider.value = cnt;
    }
    private int _progress;
    private void OnResLoadFinish()
    {
        if (++_progress == 2)
        {
            SceneManager.LoadSceneAsync("GameScene");
        }
        if (_progress == 1)
            ResData.Inst.LoadPermanentAssetByLabel<Sprite>("PreLoadSprite");
    }

    private void Start()
    {
        _btnTips.onClick.AddListener(OnBtnTipsClick);
        _btnNewGame.onClick.AddListener(OnBtnNewGameClick);
        _btnLoadFile.onClick.AddListener(OnBtnLoadFileClick);
    }

    private void OnEnable()
    {
        _txtLoading.gameObject.SetActive(false);
        _wait.gameObject.SetActive(false);
        _slider.gameObject.SetActive(false);
        ResEvent.OnLoading += OnResLoading;
        ResEvent.OnStartLoad += OnResStartLoad;
        ResEvent.OnLoadFinish += OnResLoadFinish;
    }
    private void OnDisable()
    {
        ResEvent.OnLoading -= OnResLoading;
        ResEvent.OnStartLoad -= OnResStartLoad;
        ResEvent.OnLoadFinish -= OnResLoadFinish;
    }
    private void OnBtnTipsClick()
    {
        _tips.SetActive(true);
    }
    private void OnBtnNewGameClick()
    {
        _txtLoading.gameObject.SetActive(true);
        _wait.gameObject.SetActive(true);
        _slider.gameObject.SetActive(true);
        _btnNewGame.gameObject.SetActive(false);
        _btnLoadFile.gameObject.SetActive(false);
        _ani.Play();
        ResData.Inst.LoadPermanentAssetByLabel<GameObject>("PreLoadGameObject");
    }
    private void OnBtnLoadFileClick()
    {

    }
}
