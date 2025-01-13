using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AssetInfo
{
    public AsyncOperationHandle Handle;
    public int ReferCount;
}
public static class ResEvent
{
    public static Action<string,int> OnStartLoad;

    public static Action<string,int,int> OnLoading;

    public static Action OnLoadFinish;
}
public class ResData
{
    public static readonly ResData Inst = new ResData();
    private Dictionary<string, AssetInfo> _resDic_normal;
    private Dictionary<string, object> _resDic_permanent;
    private ResData()
    {
        _resDic_normal = new Dictionary<string, AssetInfo>();
        _resDic_permanent = new Dictionary<string, object>();
    }
    /// <summary>
    /// 根据地址获取资源（不用时卸载）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="addresses"></param>
    /// <param name="address"></param>
    /// <param name="callBack"></param>
    public void GetResByAddress<T>(HashSet<string> addresses,string address,Action<T> callBack) where T : UnityEngine.Object
    {
        if (_resDic_normal.TryGetValue(address, out var res))
        {
            if (!addresses.Contains(address))
            {
                addresses.Add(address);
                res.ReferCount++;
            }
            callBack?.Invoke(res.Handle.Result as T);
        }
        else
        {
            AssetInfo info = new AssetInfo();
            Addressables.LoadAssetAsync<T>(address).Completed += h =>
            {
                if (h.Status == AsyncOperationStatus.Succeeded)
                {
                    info.Handle = h;
                    info.ReferCount = 1;
                    addresses.Add(address);
                    callBack?.Invoke(h.Result);
                    _resDic_normal[address] = info;
                }
            };
        }
    }
    /// <summary>
    /// 获取常驻内存中的资源（垃圾等）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <returns></returns>
    public T GetResByAddressPermanent<T>(string address) where T : UnityEngine.Object
    {
        if (_resDic_permanent.TryGetValue(address, out var res))
        {
            return res as T;
        }
        return null;
    }
    /// <summary>
    /// 加载常驻内存中的资源（UI等）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    public void LoadPermanentAssetByLabel<T>(string label) where T : UnityEngine.Object
    {
        Addressables.LoadResourceLocationsAsync(label).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var locations = handle.Result;

                int loadCnt = 0;
                int loadTotalCnt = locations.Count;
                ResEvent.OnStartLoad?.Invoke(typeof(T).Name, loadTotalCnt);
                foreach (var location in locations)
                {
                    Addressables.LoadAssetAsync<T>(location.PrimaryKey).Completed += assetHandle =>
                    {
                        if (assetHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            loadCnt++;
                            _resDic_permanent[location.PrimaryKey] = assetHandle.Result;
                            ResEvent.OnLoading?.Invoke(typeof(T).Name, loadCnt, loadTotalCnt);
                            if (loadCnt == loadTotalCnt)
                                ResEvent.OnLoadFinish?.Invoke();
                        }
                    };
                }
            }
        };
    }
    public void ReleasAsset(HashSet<string> addresses)
    {
        foreach (var address in addresses)
        {
            if (_resDic_normal.TryGetValue(address,out var value))
            {
                if (--value.ReferCount == 0)
                {
                    value.Handle.Release();
                    _resDic_normal.Remove(address);
                }
            }
        }
    }
}
