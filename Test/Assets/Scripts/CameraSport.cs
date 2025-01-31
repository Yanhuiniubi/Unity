using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSport
{
    public static void StartRotate(Camera camera, Transform target,Action OnFinish, float rotationSpeed = 120f, float
        radius = 5f, float angle = 0f)
    {
        GameMod.Inst.SetGameState(eGameState.OpenUI);
        GameMod.Inst.StartCoroutine(StartRotateIE(camera, target, OnFinish,rotationSpeed, radius, angle));
    }
    static IEnumerator StartRotateIE(Camera camera, Transform target, Action OnFinish,float rotationSpeed = 120f, float
        radius = 5f,float angle = 0f)
    {
        while (true)
        {
            angle += rotationSpeed * Time.deltaTime;

            // 让角度在 0 到 360 度之间循环
            if (angle >= 360f)
            {
                angle = 0f;
                GameMod.Inst.SetGameState(eGameState.Normal);
                OnFinish?.Invoke();
                yield break;
            }

            // 计算摄像机的位置
            float x = target.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = target.position.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = target.position.y + 2f; // 可以调整这个值，决定摄像机的高度

            // 更新摄像机的位置
            camera.transform.position = new Vector3(x, y, z);

            // 始终让摄像机朝向目标
            camera.transform.LookAt(target);
            yield return null;
        }
    }
}
